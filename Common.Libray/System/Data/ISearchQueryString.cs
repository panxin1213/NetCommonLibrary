using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace System.Data
{
    /// <summary>
    /// 根据自身字段获取搜索条件的接口
    /// </summary>
    public interface ISearchQueryString
    {
        /// <summary>
        /// 
        /// </summary>
        IDictionary<string, object> QueryString {get;}

    }
    /// <summary>
    /// 默认实现
    /// </summary>
    public abstract class SearchQueryString
    {
        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, object> QueryString {get;private set;}
        
        /// <summary>
        /// 按继承的模型字段,取得所有搜索条件参数 
        /// </summary>
        /// <param name="ex">排除的参数，逗号分开</param>
        /// <returns></returns>
        private IDictionary<string, object> GetSearchQueryString(string ex = "")
        {
            var x = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var r = new Dictionary<string, object>();
            ex = ("," + ex + ",").ToLower();
            var nullableBoolen = typeof(bool?);
            foreach (PropertyInfo y in x)
            {
                if (ex.IndexOf(("," + y.Name + ",").ToLower()) == -1)
                {
                    var v = y.GetValue(this, null);
                    if (v != null && y.PropertyType.FullName.Contains("System.DateTime"))
                    {
                        var xx = (DateTime)v;
                        if (xx.TimeOfDay == DateTime.Now.Date.TimeOfDay)
                        {
                            r.Add(y.Name, xx.Date.ToString("yyyy-MM-dd"));
                        }
                    }
                    else
                    {

                        //if (v != null || nullableBoolen.IsAssignableFrom(y.PropertyType))
                        r.Add(y.Name, v);
                    }
                }
            }
            return r;
        }
        
    }
}
