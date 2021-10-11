using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web.Routing;

namespace System.Collections.Generic
{
    /// <summary>
    /// 字典扩展
    /// </summary>
    public static class DictionaryEx
    {
        /// <summary>
        /// 安全的添加键值，如果重复则更新原值
        /// </summary>
        /// <typeparam name="Tkey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SafeAdd<Tkey, TValue>(this IDictionary<Tkey, TValue> dic, Tkey key, TValue value)
        {
            if (dic.ContainsKey(key))
            {
                dic[key] = value;
            }
            else
            {
                dic.Add(key, value);
            }
        }
        /// <summary>
        /// 从字典中取值，如果没有。用defaultT代替
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic">字典</param>
        /// <param name="key">标识</param>
        /// <param name="defaultT">默认值，如果没有</param>
        /// <returns>与字典中Value的类型 T 一致</returns>
        public static TValue TryGetValue<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey key, TValue defaultT)
        {
            TValue re = defaultT;
            if (dic != null)
            {
                if (typeof(String).IsAssignableFrom(typeof(TKey))) //字符串比较
                {
                    re = dic.Where(a => String.Equals(a.Key.ToString(), key as string, StringComparison.OrdinalIgnoreCase)).Select(a => a.Value).FirstOrDefault();
                }
                else
                {
                    if (dic != null) dic.TryGetValue(key, out re);
                }
            }
            if (re == null)
                return defaultT;
            else
                return re;
            /*
            if (dic != null && dic.ContainsKey(key))
            {
                var r = dic[key];
                if (null ==r )
                    r = defaultT;
                return r;
            }
            else
            {
                return defaultT;
            }*/
        }
        /// <summary>
        /// 当前字典是否包含参数中的所有key和value
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <param name="Exceptkey">排除对比的字段</param>
        /// <returns></returns>
        public static bool ContainsAll<TKey, TValue>(this IDictionary<TKey, TValue> current, IDictionary<TKey, TValue> target, TKey[] Exceptkey = null)
        {
            return current.ContainsAll(target.AsEnumerable(), Exceptkey);
        }

        /// <summary>
        /// 当前字典是否包含参数中的所有key和value
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="current"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        /// <param name="Exceptkey">排除对比的字段</param>
        public static bool ContainsAll<TKey, TValue>(this IDictionary<TKey, TValue> current, IEnumerable<KeyValuePair<TKey, TValue>> target, TKey[] Exceptkey = null)
        {
            if (target == null) return false;
            foreach (var r in target)
            {

                if (r.Value == null || r.Value.Equals(String.Empty) || (Exceptkey != null && Exceptkey.Any(a => r.Key.Equals(a))))
                {
                    continue;
                }
                var objV = current.TryGetValue<TKey, TValue>(r.Key, default(TValue));
                var v = objV == null ? String.Empty : objV.ToString();
                if (r.Value != null && !r.Value.ToString().Equals(v, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="keys"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static bool ContainsAnyKey<TKey, TValue>(this IDictionary<TKey, TValue> current, TKey[] keys)
        {
            if (keys == null) return false;
            return keys.Any(i => current.ContainsKey(i) && ((current[i] is ValueType) ? !current[i].Equals(default(TValue)) : (current[i] != null)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static RouteValueDictionary CopyAdd(this RouteValueDictionary current, string key, object value)
        {
            var copy = CopyAdd((IDictionary<string, object>)current, key, value);
            return new RouteValueDictionary(copy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static RouteValueDictionary CopySet(this RouteValueDictionary current, string key,
            object value)
        {
            var copy = CopySet((IDictionary<string, object>)current, key, value);
            return new RouteValueDictionary(copy);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static RouteValueDictionary CopyRemove(this RouteValueDictionary current, params string[] keys)
        {
            var copy = CopyRemove((IDictionary<string, object>)current, keys);
            return new RouteValueDictionary(copy);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> CopyAdd<TKey, TValue>(this IDictionary<TKey, TValue> current, TKey key,
            TValue value)
        {
            var copy = current.GetCopy();
            if (copy.ContainsKey(key))
            {
                if (value.GetType() == typeof(string))
                {
                    copy[key] = (TValue)Convert.ChangeType((copy[key].ToString() + " " + value.ToString()), typeof(TValue));
                }
                else
                {
                    copy[key] = value;
                }
            }
            else
            {
                copy.Add(key, value);
            }
            return copy;
        }
        /// <summary>
        /// 按key复制字典 sman 20151111
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="current"></param>
        /// <param name="key">需要复制的key</param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> CopyByKey<TKey, TValue>(this IDictionary<TKey, TValue> current, TKey[] key)
        {
            if (current == null) return new Dictionary<TKey, TValue>();
            return current.Where(a => key.Any(b => a.Key.Equals(b))).ToDictionary(a => a.Key, b => b.Value);

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> CopySet<TKey, TValue>(this IDictionary<TKey, TValue> current, TKey key,
            TValue value)
        {
            var copy = current.GetCopy();
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                copy = copy.CopyRemove(key);
            }
            else
            {
                if (!current.ContainsKey(key)) return CopyAdd(current, key, value);
                copy[key] = value;
            }
            return copy;
        }

        public static IDictionary<TKey, TValue> DeepCopy<TKey, TValue>(TKey key,
            TValue value)
        {
            Dictionary<TKey, TValue> dic = new Dictionary<TKey, TValue>();
            dic.Add(key, value);
            return dic;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <param name="keys"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> CopyRemove<TKey, TValue>(this IDictionary<TKey, TValue> current, params TKey[] keys)
        {
            var copy = current.GetCopy();
            foreach (var key in keys)
            {
                if (copy.ContainsKey(key)) copy.Remove(key);
            }
            return copy;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> GetCopy<TKey, TValue>(this IDictionary<TKey, TValue> current)
        {
            var copy = new KeyValuePair<TKey, TValue>[current.Count];
            current.CopyTo(copy, 0);
            return copy.Where(p => p.Value != null && !string.IsNullOrEmpty(p.Value.ToString())).ToDictionary(i => i.Key, i => i.Value);
        }

        /// <summary>
        /// 返回置顶模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="d"></param>
        /// <returns></returns>
        public static T ToModel<T>(this IDictionary<string, object> d) where T : class
        {
            try
            {
                var type = typeof(T);

                T m = Activator.CreateInstance(type) as T;

                return ToModel(d, m);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 返回置顶模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="d"></param>
        /// <returns></returns>
        public static T ToModel<T>(this IDictionary<string, object> d, T m) where T : class
        {
            try
            {
                var type = typeof(T);

                foreach (var item in type.GetProperties())
                {
                    var v = d.SingleOrDefault(a => a.Key.Equals(item.Name, StringComparison.OrdinalIgnoreCase)).Value;

                    if (v != null)
                    {
                        item.SetValue(m, Convert.ChangeType(v, item.PropertyType), null);
                    }
                }

                return m;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 字典转动态对象
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static dynamic ToDynamic(this IDictionary<string, object> d)
        {
            if (d == null || !d.Any())
            {
                return null;
            }

            dynamic obj = new System.Dynamic.ExpandoObject();

            foreach (KeyValuePair<string, object> item in d)
            {
                ((IDictionary<string, object>)obj).Add(item.Key, item.Value);
            }

            return obj;
        }
    }
}
