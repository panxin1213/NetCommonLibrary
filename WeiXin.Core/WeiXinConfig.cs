using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using WeiXinManage;
using System.Data;
using ChinaBM.Common;

namespace WeiXin.Core
{
    public class WeiXinConfig
    {
        public WeiXinConfig()
        {
            try
            {
                _weiXinToken = ConfigurationManager.AppSettings["WXToken"] ?? "";
                _weiXinAppId = ConfigurationManager.AppSettings["WXAppId"] ?? "";
                _weiXinSecret = ConfigurationManager.AppSettings["WXSecret"] ?? "";
                _weiXinGuanZhu = ConfigurationManager.AppSettings["WXGuanZhu"] ?? "";
                _isauth = ConvertKit.Convert(ConfigurationManager.AppSettings["IsAuth"], false);
                _weiXinAuthCode = ConfigurationManager.AppSettings["WeiXinAuthCode"] ?? "chinamost003";

                if (String.IsNullOrEmpty(_weiXinToken) || String.IsNullOrEmpty(_weiXinAppId) || String.IsNullOrEmpty(_weiXinSecret) || String.IsNullOrEmpty(_weiXinGuanZhu))
                {
                    throw new Exception("WXToken或WXAppId或WXSecret或WXGuanZhu为空");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + "\n微信配置存在问题");
            }
        }

        private string _weiXinToken;
        /// <summary>
        /// token
        /// </summary>
        public string WeiXinToken
        {
            get
            {
                return _weiXinToken;
            }
        }

        private string _weiXinGuanZhu;
        /// <summary>
        /// 关注页地址
        /// </summary>
        public string WeiXinGuanZhu
        {
            get
            {
                return _weiXinGuanZhu;
            }
        }


        private string _weiXinAppId;
        /// <summary>
        /// 微信 公众号 appid
        /// </summary>
        public string WeiXinAppId
        {
            get
            {
                return _weiXinAppId;
            }
        }


        private string _weiXinSecret;
        /// <summary>
        /// 微信 公众号 secret
        /// </summary>
        public string WeiXinSecret
        {
            get
            {
                return _weiXinSecret;
            }
        }

        public string TimeStamp
        {
            get
            {
                return DateTime.Now.DateTicks().ToSafeString();
            }
        }

        
        /// <summary>
        /// access_token
        /// </summary>
        private static AccessTokenModel _accessTokens = null;

        /// <summary>
        /// accessToken
        /// </summary>
        public string AccessToken
        {
            get
            {
                if (_accessTokens != null)
                {
                    if (_accessTokens.EndTime > DateTime.Now)
                    {
                        return _accessTokens.AccessToken;
                    }
                    else
                    {
                        _accessTokens = null;
                    }
                }

                var error = "";
                var token = WeiXinHelper.GetAccessToken(WeiXinAppId, WeiXinSecret, out error);

                if (!String.IsNullOrEmpty(error))
                {
                    throw new Exception(error);
                }

                _accessTokens = new AccessTokenModel { AccessToken = token, EndTime = DateTime.Now.AddMinutes(80) };

                return token;
            }
        }
        
        private bool _isauth;

        /// <summary>
        /// 是否需要网页授权
        /// </summary>
        public bool IsAuth
        {
            get
            {
                return _isauth;
            }
        }


        private string _weiXinAuthCode;

        /// <summary>
        /// 微信验证code
        /// </summary>
        public string WeiXinAuthCode
        {
            get
            {
                return _weiXinAuthCode;
            }
        }


    }

    public class AccessTokenModel
    {
        public string AccessToken { get; set; }

        public DateTime EndTime { get; set; }
    }
}
