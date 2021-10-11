using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Login.Plugins.QQ.Model
{
    public class ErrorModel
    {
        public ErrorModel(string s)
        {
            var d = s.ToSafeString().Replace("callback(", "").Replace(");\n", "").JsonToDictionary();

            if (d.Count > 0)
            {
                ErrorCode = d.TryGetValue("error", "").ToSafeString();
                ErrorMessage = d.TryGetValue("error_description", "").ToSafeString();
            }
        }
        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}
