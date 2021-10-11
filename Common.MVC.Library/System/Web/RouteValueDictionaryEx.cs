using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.Routing;

namespace System.Web
{
    /// <summary>
    /// 重写路由字典集合，修复匿名对象传值内部不区分大小写，但没做重复键判断 change by lbq 2014-07-21
    /// </summary>
    public partial class RouteValueDictionaryEx : RouteValueDictionary
    {
        public RouteValueDictionaryEx()
            : base()
        {

        }
        public RouteValueDictionaryEx(object values)
        {
            if (values != null)
            {
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(values))
                {
                    object obj2 = descriptor.GetValue(values);
                    if (this.ContainsKey(descriptor.Name))
                    {
                        this[descriptor.Name] = obj2;
                    }
                    else
                    {
                        this.Add(descriptor.Name, obj2);
                    }
                }
            }
        }

        public RouteValueDictionaryEx(IDictionary<string, object> dictionary)
            : base(dictionary)
        {
        }
    }
}
