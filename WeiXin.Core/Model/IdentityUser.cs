using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WeiXinManage.Model;

namespace WeiXin.Core.Model
{
    [Serializable]
    public class IdentityUser
    {
        /// <summary>
        /// openid
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 唯一id
        /// </summary>
        public string UnionID { get; set; }

        /// <summary>
        /// authaccesstoken,验证信息
        /// </summary>
        public string Auth_Access_Token { get; set; }

        /// <summary>
        /// token过期时间
        /// </summary>
        public DateTime Token_EndTime { get; set; }

        /// <summary>
        /// 微信用户信息
        /// </summary>
        public W_User_Info User_Info { get; set; }

    }
}
