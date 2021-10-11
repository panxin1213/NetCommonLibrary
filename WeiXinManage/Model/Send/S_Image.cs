using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinManage.Model
{
    /// <summary>
    /// 图片信息
    /// </summary>
    public class S_Image : Media
    {
        public S_Image()
        {
            base.MsgType = "image";
        }
    }
}
