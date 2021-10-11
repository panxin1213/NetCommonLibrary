using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinManage.Model
{
    /// <summary>
    /// oauth验证获取模型
    /// </summary>
    public class W_OAuthModel
    {
        /// <summary>
        /// openid
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// code 获取的access_token
        /// </summary>
        public string Auth_Access_Token { get; set; }

        /// <summary>
        /// 验证数据符合规则
        /// </summary>
        /// <returns></returns>
        public bool Valid()
        {
            return !String.IsNullOrEmpty(OpenId) && !String.IsNullOrEmpty(Auth_Access_Token);
        }
    }
}
