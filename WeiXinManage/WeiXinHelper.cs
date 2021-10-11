using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Net;
using System.IO;
using Common.Library.Log;
using System.Xml.Linq;
using ChinaBM.Common;
using System.Web;
using WeiXinManage.Model;
using WeiXinManage.Model.Card;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace WeiXinManage
{
    public class WeiXinHelper
    {
        #region 基础方法块

        /// <summary>
        /// 安全验证
        /// </summary>
        /// <returns></returns>
        public static bool Valid(string signature, string timestamp, string nonce, string token)
        {
            string[] ArrTmp = { token, timestamp, nonce };
            Array.Sort(ArrTmp);     //字典排序  
            string tmpStr = string.Join("", ArrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            return tmpStr == signature;
        }

        /// <summary>
        /// 获取jsapi的安全码
        /// </summary>
        /// <param name="ticket"></param>
        /// <param name="noncestr"></param>
        /// <param name="timestamp"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string JSApiSigNature(string ticket, string noncestr, string timestamp, string url)
        {
            var tmpStr = string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", ticket, noncestr, timestamp, url);
            //Logger.Error("", tmpStr + "      urlraw:" + System.Web.HttpContext.Current.Request.RawUrl, null);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");
            tmpStr = tmpStr.ToLower();
            return tmpStr;
        }

        /// <summary>
        /// 返回AccessToken
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="secret"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static string GetAccessToken(string appid, string secret, out string error)
        {
            error = "";
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                var s = client.DownloadString(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appid, secret));

                var o = s.JsonToDictionary();

                if (o.ContainsKey("errcode"))
                {
                    error += o["errcode"] + "\r\n" + o["errmsg"];
                    return string.Empty;
                }

                return o["access_token"].ToString();
            }
        }

        /// <summary>
        /// 删除自定义菜单
        /// </summary>
        /// <param name="access_token">access_token</param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool DeleteMenu(string access_token, out string error)
        {
            error = "";
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}", access_token);
            try
            {
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    var s = client.DownloadString(url);

                    var o = s.JsonToDictionary();

                    if (o["errcode"].ToInt(-1) != 0)
                    {
                        error += o["errcode"] + "\n\r" + o["errmsg"];
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return false;
            }

        }

        /// <summary>
        /// 创建自定义菜单
        /// </summary>
        /// <param name="buttonstring"></param>
        /// <param name="access_token"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool CreateMenu(string buttonstring, string access_token, out string error)
        {
            error = "";

            var url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", access_token);

            try
            {
                var srcString = HttpKit.HttpPostSubmit(buttonstring, url);

                var o = srcString.JsonToDictionary();

                if (o.ContainsKey("errcode") && o["errcode"].ToString() != "0")
                {
                    error += o["errcode"] + "\r\n" + o["errmsg"];
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                error += e.Message;
                return false;
            }
        }

        /// <summary>
        /// 获取自定义菜单
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static string GetMenu(string access_token, out string error)
        {
            error = "";

            var url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", access_token);

            try
            {
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    var s = client.DownloadString(url);

                    var o = s.JsonToDictionary();

                    if (o != null && o.ContainsKey("errcode"))
                    {
                        error += o["errcode"] + "\r\n" + o["errmsg"];
                        return "";
                    }
                    else
                    {
                        return s;
                    }
                }
            }
            catch (Exception e)
            {
                error += e.Message;
                return "";
            }
        }

        /// <summary>
        /// 返回微信接收数据
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> GetPostXMLData()
        {
            try
            {
                var document = XElement.Load(System.Web.HttpContext.Current.Request.InputStream);

                var filepath = HttpKit.GetMapPath("/visit/" + DateTime.Now.ToString("yyyyMMdd") + ".txt");

                DirectoryEx.CreateFolder(filepath);

                using (var write = new StreamWriter(filepath, true))
                {
                    write.WriteLine(HttpKit.Url);
                    write.Write(document.ToString());
                    write.Write("\r\n\r\n");
                }

                return document.Descendants().ToDictionary(a => a.Name.ToString(), a => a.Value);
            }
            catch (Exception e)
            {
                Logger.Error(typeof(WeiXinHelper), e.Message, e);
            }
            return new Dictionary<string, string>();
        }

        /// <summary>
        /// 返回oauth验证url
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="url"></param>
        /// <param name="state"></param>
        /// <param name="scope">应用授权作用域，snsapi_base （不弹出授权页面，直接跳转，只能获取用户openid），snsapi_userinfo （弹出授权页面</param>
        /// <returns></returns>
        public static string GetOAuthLink(string appid, string url, string state, string scope = "snsapi_base")
        {
            state = System.Web.HttpContext.Current.Server.UrlEncode(state);
            return string.Format("https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state={3}#wechat_redirect",
                appid,
                url,
                scope,
                state);
        }

        /// <summary>
        /// 通过oauth 验证得到的code 获取openid
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="secret"></param>
        /// <param name="code"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static W_OAuthModel GetOAuthModelByCode(string appid, string secret, string code, out string error)
        {
            error = "";

            var oauthm = new W_OAuthModel();

            var url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", appid, secret, code);

            try
            {
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    var s = client.DownloadString(url);

                    var o = s.JsonToDictionary();

                    if (o.ContainsKey("access_token"))
                    {
                        oauthm.Auth_Access_Token = o["access_token"].ToSafeString();
                    }

                    if (o.ContainsKey("openid"))
                    {
                        oauthm.OpenId = o["openid"].ToSafeString();
                    }

                    if (o.ContainsKey("errcode"))
                    {
                        error += o["errcode"] + "\r\n" + o["errmsg"];
                    }

                }
            }
            catch (Exception e)
            {
                error += e.Message;
                return null;
            }
            return oauthm.Valid() ? oauthm : null;
        }

        /// <summary>
        /// 通过code获取用户信息
        /// </summary>
        /// <param name="appid"></param>
        /// <param name="secret"></param>
        /// <param name="code"></param>
        /// <param name="access_token"></param>
        /// <param name="error"></param>
        /// <param name="scope">应用授权作用域，snsapi_base （不弹出授权页面，直接跳转，只能获取用户openid），snsapi_userinfo （弹出授权页面，可通过openid拿到昵称、性别、所在地。并且，即使在未关注的情况下，只要用户授权，也能获取其信息）</param>
        /// <returns></returns>
        public static W_User_Info GetUserInfo(string appid, string secret, string code, string access_token, out string error, string scope = "snsapi_base")
        {
            error = "";

            var url = string.Format("https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", appid, secret, code);

            try
            {
                var auth_access_token = "";
                var oauthm = GetOAuthModelByCode(appid, secret, code, out error);
                var openid = oauthm != null ? oauthm.OpenId : "";

                if (scope == "snsapi_base")
                {
                    if (!String.IsNullOrEmpty(openid))
                    {
                        return GetUserInfoMustSubscribe(openid, access_token, out error);
                    }
                }
                else
                {
                    if (!String.IsNullOrEmpty(auth_access_token) && !String.IsNullOrEmpty(openid))
                    {
                        return GetUserInfo(auth_access_token: auth_access_token, openid: openid, error: out error);
                    }
                }
            }
            catch (Exception e)
            {
                error += e.Message;
                return null;
            }

            return null;
        }

        /// <summary>
        /// 通过auth_access_token，返回用户信息，需要scope为snsapi_userinfo的授权
        /// </summary>
        /// <param name="auth_access_token"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static W_User_Info GetUserInfo(string auth_access_token, string openid, out string error)
        {
            error = "";

            var url = string.Format("https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN", auth_access_token, openid);

            return GetUserInfo(url, ref error);
        }

        /// <summary>
        /// 通过auth_refresh_token获取用户信息
        /// </summary>
        /// <param name="refresh_token"></param>
        /// <param name="appid"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static W_User_Info GetUserInfoByRefresh(string refresh_token, string appid, out string error)
        {
            error = "";

            var url = string.Format("https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}", appid, refresh_token);

            try
            {
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    var s = client.DownloadString(url);

                    var o = s.JsonToDictionary();

                    if (o.ContainsKey("access_token") && o.ContainsKey("openid"))
                    {
                        return GetUserInfo(auth_access_token: o["access_token"].ToSafeString(), openid: o["openid"].ToSafeString(), error: out error);
                    }

                    if (o.ContainsKey("errcode"))
                    {
                        error += o["errcode"] + "\r\n" + o["errmsg"];
                    }
                    return null;
                }
            }
            catch (Exception e)
            {
                error += e.Message;
                return null;
            }
        }

        /// <summary>
        /// 获取微信用户信息(需关注)
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="access_token"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static W_User_Info GetUserInfoMustSubscribe(string openid, string access_token, out string error)
        {
            error = "";

            var url = string.Format("https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", access_token, openid);

            return GetUserInfo(url, ref error);
        }

        /// <summary>
        /// 获取jsapiticket
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static string GetJsapiTicket(string access_token, out string error)
        {
            error = "";
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", access_token);
            try
            {
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    var s = client.DownloadString(url);

                    var o = s.JsonToDictionary();

                    if (o.ContainsKey("errcode") && o["errcode"].ToString() == "0" && o.ContainsKey("ticket"))
                    {
                        return o["ticket"].ToString();
                    }

                    if (o.ContainsKey("errcode") && o["errcode"].ToString() != "0")
                    {
                        error += o["errcode"] + "\r\n" + o["errmsg"];
                    }
                    return "";
                }
            }
            catch (Exception e)
            {
                error += e.Message;
                return null;
            }
        }

        /// <summary>
        /// 判断用户是否关注
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="access_token"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool IsAttention(string openid, string access_token, out string error)
        {
            error = "";

            var m = GetUserInfoMustSubscribe(openid, access_token, out error);

            return m != null && !String.IsNullOrEmpty(m.NickName);
        }

        /// <summary>
        /// 返回当前菜单Json字符串
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static string GetMenuJsonString(string access_token, out string error)
        {
            error = "";

            var url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", access_token);

            try
            {
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    var s = client.DownloadString(url);

                    var o = s.JsonToDictionary();

                    if (o.ContainsKey("errcode") && o["errcode"].ToInt(-1) != 0)
                    {
                        error += o["errcode"] + "\n\r" + o["errmsg"];
                        return "";
                    }

                    return s;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return "";
            }
        }

        /// <summary>
        /// 判断当前公众号类型(1:订阅号,2:服务号)
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static int? GetType(string access_token, out bool isauth)
        {
            isauth = false;
            var error = "";

            isauth = IsAuth(access_token);

            if (isauth)
            {
                var s = ShortUrl(access_token, "http://www.baidu.com/", out error);
            }
            else
            {
                var s = GetMenuJsonString(access_token, out error);
            }

            if (!String.IsNullOrEmpty(error) && (error.StartsWith("48001") || error.StartsWith("50001")))
            {
                return 1;
            }

            return 2;
        }

        /// <summary>
        /// 长链接转短链
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="changeurl"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static string ShortUrl(string access_token, string changeurl, out string error)
        {
            error = "";

            var url = string.Format("https://api.weixin.qq.com/cgi-bin/shorturl?access_token={0}", access_token);

            try
            {
                var srcString = HttpKit.HttpPostSubmit(new { action = "long2short", long_url = changeurl }.ToJson(), url);

                var o = srcString.JsonToDictionary();

                if (o.ContainsKey("errcode") && o["errcode"].ToSafeString() != "0")
                {
                    error += o["errcode"] + "\r\n" + o["errmsg"];
                    return "";
                }

                return o["short_url"].ToSafeString();
            }
            catch (Exception e)
            {
                error += e.Message;
                return "";
            }
        }

        /// <summary>
        /// 获取关注用户openid列表json字符串
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="next_openid"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static string GetAttentionUserList(string access_token, string next_openid, out string error)
        {
            error = "";

            var url = string.Format("https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid={1}", access_token, next_openid);

            try
            {
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    var s = client.DownloadString(url);

                    var o = s.JsonToDictionary();

                    if (o.ContainsKey("errcode") && o["errcode"].ToInt(-1) != 0)
                    {
                        error += o["errcode"] + "\n\r" + o["errmsg"];
                        return "";
                    }

                    return s;
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return "";
            }
        }

        /// <summary>
        /// 判断是否认证
        /// </summary>
        /// <param name="access_token"></param>
        /// <returns></returns>
        public static bool IsAuth(string access_token)
        {
            var error = "";

            var s = GetAttentionUserList(access_token, "", out error);

            return !String.IsNullOrEmpty(s);
        }

        /// <summary>
        /// 上传多媒体文件,返回 MediaId 
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="type"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static string UploadFile(string access_token, string filepath, FileTypeEnum type, out string error)
        {
            error = "";

            if (!File.Exists(filepath))
            {
                return "";
            }

            var url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", access_token, typeof(FileTypeEnum).GetEnumName(type));

            try
            {
                using (var client = new WebClient())
                {
                    client.Credentials = CredentialCache.DefaultCredentials;
                    return Encoding.UTF8.GetString(client.UploadFile(url, "POST", filepath));
                }
            }
            catch (Exception e)
            {
                error += e.Message;
                return "";
            }
        }

        /// <summary>
        /// 异步上传文件，MediaId在callback中去获取
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="filepath"></param>
        /// <param name="type"></param>
        /// <param name="error"></param>
        /// <param name="callback"></param>
        public static void UploadFileAsync(string access_token, string filepath, FileTypeEnum type, out string error, Action<object, UploadFileCompletedEventArgs> callback)
        {
            error = "";

            if (!File.Exists(filepath))
            {
                error = "filepath error";
                return;
            }

            var url = string.Format("http://file.api.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}", access_token, typeof(FileTypeEnum).GetEnumName(type));

            try
            {
                using (var client = new WebClient())
                {
                    client.Credentials = CredentialCache.DefaultCredentials;
                    client.UploadFileCompleted += new UploadFileCompletedEventHandler(callback);
                    client.UploadFile(url, "POST", filepath);
                    return;
                }
            }
            catch (Exception e)
            {
                error += e.Message;
                return;
            }
        }

        #endregion


        #region 微信卡劵块


        /// <summary>
        /// 获取微信卡apiticket
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static string GetCardApiTicket(string access_token, out string error)
        {
            error = "";
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=wx_card", access_token);
            try
            {
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    var s = client.DownloadString(url);

                    var o = s.JsonToDictionary();

                    if (o.ContainsKey("errcode") && o["errcode"].ToString() == "0" && o.ContainsKey("ticket"))
                    {
                        return o["ticket"].ToString();
                    }

                    if (o.ContainsKey("errcode") && o["errcode"].ToString() != "0")
                    {
                        error += o["errcode"] + "\r\n" + o["errmsg"];
                    }
                    return "";
                }
            }
            catch (Exception e)
            {
                error += e.Message;
                return null;
            }
        }


        /// <summary>
        /// 获取用户卡劵列表信息
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openid"></param>
        /// <param name="card_id"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static string GetCardCodeByOpenId(string access_token, string openid, string card_id, out string error)
        {
            error = "";
            var url = string.Format("https://api.weixin.qq.com/card/user/getcardlist?access_token={0}", access_token);
            try
            {
                var s = HttpKit.HttpPostSubmit(new { openid = openid, card_id = card_id }.ToJson(), url);
                var o = s.JsonToDictionary();

                if (o.ContainsKey("errcode") && o["errcode"].ToString() == "0" && o.ContainsKey("card_list"))
                {
                    var dic = (o["card_list"] as object[])[0] as Dictionary<string, object>;

                    if (dic["card_id"].ToSafeString() == card_id)
                    {
                        return dic["code"].ToSafeString();
                    }
                    else
                    {
                        return "";
                    }
                }

                if (o.ContainsKey("errcode") && o["errcode"].ToString() != "0")
                {
                    error += o["errcode"] + "\r\n" + o["errmsg"];
                }
                return "";
            }
            catch (Exception e)
            {
                error += e.Message;
                return null;
            }
        }

        /// <summary>
        /// 设置是否需要积分
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="card_id"></param>
        /// <param name="isneed"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool UpdateNeedIntegral(string access_token, string card_id, bool isneed, out string error)
        {
            error = "";
            var url = string.Format("https://api.weixin.qq.com/card/update?access_token={0}", access_token);
            try
            {
                var jsonstr = new
                {
                    card_id = card_id,
                    member_card = new
                    {
                        supply_bonus = isneed
                    }

                }.ToJson();
                var s = HttpKit.HttpPostSubmit(jsonstr, url);
                var o = s.JsonToDictionary();

                if (o.ContainsKey("errcode") && o["errcode"].ToString() != "0")
                {
                    error += o["errcode"] + "\r\n" + o["errmsg"];
                }
                return true;
            }
            catch (Exception e)
            {
                error += e.Message;
                return false;
            }

        }

        /// <summary>
        /// 设置更新积分和金额信息
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="m"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool SetUpdateUserInfo(string access_token, UpdateUserInfoModel m, out string error)
        {

            error = "";
            var url = string.Format("https://api.weixin.qq.com/card/membercard/updateuser?access_token={0}", access_token);
            try
            {
                var jsonstr = m.ToJson(out error);

                if (!String.IsNullOrWhiteSpace(error))
                {
                    return false;
                }

                var s = HttpKit.HttpPostSubmit(jsonstr, url);
                var o = s.JsonToDictionary();

                if (o.ContainsKey("errcode") && o["errcode"].ToString() != "0")
                {
                    error += o["errcode"] + "\r\n" + o["errmsg"];
                }
                return true;
            }
            catch (Exception e)
            {
                error += e.Message;
                return false;
            }
        }


        /// <summary>
        /// 打开扫码支付和储蓄部分
        /// </summary>
        /// <returns></returns>
        public static string UpdateCardInfo(string access_token, string card_id, out string error)
        {
            error = "";
            var url = string.Format("https://api.weixin.qq.com/card/update?access_token={0}", access_token);
            try
            {
                var jsonString = new
                {
                    card_id = card_id
                    ,
                    member_card = new
                    {
                        supply_balance = true,
                        base_info = new
                        {
                            pay_info = new
                            {
                                swipe_card = new
                                {
                                    is_swipe_card = true
                                }
                            }
                        }
                    }
                }.ToJson();
                var s = HttpKit.HttpPostSubmit(jsonString, url);
                var o = s.JsonToDictionary();

                if (o.ContainsKey("errcode") && o["errcode"].ToString() == "0" && o.ContainsKey("card_list"))
                {
                    var dic = (o["card_list"] as object[])[0] as Dictionary<string, object>;

                    if (dic["card_id"].ToSafeString() == card_id)
                    {
                        return dic["code"].ToSafeString();
                    }
                    else
                    {
                        return "";
                    }
                }

                if (o.ContainsKey("errcode") && o["errcode"].ToString() != "0")
                {
                    error += o["errcode"] + "\r\n" + o["errmsg"];
                }
                return "";
            }
            catch (Exception e)
            {
                error += e.Message;
                return null;
            }
        }




        /// <summary>
        /// 获取卡劵详细信息
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="card_id"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static CardMainModel GetCardInfo(string access_token, string card_id, out string error)
        {
            error = "";
            var url = string.Format("https://api.weixin.qq.com/card/get?access_token={0}", access_token);
            try
            {
                var s = HttpKit.HttpPostSubmit(new { card_id = card_id }.ToJson(), url);
                var o = s.JsonToDictionary();

                if (o.ContainsKey("errcode") && o["errcode"].ToString() == "0")
                {
                    var m = new CardMainModel();
                    o = s.JsonToDictionary() as Dictionary<string, object>;
                    if (o["card"] as Dictionary<string, object> != null)
                    {
                        var item = o["card"] as Dictionary<string, object>;
                        m.card_type = item["card_type"].ToSafeString();
                        m.LoadParam(item[m.card_type.ToLower()] as Dictionary<string, object>, out error);
                    }
                    m.ReturnString = s;
                    return m;
                }

                if (o.ContainsKey("errcode") && o["errcode"].ToString() != "0")
                {
                    error += o["errcode"] + "\r\n" + o["errmsg"];
                }
                return null;
            }
            catch (Exception e)
            {
                error += e.Message;
                return null;
            }
        }


        /// <summary>
        /// 通过CardId和Code获取Code信息
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="card_id"></param>
        /// <param name="code"></param>
        /// <param name="check_consume"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static string GetCardCodeInfo(string access_token, string card_id, string code, bool check_consume, out string error)
        {
            error = "";
            var url = string.Format("https://api.weixin.qq.com/card/code/get?access_token={0}", access_token);
            try
            {
                var s = HttpKit.HttpPostSubmit(new { code = code, card_id = card_id, check_consume = check_consume }.ToJson(), url);
                var o = s.JsonToDictionary();

                if (o.ContainsKey("errcode") && o["errcode"].ToString() == "0")
                {
                    return s;
                }

                if (o.ContainsKey("errcode") && o["errcode"].ToString() != "0")
                {
                    error += o["errcode"] + "\r\n" + o["errmsg"];
                }
                return "";
            }
            catch (Exception e)
            {
                error += e.Message;
                return null;
            }
        }


        /// <summary>
        /// 获取会员卡用户Code信息
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="card_id"></param>
        /// <param name="code"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static string GetMemberCardCodeInfo(string access_token, string card_id, string code, out string error)
        {
            error = "";
            var url = string.Format("https://api.weixin.qq.com/card/membercard/userinfo/get?access_token={0}", access_token);
            try
            {
                var s = HttpKit.HttpPostSubmit(new { code = code, card_id = card_id }.ToJson(), url);
                var o = s.JsonToDictionary();

                if (o.ContainsKey("errcode") && o["errcode"].ToString() == "0")
                {
                    return s;
                }

                if (o.ContainsKey("errcode") && o["errcode"].ToString() != "0")
                {
                    error += o["errcode"] + "\r\n" + o["errmsg"];
                }
                return "";
            }
            catch (Exception e)
            {
                error += e.Message;
                return null;
            }
        }




        /// <summary>
        /// 设置用户使用动态码
        /// </summary>
        /// <returns></returns>
        public static string UpdateChangeDynamicCodeCard(string access_token, string card_id, bool use_dynamic_code, out string error)
        {
            error = "";
            var url = string.Format("https://api.weixin.qq.com/card/update?access_token={0}", access_token);
            try
            {
                var jsonString = new
                {
                    card_id = card_id
                    ,
                    member_card = new
                    {
                        base_info = new
                        {
                            use_dynamic_code = use_dynamic_code
                        }
                    }
                }.ToJson();
                var s = HttpKit.HttpPostSubmit(jsonString, url);
                var o = s.JsonToDictionary();

                if (o.ContainsKey("errcode") && o["errcode"].ToString() == "0" && o.ContainsKey("card_list"))
                {
                    var dic = (o["card_list"] as object[])[0] as Dictionary<string, object>;

                    if (dic["card_id"].ToSafeString() == card_id)
                    {
                        return dic["code"].ToSafeString();
                    }
                    else
                    {
                        return "";
                    }
                }

                if (o.ContainsKey("errcode") && o["errcode"].ToString() != "0")
                {
                    error += o["errcode"] + "\r\n" + o["errmsg"];
                }
                return "";
            }
            catch (Exception e)
            {
                error += e.Message;
                return null;
            }
        }



        #endregion



        #region 微信支付块

        /// <summary>
        /// 统一下单方法
        /// </summary>
        /// <param name="m">统一下单对象</param>
        /// <param name="mch_key">商户key</param>
        /// <returns></returns>
        public static string UnifiedOrder(UnifiedOrderParamModel m, string mch_key, out string err)
        {
            err = "";

            var str = m.GetXmlString(mch_key, out err);

            var s = HttpKit.HttpPostSubmit(str, "https://api.mch.weixin.qq.com/pay/unifiedorder");

            return s;
        }


        public static string OrderQueryStatus(OrderQueryModel m, string mch_key, out string err)
        {
            err = "";

            var str = m.GetXmlString(mch_key, out err);

            var s = HttpKit.HttpPostSubmit(str, "https://api.mch.weixin.qq.com/pay/orderquery");

            return s;
        }



        public static string SendHongBao(HongBaoModel m, string mch_key, string crtpath, out string err)
        {
            err = "";

            var str = m.GetXmlString(mch_key, out err);

            var s = HttpWebRequestWithCertificate("https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack"
                , 60, str, true, "utf-8", crtpath, m.mch_id, out err);

            return s;
        }



        #endregion



        #region 群发消息块

        /// <summary>
        /// 群发文本消息测试接口
        /// </summary>
        /// <param name="access_token"></param>
        /// <param name="openid"></param>
        /// <param name="content"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static int SendAllMessageTextTest(string access_token, string openid, string content, out string error)
        {
            error = "";
            var url = string.Format("https://api.weixin.qq.com/cgi-bin/message/mass/preview?access_token={0}", access_token);
            try
            {
                var srcString = HttpKit.HttpPostSubmit(new { touser = openid, text = new { content = content }, msgtype = "text" }.ToJson(), url);

                var o = srcString.JsonToDictionary();

                if (o.ContainsKey("errcode") && o["errcode"].ToString() != "0")
                {
                    error += o["errcode"] + "\r\n" + o["errmsg"];
                    return 0;
                }

                return o["msg_id"].ToInt();
            }
            catch (Exception e)
            {
                error += e.Message;
                return 0;
            }
        }


        #endregion


        #region 公共方法

        /// <summary>
        /// 带证书请求
        /// </summary>
        /// <param name="requestUrl">请求的地址</param>
        /// <param name="timeout">超时时间</param>
        /// <param name="requestContent">请求的字符串</param>
        /// <param name="isPost">是否是POST</param>
        /// <param name="encoding">字符集编码</param>
        /// <param name="certificatePath">证书路径</param>
        /// <param name="certPassword">证书密码</param>
        /// <param name="msg">消息</param>
        /// <returns>请求结果</returns>
        public static string HttpWebRequestWithCertificate(string requestUrl, int timeout, string requestContent, bool isPost, string encoding, string certificatePath, string certPassword, out string msg)
        {
            msg = string.Empty;
            string result = string.Empty;
            try
            {
                string cert = string.Format(@"{0}", certificatePath);
                string password = certPassword;
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                X509Certificate2 cer = new X509Certificate2(cert, password, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);//此处遇到一坑：在阿里云提供的服务器上用X509Certificate总是失败，改为X509Certificate2后成功


                byte[] bytes = System.Text.Encoding.GetEncoding(encoding).GetBytes(requestContent);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
                request.ClientCertificates.Add(cer);
                request.ContentType = "application/x-www-form-urlencoded";
                request.Referer = requestUrl;
                request.Method = isPost ? "POST" : "GET";
                request.ContentLength = bytes.Length;
                request.Timeout = timeout * 1000;
                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                if (responseStream != null)
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.GetEncoding(encoding));
                    result = reader.ReadToEnd();
                    reader.Close();
                    responseStream.Close();
                    request.Abort();
                    response.Close();
                    return result.Trim();
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message + ex.StackTrace;
            }


            return result;
        }


        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }

        #endregion


        #region private


        private static W_User_Info GetUserInfo(string url, ref string error)
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Encoding = Encoding.UTF8;
                    var s = client.DownloadString(url);

                    var m = W_User_Info.Parse(s);

                    if (m != null)
                    {
                        m.ReturnString = s;
                        return m;
                    }

                    var o = s.JsonToDictionary();

                    if (o.ContainsKey("errcode"))
                    {
                        error += o["errcode"] + "\r\n" + o["errmsg"];
                    }
                    return null;
                }
            }
            catch (Exception e)
            {
                error += e.Message;
                return null;
            }
        }

        #endregion





    }
}
