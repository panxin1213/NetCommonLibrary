using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web;
using Core.Web;

namespace System.Web.Mvc
{
    public class BaseWebViewPage : BMWebViewPage
    {
        public ServiceMain ServiceMain { get; set; }

        public BaseWebViewPage()
        {
            ServiceMain = System.Web.HttpContext.Current.GetServiceMain();
        }

        public override void Execute()
        {

        }
    }

    public class BaseWebViewPage<T> : BMWebViewPage<T>
    {
        public ServiceMain ServiceMain { get; set; }

        public BaseWebViewPage()
        {
            ServiceMain = System.Web.HttpContext.Current.GetServiceMain();
        }

        public override void Execute()
        {

        }
    }
}
