using Core.Base;
using Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc
{
   public static class HtmlHeplerTitleEx
    {
       public static MvcHtmlString Title(this HtmlHelper html,string titleFormat,params object[] args)
       {
           
           return MvcHtmlString.Create("<title>" + html.Encode(String.Format(titleFormat, args)) + " - " + BaseConfig.Current.SiteName + "</title>");
       }
    }
}
