using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq
{
    /// <summary>
    /// Queryable扩展
    /// </summary>
    public static class QueryableEx
    {


        #region between 扩展
        /// <summary>
        /// 定义最小默认时间
        /// </summary>
        public static readonly DateTime SqlDateTimeMin = new DateTime(2000, 1, 1);
        /// <summary>
        /// 定义最大默认时间
        /// </summary>
        public static readonly DateTime SqlDateTimeMax = DateTime.MaxValue.AddDays(-1);
        /// <summary>
        /// 可空日期的Between
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static IQueryable<TSource> WhereBetweenDate<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, DateTime>> keySelector, DateTime? d1, DateTime? d2)           
        {

            DateTime _d1 = d1.GetValueOrDefault(SqlDateTimeMin);
            DateTime _d2 = d2.GetValueOrDefault(SqlDateTimeMax);

            return source.WhereBetweenDate(keySelector, _d1, _d2);
        }
        /// <summary>
        /// 日期Between
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static IQueryable<TSource> WhereBetweenDate<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, DateTime>> keySelector, DateTime d1, DateTime d2)
        {
            ComparableEx.CompareAndSwap(ref d1, ref d2);
            d1 = d1.Date;
            d2 = d2.Date.AddDays(1).Date.AddSeconds(-1);
            return source.WhereBetween(keySelector, d1, d2);
        }
        /// <summary>
        /// Between方法扩展
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="source"></param>
        /// <param name="keySelector"></param>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <returns></returns>
        public static IQueryable<TSource> WhereBetween<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, TKey v1, TKey v2)
            where TKey : IComparable<TKey>
        {

            
            var parameter = keySelector.Parameters;

            ComparableEx.CompareAndSwap(ref v1,ref v2);
            /*
            var _min = v1 as IComparable<TKey>;
            if (_min.CompareTo(v2) > 0)
            {
                var t = v2;
                v2 = v1;
                v1 = t;
            }*/

            Type type = typeof(TKey);
            var constantFrom = Expression.Constant(v1, type);

            var constantTo = Expression.Constant(v2, type);

            

            Expression nonNullProperty = keySelector.Body;

            //如果是Nullable<X>类型，则转化成X类型

            if (IsNullableType(type))
            {

                type = GetNonNullableType(type);

                nonNullProperty = Expression.Convert(keySelector.Body, type);

            }

            var c1 = Expression.GreaterThanOrEqual(nonNullProperty, constantFrom);

            var c2 = Expression.LessThanOrEqual(nonNullProperty, constantTo);

            var c = Expression.AndAlso(c1, c2);

            Expression<Func<TSource, bool>> lambda = Expression.Lambda<Func<TSource, bool>>(c, parameter);



            return source.Where(lambda);



        }


        static bool IsNullableType(Type type)
        {

            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        }



        static Type GetNonNullableType(Type type)
        {

            return type.GetGenericArguments()[0];

            //return IsNullableType(type) ? type.GetGenericArguments()[0] : type;

        }
        #endregion

        #region whereif
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        /// 
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, Expression<Func<T, int, bool>> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, Func<T, bool> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="predicate"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static IEnumerable<T> WhereIf<T>(this IEnumerable<T> source, Func<T, int, bool> predicate, bool condition)
        {
            return condition ? source.Where(predicate) : source;
        }
        #endregion
    }
}