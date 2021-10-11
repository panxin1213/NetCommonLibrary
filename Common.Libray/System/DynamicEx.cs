using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Dynamic;
using System.Collections;

namespace System.Dynamic
{
    /// <summary>
    /// 动态对象扩展
    /// </summary>
    public static class DynamicPageObjectExt
    {
        /// <summary>
        /// 把object 转换成 dynamic
        /// </summary>
        /// <param name="anonymousObject"></param>
        /// <returns></returns>
        public static dynamic ToExpando(this object anonymousObject)
        {
            
            return new DynamicPageObject(anonymousObject);
        }

        /// <summary>
        /// Extension method that turns a dictionary of string and object to an ExpandoObject
        /// </summary>
        public static ExpandoObject ToExpando(this IDictionary<string, object> dictionary)
        {
            var expando = new ExpandoObject();
            var expandoDic = (IDictionary<string, object>)expando;

            // go through the items in the dictionary and copy over the key value pairs)
            foreach (var kvp in dictionary)
            {
                // if the value can also be turned into an ExpandoObject, then do it!
                if (kvp.Value is IDictionary<string, object>)
                {
                    var expandoValue = ((IDictionary<string, object>)kvp.Value).ToExpando();
                    expandoDic.Add(kvp.Key, expandoValue);
                }
                else if (kvp.Value is ICollection)
                {
                    // iterate through the collection and convert any strin-object dictionaries
                    // along the way into expando objects
                    var itemList = new List<object>();
                    foreach (var item in (ICollection)kvp.Value)
                    {
                        if (item is IDictionary<string, object>)
                        {
                            var expandoItem = ((IDictionary<string, object>)item).ToExpando();
                            itemList.Add(expandoItem);
                        }
                        else
                        {
                            itemList.Add(item);
                        }
                    }

                    expandoDic.Add(kvp.Key, itemList);
                }
                else
                {
                    expandoDic.Add(kvp);
                }
            }

            return expando;
        }
        /*
        public static object ToType<T>(this object obj)
        {

            //create instance of T type object:

            var tmp = Activator.CreateInstance(typeof(T));

            //loop through the properties of the object you want to covert:          
            foreach (PropertyInfo pi in obj.GetType().GetProperties())
            {
                try
                {

                    //get the value of property and try 
                    //to assign it to the property of T type object:
                    tmp.GetType().GetProperty(pi.Name).SetValue(tmp,
                                              pi.GetValue(obj, null), null);
                }
                catch { }
            }

            //return the T type object:         
            return tmp;
        }
        public static IList<T> ToNonAnonymousList<T>(this List<object> list)
        {

            //define system Type representing List of objects of T type:
            var genericType = typeof(List<>).MakeGenericType(typeof(T));

            //create an object instance of defined type:
            var l = (IList<T>)Activator.CreateInstance(genericType);

            //get method Add from from the list:
            MethodInfo addMethod = l.GetType().GetMethod("Add");

            //loop through the calling list:
            foreach (T item in list)
            {

                //convert each object of the list into T object 
                //by calling extension ToType<T>()
                //Add this object to newly created list:
                addMethod.Invoke(l, new object[] { item.ToType<T>() });
            }

            //return List of T objects:
            return l;
        }
        */
        /*
        //唉.效率不行
        public static T ToExpando<T>(this object anonymousObject)
        {
            var t = typeof(T);
            var r = (T)t.Assembly.CreateInstance(t.FullName);

            if (r == null)
                return default(T);
            var ot = anonymousObject.GetType();

            var bf = BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public;

            foreach (PropertyInfo p in t.GetProperties(bf))
            {
                try
                {
                    object value = ot
                                 .InvokeMember(p.Name,
                                               bf,
                                               null,
                                               anonymousObject,
                                               null);
                    p.SetValue(r, value, bf, null, null, null);
                }
                catch { }
            }
            return r;
        }*/
    }
    /// <summary>
    /// 动态对象，继承DynamicObject，重写TryGetMember方法。让未知字段只提示，不报错
    /// </summary>
    public class DynamicPageObject : DynamicObject
    {
        private object _originalObject;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="originalObject"></param>
        public DynamicPageObject(object originalObject)
        {
            _originalObject = originalObject;
        }
        /// <summary>
        /// 转换之前的对象
        /// </summary>
        public object OriginalObject { get { return _originalObject; } }
        /// <summary>
        /// 重写TryGetMember方法。让未知字段只提示，不报错
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            try
            {
                result = _originalObject
                            .GetType()
                            .InvokeMember(binder.Name,
                                          BindingFlags.GetProperty
                                              | BindingFlags.Instance
                                              | BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.GetField,
                                          null,
                                          _originalObject,
                                          null);

            }
            catch
            {
                result = String.Format("属性“{0}”调用错误", binder.Name);
                //return false;
                //throw;
            }
            return true;
        }
    }
}
