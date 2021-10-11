using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Web.Hosting;

namespace Common.MVC.Library.Mvc.PrecompiledViews
{
    public class CompiledVirtualPathProvider : VirtualPathProvider
    {
        public override bool FileExists(string virtualPath)
        {
            return this.GetCompiledType(virtualPath) != null || base.Previous.FileExists(virtualPath);
        }
        private Type GetCompiledType(string virtualPath)
        {
            return ApplicationPartRegistry.Instance.GetCompiledType(virtualPath);
        }
        public override VirtualFile GetFile(string virtualPath)
        {
            if (base.Previous.FileExists(virtualPath))
            {
                return base.Previous.GetFile(virtualPath);
            }
            Type compiledType = this.GetCompiledType(virtualPath);
            if (compiledType != null)
            {
                return new CompiledVirtualFile(virtualPath, compiledType);
            }
            return null;
        }
        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
        {
            if (virtualPathDependencies == null)
            {
                return base.Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
            }
            return base.Previous.GetCacheDependency(virtualPath,
                from string vp in virtualPathDependencies
                where this.GetCompiledType(vp) == null
                select vp, utcStart);
        }
    }
}
