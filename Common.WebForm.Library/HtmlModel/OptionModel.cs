using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web
{
    /// <summary>
    /// option模型
    /// </summary>
    public class OptionModel
    {
        public OptionModel(string value, string text)
        {
            Value = value;
            Text = text;
            Disabled = false;
            Selected = false;
        }

        /// <summary>
        /// option value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// option text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 是否可选
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// 是否已选
        /// </summary>
        public bool Selected { get; set; }
    }
}
