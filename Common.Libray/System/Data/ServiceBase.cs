using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Linq.Expressions;
using System.Data.Pager;
using Common.Library.Caching;
using Core.Common;
using System.Configuration;
using System.Net;
using Common.Library.Log;
using System.Text.RegularExpressions;
using System.Reflection;

namespace System.Data
{
    public class ServiceBase<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        public ServiceBase(IDbConnection db)
        {
            this.db = db;
            CacheTypes = new List<Type> { typeof(T) };
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="db">数据库链接对象</param>
        /// <param name="cacheTypes">清楚缓存类型数组</param>
        public ServiceBase(IDbConnection db, params Type[] cacheTypes)
        {
            this.db = db;
            if (cacheTypes == null || cacheTypes.Count() == 0)
            {
                CacheTypes = new List<Type> { typeof(T) };
            }
            else
            {
                CacheTypes = cacheTypes.ToList();

                if (!CacheTypes.Any(a => a.FullName == typeof(T).FullName))
                {
                    CacheTypes.Add(typeof(T));
                }
            }
        }



        /// <summary>
        /// 注入的connection
        /// </summary>
        protected IDbConnection db { get; set; }

        private IList<Type> CacheTypes { get; set; }

        /// <summary>
        /// 返回当前类中的数据连接对象
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetThisIDbConnection()
        {
            return db;
        }

        /// <summary>
        /// 按主键查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T Get(dynamic id)
        {

            return db.Get<T>((object)id);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Update(T entity)
        {
            var r = db.Update(entity);
            if (r)
            {
                RemoveCacheByThisType();
            }

            return r;
        }
        /// <summary>
        /// 更新指定字段
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="propertys"></param>
        /// <returns></returns>
        public virtual bool Update(T entity, params Expression<Func<T, object>>[] propertys)
        {
            var r = db.Update(entity, propertys: propertys);
            if (r)
            {
                RemoveCacheByThisType();
            }

            return r;
        }
        /// <summary>
        /// 更新排除指定字段
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="excludePropertys">要排除的字段</param>
        /// <returns></returns>
        public virtual bool UpdateExclude(T entity, params Expression<Func<T, object>>[] excludePropertys)
        {
            var r = db.UpdateExclude(entity, excludePropertys: excludePropertys);
            if (r)
            {
                RemoveCacheByThisType();
            }

            return r;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual bool Delete(T entity)
        {
            var r= db.Delete(entity);
            if (r)
            {
                RemoveCacheByThisType();
            }

            return r;
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual long Insert(T entity)
        {
            var r = db.Insert(entity);
            if (r > 0)
            {
                RemoveCacheByThisType();
            }

            return r;
        }

        /// <summary>
        /// 所有字段（包括主键）一起插入 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected bool InsertWithKeyField(T entity)
        {
            var r = db.InsertWithKeyField(entity);

            if (r)
            {
                RemoveCacheByThisType();
            }

            return r;
        }
        /// <summary>
        /// 批量插入
        /// </summary>
        /// <param name="entites"></param>
        /// <returns>最后的identity</returns>
        public virtual long Insert(IEnumerable<T> entites)
        {
            var r = db.Insert<T>(entites);

            if (r > 0)
            {
                RemoveCacheByThisType();
            }

            return r;
        }
        /// <summary>
        /// 批量插入包括主键
        /// </summary>
        /// <param name="entites"></param>
        /// <returns>返回插入的数量</returns>
        protected virtual int InsertWithKeyField(IEnumerable<T> entites)
        {
            var r = db.InsertWithKeyField<T>(entites);

            if (r > 0)
            {
                RemoveCacheByThisType();
            }

            return r;
        }

        /// <summary>
        /// 排除字段添加
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="excludePropertys"></param>
        /// <returns></returns>
        public virtual long InsertExclude(T entity, params Expression<Func<T, object>>[] excludePropertys)
        {
            var r = db.InsertExclude(entity, excludePropertys: excludePropertys);

            if (r > 0)
            {
                RemoveCacheByThisType();
            }

            return r;
        }


        /// <summary>
        /// 批量更新
        /// </summary>
        /// <param name="entites"></param>
        /// <returns></returns>
        public virtual int Update(IEnumerable<T> entites)
        {
            var r = db.Update<T>(entites);

            if (r > 0)
            {
                RemoveCacheByThisType();
            }

            return r;
        }
        /// <summary>
        /// 批量更新 指定字段
        /// </summary>
        /// <param name="entites"></param>
        /// <param name="propertys">要更新字段</param>
        /// <returns>影响的行数</returns>
        public virtual int Update(IEnumerable<T> entites, params Expression<Func<T, object>>[] propertys)
        {
            var r = db.Update<T>(entites, propertys: propertys);

            if (r > 0)
            {
                RemoveCacheByThisType();
            }

            return r;
        }
        /// <summary>
        /// 更新排除指定字段
        /// </summary>
        /// <param name="entites"></param>
        /// <param name="excludePropertys">要排除的字段</param>
        /// <returns>影响的行数</returns>
        public virtual int UpdateExclude(IEnumerable<T> entites, params Expression<Func<T, object>>[] excludePropertys)
        {
            var r = db.UpdateExclude(entites, excludePropertys: excludePropertys);

            if (r > 0)
            {
                RemoveCacheByThisType();
            }

            return r;
        }


        /// <summary>
        /// 根据条件取得信息总数
        /// </summary>
        /// <param name="conditions"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        protected Dictionary<string, int> GetCountByCondition(IDictionary<string, string> conditions, object param)
        {
            var d = new Dictionary<string, int>();
            var sb = new StringBuilder();

            var i = 0;


            foreach (var item in conditions)
            {
                var wherestr = "select count(0) as cots,'" + item.Key + "' as keys from {1} where {0}";

                sb.Append(string.Format(wherestr, item.Value, typeof(T).Name));

                if (i != conditions.Count - 1)
                {
                    sb.Append(" union ");
                }

                i++;
                //var count = db.Query<int>(, item.Value), param[item.Key]).FirstOrDefault();
                //d.Add(item.Key, count);
            }

            var t = db.Query(sb.ToString(), param);

            foreach (var item in t)
            {
                d.Add(item.keys, item.cots);
            }

            return d;
        }

        /// <summary>
        /// 清除当前类型的缓存
        /// </summary>
        public virtual void RemoveCacheByThisType()
        {
            foreach (var item in CacheTypes)
            {
                CacheManager.RemoveByPrefix(item.FullName);
            }

            try
            {
                var cacheUrl = ConfigurationManager.AppSettings["CacheUrl"].ToSafeString();
                if (!String.IsNullOrWhiteSpace(cacheUrl))
                {
                    using (var client = new WebClient())
                    {
                        client.Headers["Host"] = new Regex(@"http://(?<key>((?!/).)*)(/|$)", RegexOptions.IgnoreCase).Match(cacheUrl).Groups["key"].Value;
                        Logger.Info("", client.Headers["Host"] + ":" + cacheUrl, null);
                        client.DownloadString(cacheUrl);
                    }
                }
            }
            catch (Exception e)
            {
                Logger.Error(this, e.Message, e);
            }
        }






        #region Page

        /// <summary>
        /// 一般分页
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public virtual PagerDataModel<T> List(ISearchModel search)
        {
            return db.QueryPage<T>(search);
        }

        public virtual PagerDataModel<dynamic> List(ISearchModel<dynamic> search)
        {
            return db.QueryPage<dynamic>(search);
        }
        /// <summary>
        /// 联合查询分页
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <param name="search"></param>
        /// <returns></returns>
        public virtual PagerDataModel<T> List<T2>(ISearchModel<T, T2, T> search)
        {
            return db.QueryPage<T, T2, T>(search);
        }
        /// <summary>
        /// 联合查询分页
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <param name="search"></param>
        /// <returns></returns>
        public virtual PagerDataModel<T> List<T2, T3>(ISearchModel<T, T2, T3, T> search)
        {
            return db.QueryPage<T, T2, T3, T>(search);
        }
        /// <summary>
        /// 联合查询分页
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <param name="search"></param>
        /// <returns></returns>
        public virtual PagerDataModel<T> List<T2, T3, T4>(ISearchModel<T, T2, T3, T4, T> search)
        {
            return db.QueryPage<T, T2, T3, T4, T>(search);
        }
        /// <summary>
        /// 联合查询分页
        /// </summary>
        /// <typeparam name="T2"></typeparam>
        /// <typeparam name="T3"></typeparam>
        /// <typeparam name="T4"></typeparam>
        /// <typeparam name="T5"></typeparam>
        /// <param name="search"></param>
        /// <returns></returns>
        public virtual PagerDataModel<T> List<T2, T3, T4, T5>(ISearchModel<T, T2, T3, T4, T5, T> search)
        {
            return db.QueryPage<T, T2, T3, T4, T5, T>(search);
        }

        #endregion


        #region List

        /// <summary>
        /// 通过分页模型获取sql
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        private string GetSql(ISearchModel search)
        {
            var sql = search.GetSql() + search.GetOrderBy();

            if (search.psize > 0)
            {
                sql = sql.Substring(0, sql.IndexOf("select", StringComparison.OrdinalIgnoreCase) + 6) + " top " + search.psize + sql.Substring(sql.IndexOf("select", StringComparison.OrdinalIgnoreCase) + 6);
            }

            return sql;
        }

        public virtual List<T> GetListBySearchModel<TOne, Ttwo, Tthree, TFour, TFive, T>(ISearchModel<TOne, Ttwo, Tthree, TFour, TFive, T> search, bool iscache = true)
        {
            var sql = GetSql(search);

            Func<List<T>> func = () =>
            {
                var q = db.Query(sql, search.Map, search.GetParameters(), splitOn: search.SplitOn);

                return q.ToList();
            };

            var paramstr = GetSearchModelParamtersJsonString(search);

            var cachekey = String.IsNullOrEmpty(paramstr) ? "" : CacheKey.GetCacheKey<T>("GetListBySearchModel", sql, paramstr);

            return IsCacheCommonFunc(func, cachekey, iscache);
        }

        public virtual List<T> GetListBySearchModel<TOne, Ttwo, Tthree, TFour, T>(ISearchModel<TOne, Ttwo, Tthree, TFour, T> search, bool iscache = true)
        {
            var sql = GetSql(search);

            Func<List<T>> func = () =>
            {
                var q = db.Query(sql, search.Map, search.GetParameters(), splitOn: search.SplitOn);

                return q.ToList();
            };

            var paramstr = GetSearchModelParamtersJsonString(search);

            var cachekey = String.IsNullOrEmpty(paramstr) ? "" : CacheKey.GetCacheKey<T>("GetListBySearchModel", sql, paramstr);

            return IsCacheCommonFunc(func, cachekey, iscache);
        }

        public virtual List<T> GetListBySearchModel<TOne, Ttwo, Tthree, T>(ISearchModel<TOne, Ttwo, Tthree, T> search, bool iscache = true)
        {
            var sql = GetSql(search);

            Func<List<T>> func = () =>
            {
                var q = db.Query(sql, search.Map, search.GetParameters(), splitOn: search.SplitOn);

                return q.ToList();
            };

            var paramstr = GetSearchModelParamtersJsonString(search);

            var cachekey = String.IsNullOrEmpty(paramstr) ? "" : CacheKey.GetCacheKey<T>("GetListBySearchModel", sql, paramstr);

            return IsCacheCommonFunc(func, cachekey, iscache);
        }

        public virtual List<T> GetListBySearchModel<TOne, Ttwo, T>(ISearchModel<TOne, Ttwo, T> search, bool iscache = true)
        {
            var sql = GetSql(search);

            Func<List<T>> func = () =>
            {
                var q = db.Query(sql, search.Map, search.GetParameters(), splitOn: search.SplitOn);

                return q.ToList();
            };

            if (iscache)
            {

                var paramstr = GetSearchModelParamtersJsonString(search);

                var cachekey = String.IsNullOrEmpty(paramstr) ? "" : CacheKey.GetCacheKey<T>("GetListBySearchModel", sql, paramstr);

                return IsCacheCommonFunc(func, cachekey, iscache);
            }
            else
            {
                return func();
            }
        }

        public virtual List<T> GetListBySearchModel(ISearchModel search, bool iscache = true)
        {
            var sql = GetSql(search);

            Func<List<T>> func = () =>
            {
                var q = db.Query<T>(sql, search.GetParameters());

                return q.ToList();
            };

            if (!iscache)
            {
                return func();
            }

            var paramstr = GetSearchModelParamtersJsonString(search);

            var cachekey = String.IsNullOrEmpty(paramstr) ? "" : CacheKey.GetCacheKey<T>("GetListBySearchModel", sql, paramstr);

            return IsCacheCommonFunc(func, cachekey, iscache);
        }

        private string GetSearchModelParamtersJsonString(ISearchModel search)
        {
            var filed = search.Parameters.GetType().GetField("parameters", BindingFlags.NonPublic | BindingFlags.Instance);
            if (filed == null)
            {
                return null;
            }
            var o = filed.GetValue(search.Parameters);

            if (o == null)
            {
                return null;
            }
            return o.ToJson();
        }


        #endregion


        /// <summary>
        /// 公共缓存方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="func"></param>
        /// <param name="cachekey"></param>
        /// <param name="usecache"></param>
        /// <returns></returns>
        public T IsCacheCommonFunc<T>(Func<T> func, string cachekey, bool usecache = true) where T : class,new()
        {
            if (usecache)
            {
                return CacheManager.Get<T>(cachekey, func) ?? (typeof(T).IsGenericType || typeof(T).IsArray ? new T() : null);
            }
            return func();
        }

    }
}
