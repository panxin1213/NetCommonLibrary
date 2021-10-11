using Login.Plugins.QQ.Interface;
using Login.Plugins.QQ.Model;
using Login.Plugins.QQ.Section;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Login.Plugins.QQ
{
    public static class QQLoginHelper
    {
        private static QQConfigSection config = QQConfigSection.Current;

        public static IQQConfigOperation configoperation = CreateQQConfigOperation();

        /// <summary>
        /// 从配置节中读取密码转换类型并实例化
        /// </summary>
        /// <returns></returns>
        private static IQQConfigOperation CreateQQConfigOperation()
        {
            var r = Assembly.Load(config.Operation.Assembly).CreateInstance(config.Operation.Type) as IQQConfigOperation;
            if (r == null)
                throw new Exception("请在Web.config中指定“QQConfigOperation”(提供当前站点qq互联规则操作类) ");
            return r;
        }


        /// <summary>
        /// 获取授权登录URL地址
        /// </summary>
        /// <param name="ispc">是否是pc端</param>
        /// <returns></returns>
        public static string GetAuthorizeUrl()
        {
            var url = "";
            if (!config.IsMobile)
            {
                url = "https://graph.qq.com/oauth2.0/authorize";
            }
            else
            {
                url = "https://graph.z.qq.com/moc2/authorize";
            }
            url += string.Format("?response_type=code&client_id={0}&redirect_uri={1}&state={2}"
                , config.AppKey.Value
                , HttpUtility.UrlEncode(config.AuthUrl.Value)
                , config.Server.Value);

            return url;
        }

        /// <summary>
        /// 获取accessToken
        /// </summary>
        /// <param name="code">授权返回的code</param>
        /// <returns></returns>
        public static QQAccessTokenModel GetAccessToken(string code)
        {
            var accesstoken = configoperation.GetAccessToken();
            if (accesstoken != null && accesstoken.EndTime > DateTime.Now)
            {
                return accesstoken;
            }

            var url = "";

            if (!config.IsMobile)
            {
                url = "https://graph.qq.com/oauth2.0/token";
            }
            else
            {
                url = "https://graph.z.qq.com/moc2/token";
            }

            if (accesstoken != null)
            {
                url += string.Format("?grant_type=refresh_token&refresh_token={0}", accesstoken.RefreshToken);
            }
            else
            {
                url += string.Format("?grant_type=authorization_code&code={0}&redirect_uri={1}"
                    , code
                    , HttpUtility.UrlEncode(config.AuthUrl.Value)
                    );
            }

            url += string.Format("&client_id={0}&client_secret={1}", config.AppKey.Value, config.AppSecret.Value);


            using (WebClient client = new WebClient())
            {
                var s = client.DownloadString(url);
                var error = new ErrorModel(s);

                if (!String.IsNullOrWhiteSpace(error.ErrorCode))
                {
                    throw new Exception(error.ErrorCode + ":" + error.ErrorMessage);
                }

                var m = new QQAccessTokenModel(s);

                if (!String.IsNullOrEmpty(m.AccessToken))
                {
                    return m;
                }
                return null;
            }
        }


        /// <summary>
        /// 通过accessToken获取用户openid
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static string GetOpenId(string accessToken)
        {
            var url = "";

            if (!config.IsMobile)
            {
                url = "https://graph.qq.com/oauth2.0/me";
            }
            else
            {
                url = "https://graph.z.qq.com/moc2/me";
            }

            url += string.Format("?access_token={0}", accessToken);

            using (WebClient client = new WebClient())
            {
                var s = client.DownloadString(url);
                var error = new ErrorModel(s);

                if (!String.IsNullOrWhiteSpace(error.ErrorCode))
                {
                    throw new Exception(error.ErrorCode + ":" + error.ErrorMessage);
                }

                var m = new OpenIdModel(s);

                if (String.IsNullOrEmpty(m.OpenId) || m.ClientId != config.AppKey.Value)
                {
                    return "";
                }
                return m.OpenId;
            }
        }
    }
}
