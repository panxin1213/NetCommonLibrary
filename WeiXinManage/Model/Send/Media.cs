using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinManage.Model
{
    /// <summary>
    /// 图片，语言
    /// </summary>
    public class Media : SendBase
    {
        /// <summary>
        /// 通过上传多媒体文件，得到的id。,必须
        /// </summary>
        public string MediaId { get; set; }
    }
}
