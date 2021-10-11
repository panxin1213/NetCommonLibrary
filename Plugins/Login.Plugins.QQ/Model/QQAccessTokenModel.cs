using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Login.Plugins.QQ.Model
{
    public class QQAccessTokenModel
    {
        public QQAccessTokenModel(string accesstokenstring)
        {
            var d = accesstokenstring.Split('&').Select(a => a.Split('=')).Where(a => a.Count() == 2).ToDictionary(a => a[0], a => a[1]);

            AccessToken = d.TryGetValue("access_token", "").ToSafeString();
            ExpiresIn = d.TryGetValue("expires_in", "").ToInt();
            RefreshToken = d.TryGetValue("refresh_token", "").ToSafeString();
            NowTime = DateTime.Now;
        }

        /// <summary>
        /// 当前accesstoken
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 有效秒数
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// 当前时间
        /// </summary>
        public DateTime NowTime { get; set; }

        /// <summary>
        /// 刷新token
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime EndTime
        {
            get
            {
                return NowTime.AddSeconds(ExpiresIn);
            }
        }
    }
}
