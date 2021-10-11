using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc
{
    /// <summary>
    /// 
    /// </summary>
    public static class ModelStateDictionaryEx
    {
        /// <summary>
        /// 取Modelstate里的值
        /// </summary>
        /// <param name="modelstat"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static string TryGetValue(this ModelStateDictionary modelstat, string key, object def)
        {
            ModelState m = new  ModelState();
            if (modelstat.TryGetValue(key, out m)) 
            {
                if (m.Value!=null&& !String.IsNullOrEmpty(m.Value.AttemptedValue))
                    return m.Value.AttemptedValue;
            }
            return def!=null?def.ToString():null;
        }


        /// <summary>
        /// 返回验证错误集合字符串
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static string GetErrors(this ModelStateDictionary m)
        {
            return string.Join(@"\r\n",
            m.Values.Where(a => a.Errors.Any())
                .Select(a => a.Errors)
                .Select(b => string.Join(",", b.Select(c => c.ErrorMessage))));
        }
        
    }
}
