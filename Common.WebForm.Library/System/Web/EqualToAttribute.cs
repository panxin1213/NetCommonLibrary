using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace System.Web
{
    public class EqualToAttribute : ValidationAttribute
    {
        public string EqualToField { get; set; }

        public override bool IsValid(object value)
        {
            var s = "";
            if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString[EqualToField]))
            {
                s = HttpContext.Current.Request.QueryString[EqualToField];
            }
            else if (!String.IsNullOrEmpty(HttpContext.Current.Request.Form[EqualToField]))
            {
                s = HttpContext.Current.Request.Form[EqualToField];
            }

            if (value != null)
            {
                if (s.Equals(value.ToString()))
                {
                    return true;
                }
            }
            else
            {
                if (s == null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
