using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinManage.Model
{
    /// <summary>
    /// 文本内容
    /// </summary>
    public class S_Text : SendBase
    {
        public S_Text()
        {
            base.MsgType = "text";
        }

        /// <summary>
        /// 回复的消息内容（换行：在content中能够换行，微信客户端就支持换行显示）
        /// </summary>
        public string Content { get; set; }
    }
}
