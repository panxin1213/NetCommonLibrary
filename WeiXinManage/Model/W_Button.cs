using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinManage.Model
{
    public class W_Button
    {
        /// <summary>
        /// 按钮名称，四个中文或8个字母
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 点击后的key
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 跳转地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 下级菜单，只二级有效
        /// </summary>
        public List<W_Button> Sub_Button { get; set; }

    }
}
