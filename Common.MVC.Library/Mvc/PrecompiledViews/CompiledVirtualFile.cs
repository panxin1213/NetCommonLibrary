using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Hosting;

namespace Common.MVC.Library.Mvc.PrecompiledViews
{
    public class CompiledVirtualFile : VirtualFile
    {
        public Type CompiledType
        {
            get;
            set;
        }
        public CompiledVirtualFile(string virtualPath, Type compiledType)
            : base(virtualPath)
        {
            this.CompiledType = compiledType;
        }
        public override Stream Open()
        {
            return new MemoryStream(Encoding.ASCII.GetBytes("@inherits " + this.CompiledType.AssemblyQualifiedName + "\n@{base.Execute();}"));
        }
    }
}
