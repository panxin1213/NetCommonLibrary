using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Web.Script.Serialization;
using System.Reflection;

namespace System
{
    /// <summary>
    /// object 扩展
    /// </summary>
    public static class ObjectEx
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToInt(this object obj, int defaultvalue = 0)
        {
            var v = obj;
            if (v != null)
            {
                try
                {
                    return ((IConvertible)v).ToInt32(null);
                }
                catch { }
            }
            return defaultvalue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="t1"></param>
        /// <param name="t2"></param>
        /// <param name="removePropertys"></param>
        /// <returns></returns>
        public static bool EqualsObject(this object t1, object t2, string[] removePropertys = null)
        {
            if (t1 == null || t2 == null)
            {
                return false;
            }
            var type = t1.GetType();
            if (type != t2.GetType())
            {
                return false;
            }

            foreach (var item in type.GetProperties())
            {
                if (removePropertys.Any(a => a == item.Name))
                {
                    continue;
                }
                var i1 = item.GetValue(t1, null);
                var i2 = item.GetValue(t2, null);
                if ((i1 == null || i2 == null) && i1 != i2)
                {
                    return false;
                }
                if (i1 == null && i2 == null)
                {
                    continue;
                }
                if (!i1.Equals(i2))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="defaultvalue"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object obj, DateTime defaultvalue)
        {
            DateTime newValue;

            try
            {
                if (obj != null && obj is DateTime)
                {
                    newValue = (DateTime)obj;
                }
                else
                {
                    obj = System.Convert.ChangeType(obj, typeof(DateTime));
                    newValue = (DateTime)obj;
                }
            }
            catch (Exception)
            {
                newValue = defaultvalue;
            }
            return newValue;
        }


        /// <summary>
        /// 取字段说明文字
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <returns></returns>
        public static string GetDescription<T>(Expression<Func<T, object>> property)
        {
            var t = typeof(T);
            var proname = ExpressionEx.ParsePropertySelector(property, "GetDescription", "property");
            var dis = typeof(T).GetProperty(proname).GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;
            return dis == null ? proname : dis.Description;
        }


        /// <summary> 
        /// 返回对象序列化 
        /// </summary> 
        /// <param name="obj">源对象</param> 
        /// <returns>json数据</returns> 
        public static string ToJson(this object obj)
        {
            JavaScriptSerializer serialize = new JavaScriptSerializer();
            serialize.MaxJsonLength = Int32.MaxValue;
            return serialize.Serialize(obj);
        }

        /// <summary> 
        /// 控制深度 
        /// </summary> 
        /// <param name="obj">源对象</param> 
        /// <param name="recursionDepth">深度</param> 
        /// <returns>json数据</returns> 
        public static string ToJson(this object obj, int recursionDepth)
        {
            JavaScriptSerializer serialize = new JavaScriptSerializer();
            serialize.RecursionLimit = recursionDepth;
            return serialize.Serialize(obj);
        }

        /// <summary>
        /// 匿名对象转dictionary
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ToDictionary(this object obj)
        {
            if (obj == null)
            {
                return new Dictionary<string, object>();
            }

            var type = obj.GetType();

            var dic = new Dictionary<string, object>();

            foreach (var property in type.GetProperties())
            {
                dic.Add(property.Name, property.GetValue(obj, null));
            }

            return dic;
        }

        public static string ToSafeString(this object obj)
        {
            if (obj == null)
            {
                return "";
            }

            return obj.ToString();
        }

        public static string ToSafeString(this DateTime? obj, string format)
        {
            if (obj == null)
            {
                return "";
            }

            return obj.Value.ToString(format);
        }


        /// <summary>
        /// 返回新对象
        /// </summary>
        /// <typeparam name="T">返回对象</typeparam>
        /// <param name="obj">传入对象</param>
        /// <returns></returns>
        public static T CopyNewObject<T>(this object obj) where T : class,new()
        {
            var t = new T();
            try
            {
                foreach (var item in typeof(T).GetProperties())
                {
                    var v = item.GetValue(obj, null);
                    if (v != null)
                    {
                        item.SetValue(t, v, null);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return t;
        }


        /// <summary>
        /// 获取对象内容(属性名称(优先显示备注):属性值)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetModelContent(this object obj)
        {
            if (obj == null)
            {
                return "";
            }

            var sb = new StringBuilder();
            var type = obj.GetType();
            foreach (var item in obj.GetType().GetProperties())
            {
                var name = item.GetDescription(type);
                var value = item.GetValue(obj, null).ToSafeString();
                if (!String.IsNullOrWhiteSpace(value))
                    sb.AppendLine(name + ":" + value);
            }

            return sb.ToSafeString();
        }
    }
}
