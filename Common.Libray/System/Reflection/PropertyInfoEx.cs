using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace System.Reflection
{
    public static class PropertyInfoEx
    {
        public static string GetDescription(this PropertyInfo p, Type type = null)
        {
            var attr = p.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
            if (attr == null && type == null)
            {
                return p.Name;
            }
            if (attr == null && type != null)
            {
                var metadateattr = type.GetCustomAttributes(typeof(MetadataTypeAttribute), false).FirstOrDefault() as MetadataTypeAttribute;

                if (metadateattr == null)
                {
                    return p.Name;
                }
                var property = metadateattr.MetadataClassType.GetProperty(p.Name);
                if (property == null)
                {
                    return p.Name;
                }

                var display = property.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() as DisplayAttribute;

                if (display != null)
                {
                    return display.Name;
                }
            }

            if (attr == null)
            {
                return p.Name;
            }

            return attr.Description;
        }

        public static List<T> GetValidAttribute<T>(this PropertyInfo p, Type type = null) where T : Attribute
        {
            List<T> r = null;
            var metadateattr = type.GetCustomAttributes(typeof(MetadataTypeAttribute), false).FirstOrDefault() as MetadataTypeAttribute;

            if (metadateattr != null)
            {
                var property = metadateattr.MetadataClassType.GetProperty(p.Name);
                if (property != null)
                {
                    r = property.GetCustomAttributes(typeof(T), false).Select(a => a as T).ToList();
                }
            }

            if (r == null)
            {
                r = new List<T>();
            }
            
            var  modelr = p.GetCustomAttributes(typeof(T), false).Select(a => a as T).ToList();

            foreach (var item in modelr)
            {
                if (!r.Any(a => a.GetType() == item.GetType()))
                {
                    r.Add(item);
                }
            }

            return r;
        }
    }
}
