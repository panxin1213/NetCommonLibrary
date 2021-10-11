using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Xml.Linq
{
    public static class XElementEx
    {
        public static string GetSafeValue(this XElement ele)
        {
            if (ele == null)
            {
                return "";
            }
            return ele.Value.ToSafeString();
        }
    }
}
