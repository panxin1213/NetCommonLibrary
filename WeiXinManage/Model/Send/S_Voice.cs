using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinManage.Model
{
    /// <summary>
    /// 语音信息
    /// </summary>
    public class S_Voice : Media
    {
        public S_Voice()
        {
            base.MsgType = "voice";
        }
    }
}
