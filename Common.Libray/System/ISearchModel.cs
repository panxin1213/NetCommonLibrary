using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace System.Data
{
    public interface ISearchModel : IPagerParameters, ISearchCondition, ISearchOrderBy, ISearchQueryString
    {
        /// <summary>
        /// 参数
        /// </summary>
        DynamicParameters Parameters { get; }

        /// <summary>
        /// 获取参数的方法
        /// </summary>
        /// <returns></returns>
        object GetParameters();
    }

    public interface ISearchModel<T> : ISearchModel
    {

    }

    public interface ISearchModel<TFirst, TSecond, TReturn> : ISearchModel
    {
        /// <summary>
        /// 告诉dapper另一张表的开始字段
        /// </summary>
        string SplitOn { get; }
        /// <summary>
        /// 指定关系
        /// </summary>
        Func<TFirst, TSecond, TReturn> Map { get; }
    }

    public interface ISearchModel<TFirst, TSecond, TThird, TReturn> : ISearchModel
    {
        /// <summary>
        /// 告诉dapper另一张表的开始字段
        /// </summary>
        string SplitOn { get; }
        /// <summary>
        /// 指定关系
        /// </summary>
        Func<TFirst, TSecond, TThird, TReturn> Map { get; }
    }


    public interface ISearchModel<TFirst, TSecond, TThird, TFourth, TReturn> : ISearchModel
    {
        /// <summary>
        /// 告诉dapper另一张表的开始字段
        /// </summary>
        string SplitOn { get; }
        /// <summary>
        /// 指定关系
        /// </summary>
        Func<TFirst, TSecond, TThird, TFourth, TReturn> Map { get; }
    }

    public interface ISearchModel<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> : ISearchModel
    {
        /// <summary>
        /// 告诉dapper另一张表的开始字段
        /// </summary>
        string SplitOn { get; }
        /// <summary>
        /// 指定关系
        /// </summary>
        Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> Map { get; }
    }
}
