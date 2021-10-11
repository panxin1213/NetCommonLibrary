using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChinaBM.Common;
using System.Reflection;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web.SessionState;

namespace System.Web.UI
{
    public abstract class PageBase : WebBase
    {
        public PageBase()
        {
            this.InitComplete += BaseAction;
            
        }

        protected abstract void Page_Init();

        protected abstract void OnGetRequest();

        protected abstract void OnPostRequest();

        protected void BaseAction(object a, EventArgs b)
        {
            if (HttpKit.IsPost)
            {
                OnPostRequest();
            }
            else
            {
                OnGetRequest();
            }
        }

        #region DataBind 数据绑定
        /// <summary>
        ///  数据绑定
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        protected TEntity DataBind<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity);
            string typeName = type.Name;
            PropertyInfo[] propertyInfos = typeof(TEntity).GetProperties();
            var entity = Activator.CreateInstance(type) as TEntity;
            var collection = Request.Form;
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (propertyInfo.PropertyType.Name.StartsWith("IList"))
                {
                    var itype = propertyInfo.PropertyType.GetGenericArguments().FirstOrDefault();
                    var ltype = propertyInfo.PropertyType.Assembly.GetType(propertyInfo.PropertyType.FullName.Replace("IList", "List"));
                    var iml = Activator.CreateInstance(ltype);
                    var imlm = ltype.GetMethod("Add");
                    if (!collection.AllKeys.Any(a => a.StartsWith(itype.Name + "[0].")))
                    {
                        continue;
                    }
                    var r = true;
                    var ii = 0;
                    while (r)
                    {
                        var im = Activator.CreateInstance(itype);
                        if (!collection.AllKeys.Any(a => a.StartsWith(itype.Name + "[" + ii + "].")))
                        {
                            break;
                        }

                        foreach (var iproperty in itype.GetProperties())
                        {
                            if (collection.AllKeys.Any(a => a.Equals(itype.Name + "[" + ii + "]." + iproperty.Name)))
                            {
                                var value = GetConvertValue(itype.Name + "[" + ii + "]." + iproperty.Name, iproperty.PropertyType);
                                if (value != null)
                                {
                                    if (iproperty.GetGetMethod() != null)
                                    {
                                        iproperty.SetValue(im, value, null);
                                    }
                                }
                            }
                        }
                        imlm.Invoke(iml, new object[] { im });
                        ii++;
                    }
                    propertyInfo.SetValue(entity, iml, null);

                }
                else
                {
                    var value = GetConvertValue(propertyInfo.Name, propertyInfo.PropertyType);
                    if (value != null)
                    {
                        if (propertyInfo.GetSetMethod() != null)
                        {
                            propertyInfo.SetValue(entity, value, null);
                        }
                    }
                }
            }
            return entity;
        }


        private object GetConvertValue(string collkey, Type type)
        {
            var collection = Request.Form;

            string[] values = collection.GetValues(collkey);
            if (values != null)
            {
                object value;
                if (values.Length == 2 && values[1] == "false" && !type.IsArray)
                {
                    value = Convert.ToBoolean(values[0]) || false;
                }
                else if (type == typeof(DateTime?))
                {
                    value = string.Join(",", values.Select(a => a != null ? a.Trim().Replace("\r", "").Replace("\n", "") : "")).ToDateTime();
                }
                else if (type == typeof(int?))
                {
                    value = values[0].ToInt();
                }
                else if (type == typeof(double?))
                {
                    value = values[0].ToDouble();
                }
                else if (type.IsArray)
                {
                    var eletype = type.GetElementType();
                    var array = Activator.CreateInstance(type, values.Length);
                    var set = type.GetMethod("Set");
                    var x = 0;
                    foreach (var item in values)
                    {
                        set.Invoke(array, new[] { x, eletype.ConvertType(item) });
                        x++;
                    }
                    value = array;
                }
                else
                {
                    try
                    {
                        value = Convert.ChangeType(string.Join(",", values.Select(a => a != null ? a : "")), type);
                    }
                    catch
                    {
                        if (type.IsGenericType)
                        {
                            var argtype = type.GetGenericArguments();
                            if (argtype.Length == 1)
                            {
                                var o = Activator.CreateInstance(type);
                                try
                                {
                                    var m = type.GetMethod("Add");

                                    foreach (var item in values)
                                    {
                                        m.Invoke(o, new object[] { Convert.ChangeType(item, argtype[0]) });
                                    }
                                    return o;
                                }
                                catch
                                {

                                }
                            }
                            return null;
                        }
                        else
                        {
                            value = type.IsValueType ? Activator.CreateInstance(type) : null;
                        }
                    }
                }
                return value;
            }

            return null;
        }
        #endregion

        #region IsValid 数据验证

        /// <summary>
        /// 数据验证
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        protected ModelState IsValid<T>(T t) where T : class
        {
            var type = typeof(T);
            var m = new ModelState();

            foreach (var property in type.GetProperties())
            {
                var r = ValidationCustom(property, t, type);
                m.Add(r);
            }

            return m;
        }

        #region Valid

        /// <summary>
        /// 调用验证
        /// </summary>
        /// <param name="p"></param>
        /// <param name="o"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private KeyValuePair<string, string> ValidationCustom(PropertyInfo p, object o, Type type)
        {
            var r = p.GetValidAttribute<ValidationAttribute>(type);

            if (r != null && r.Count > 0)
            {
                var value = p.GetValue(o, null);

                foreach (var a in r)
                {
                    
                    if (!a.IsValid(value))
                    {
                        return new KeyValuePair<string, string>(p.Name, String.IsNullOrEmpty(a.ErrorMessage) ? a.FormatErrorMessage(p.Name) : a.ErrorMessage);
                    }
                }
            }
            return default(KeyValuePair<string, string>);
        }

        ///// <summary>
        ///// 验证是否为空
        ///// </summary>
        ///// <param name="p"></param>
        ///// <param name="o"></param>
        ///// <returns></returns>
        //private KeyValuePair<string, string> ValidRequired(PropertyInfo p, object o, Type type)
        //{
        //    var r = p.GetValidAttribute<RequiredAttribute>(type);

        //    if (r != null)
        //    {
        //        var value = p.GetValue(o, null);

        //        if (value == null || String.IsNullOrEmpty(value.ToString()))
        //        {
        //            return new KeyValuePair<string, string>(p.Name, String.IsNullOrEmpty(r.ErrorMessage) ? r.FormatErrorMessage(p.Name) : r.ErrorMessage);
        //        }
        //    }
        //    return default(KeyValuePair<string, string>);
        //}

        ///// <summary>
        ///// 验证是否超过长度
        ///// </summary>
        ///// <param name="p"></param>
        ///// <param name="o"></param>
        ///// <returns></returns>
        //private KeyValuePair<string, string> ValidMaxLength(PropertyInfo p, object o, Type type)
        //{
        //    var r = p.GetValidAttribute<StringLengthAttribute>(type);

        //    if (r != null)
        //    {
        //        var value = p.GetValue(o, null);

        //        if (value != null && value.ToString().Length > r.MaximumLength)
        //        {
        //            return new KeyValuePair<string, string>(p.Name, String.IsNullOrEmpty(r.ErrorMessage) ? r.FormatErrorMessage(p.Name) : r.ErrorMessage);
        //        }
        //    }
        //    return default(KeyValuePair<string, string>);
        //}


        //private KeyValuePair<string, string> ValidRemote(PropertyInfo p, object o, Type type)
        //{
        //    var r = p.GetValidAttribute<RemoteAttribute>(type);

        //    if (r != null)
        //    {
        //        var error = new KeyValuePair<string, string>(p.Name, String.IsNullOrEmpty(r.ErrorMessage) ? r.FormatErrorMessage(p.Name) : r.ErrorMessage);

        //        var ss = r.AdditionalFields.Split(',');

        //        var webclient = new WebClient();
        //        var param = new NameValueCollection();

        //        foreach (var item in ss)
        //        {
        //            var pi = type.GetProperty(item);
        //            if (pi == null)
        //            {
        //                return error;
        //            }
        //            var oi = pi.GetValue(o, null);
        //            param.Add(pi.Name, oi == null ? "" : oi.ToString());

        //        }
        //        try
        //        {
        //            byte[] byRemoteInfo = webclient.UploadValues("http://" + Request.Url.Authority + r.Url, "POST", param);//请求地址,传参方式,参数集合  

        //            var str = System.Text.Encoding.Default.GetString(byRemoteInfo);//获取返回值  

        //            if (!str.Equals("true", StringComparison.OrdinalIgnoreCase))
        //            {
        //                return error;
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            return error;
        //        }
        //    }
        //    return default(KeyValuePair<string, string>);
        //}


        ///// <summary>
        ///// 验证是否超过长度
        ///// </summary>
        ///// <param name="p"></param>
        ///// <param name="o"></param>
        ///// <returns></returns>
        //private KeyValuePair<string, string> ValidRegularExpressionLength(PropertyInfo p, object o, Type type)
        //{
        //    var r = p.GetValidAttribute<RegularExpressionAttribute>(type);

        //    if (r != null)
        //    {
        //        var value = p.GetValue(o, null);
        //        var regex = new Regex(r.Pattern, RegexOptions.IgnoreCase);
        //        if (!regex.Match(value.ToString()).Success)
        //        {
        //            return new KeyValuePair<string, string>(p.Name, String.IsNullOrEmpty(r.ErrorMessage) ? r.FormatErrorMessage(p.Name) : r.ErrorMessage);
        //        }
        //    }
        //    return default(KeyValuePair<string, string>);
        //}

        #endregion

        #endregion

    }

    public abstract class PageBase<T> : PageBase where T : class
    {
        protected HtmlHelper<T> Html { get; set; }

        protected ModelState MState
        {
            get
            {
                return Html.MState;
            }
            set
            {
                Html.MState = value;
            }
        }

        /// <summary>
        /// 公共加载方法
        /// </summary>
        protected override abstract void Page_Init();

        public PageBase()
        {
            this.InitComplete -= BaseAction;
            this.InitComplete += new EventHandler((a, b) =>
            {
                if (HttpKit.IsPost)
                {
                    BindPost();
                }
                else
                {
                    BindGet();
                }
            });
            this.InitComplete += BaseAction;
        }

        /// <summary>
        /// Get模型绑定
        /// </summary>
        public virtual void BindGet()
        {
            Html = new HtmlHelper<T>(null);
        }

        /// <summary>
        /// Post数据绑定,验证绑定
        /// </summary>
        public virtual void BindPost()
        {
            var m = base.DataBind<T>();
            Html = new HtmlHelper<T>(m);
            MState = base.IsValid(m);
        }

        protected override void OnGetRequest()
        {
            GetAction();
        }

        /// <summary>
        /// get加载
        /// </summary>
        protected abstract void GetAction();

        protected override void OnPostRequest()
        {
            PostAction();
        }

        /// <summary>
        /// Post加载
        /// </summary>
        protected abstract void PostAction();


    }

    public class ModelState
    {
        public ModelState()
        {
            Errors = new Dictionary<string, string>();
        }

        public Dictionary<string, string> Errors { get; set; }

        public bool IsValid
        {
            get
            {
                return Errors.Count == 0;
            }
        }

        public void Add(KeyValuePair<string, string> v)
        {
            if (v.Key == null)
            {
                return;
            }

            if (!Errors.ContainsKey(v.Key))
            {
                Errors.Add(v.Key, v.Value);
            }
        }
    }
}
