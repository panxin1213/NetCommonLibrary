using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Collections.Specialized;

namespace System.Web
{
    public class RemoteAttribute : ValidationAttribute
    {
        public string AdditionalFields { get; set; }
        public string HttpMethod { get; set; }
        /// <summary>
        /// 完整的命名空间类名称(需实现IRemoteValid接口)
        /// </summary>
        public string FullClassName { get; set; }

        public string Url { get; set; }

        public override bool IsValid(object value)
        {
            var keys = AdditionalFields.Split(',');

            var param = new NameValueCollection();

            foreach (var item in keys)
            {
                if (!String.IsNullOrEmpty(HttpContext.Current.Request.QueryString[item]))
                {
                    param.Add(item, HttpContext.Current.Request.QueryString[item]);
                }
                else if (!String.IsNullOrEmpty(HttpContext.Current.Request.Form[item]))
                {
                    param.Add(item, HttpContext.Current.Request.Form[item]);
                }
            }
            var o1 = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetName().Name == System.Configuration.ConfigurationManager.AppSettings["AdminMainAssemblName"] || a.GetName().Name.StartsWith(FullClassName)).OrderBy(a => a.FullName.Length).FirstOrDefault();

            var o = o1.CreateInstance(FullClassName) as IRemoteValid;

            if (o != null)
            {
                o.Param = param;
                if (o.Valid(System.Web.HttpContext.Current))
                {
                    return true;
                }
            }
            return false;
        }
    }

    public interface IRemoteValid
    {
        NameValueCollection Param { get; set; }
        bool Valid(HttpContext context);
    }
}
