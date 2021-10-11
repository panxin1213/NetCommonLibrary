using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace Common.MVC.Library.Mvc.PrecompiledViews
{
    public static class PreApplicationStartCode
    {
        private static bool _startWasCalled;
        public static void Start()
        {
            if (PreApplicationStartCode._startWasCalled)
            {
                return;
            }
            PreApplicationStartCode._startWasCalled = true;
            HostingEnvironment.RegisterVirtualPathProvider(new CompiledVirtualPathProvider());
        }
    }
}
