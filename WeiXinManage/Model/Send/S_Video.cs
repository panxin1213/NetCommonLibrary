using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinManage.Model
{
    /// <summary>
    /// 视频模型
    /// </summary>
    public class S_Video : Media
    {
        public S_Video()
        {
            base.MsgType = "video";
        }

        /// <summary>
        /// 视频消息的标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 视频消息的描述
        /// </summary>
        public string Description { get; set; }
    }
}
