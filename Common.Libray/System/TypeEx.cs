using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaBM.Common;

namespace System
{
    public static class TypeEx
    {
        public static object ConvertType(this Type t, object o)
        {
            if (t == typeof(DateTime?))
            {
                return o == null ? null : o.ToString().ToDateTime();
            }
            else if (t == typeof(int?))
            {
                if (o == null)
                {
                    return null;
                }
                return o.ToInt();
            }
            else if (t == typeof(bool?))
            {
                if (o == null)
                {
                    return null;
                }

                return ConvertKit.Convert(o, false);
            }

            try
            {
                return Convert.ChangeType(o, t);
            }
            catch
            {
                return t.IsValueType ? Activator.CreateInstance(t) : null;
            }
        }
    }
}
