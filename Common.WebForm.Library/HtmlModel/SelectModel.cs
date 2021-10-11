using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace System.Web
{
    /// <summary>
    /// select 模型
    /// </summary>
    public class SelectModel
    {
        public SelectModel(string name, string defaultText, IEnumerable<OptionModel> options, object attrs)
        {
            Name = name;
            DefaultText = defaultText;
            Options = options;
            Attrs = attrs;
        }

        /// <summary>
        /// select name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// select 默认内容
        /// </summary>
        public string DefaultText { get; set; }

        /// <summary>
        /// select options
        /// </summary>
        public IEnumerable<OptionModel> Options { get; set; }

        /// <summary>
        /// select 属性 支持匿名对象，和IDictionary<string,string>字典
        /// </summary>
        public object Attrs { get; set; }

        /// <summary>
        /// 返回select字符串
        /// </summary>
        /// <returns></returns>
        public string ShowSelect()
        {
            return HtmlTool.DropDownList(Name, Options, Attrs.ToDictionary(), DefaultText);
        }
    }
}
