using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Pager;
using System.Data;
using Dapper;
namespace System.Data
{
    /// <summary>
    /// Dapper框架分页扩展
    /// </summary>
    public static class PagerActionDapper
    {
        private class SqlModel
        {
            public string SqlPage;
            public string SqlCount;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="conn"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static PagerDataModel<TReturn> QueryPage<TReturn>(this IDbConnection conn, ISearchModel search)
        {
            var r = conn.QueryPage<TReturn>(search.GetSql(), search.page.Value, search.psize.Value, search.GetOrderBy(), (object)search.Parameters);
            r.QueryString = search;
            return r;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="conn"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static PagerDataModel<TReturn> QueryPage<TReturn>(this IDbConnection conn, ISearchModel<TReturn> search)
        {

            var r = conn.QueryPage<TReturn>(search.GetSql(), search.page.Value, search.psize.Value, search.GetOrderBy(), (object)search.Parameters);
            r.QueryString = search;
            return r;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="conn"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static PagerDataModel<TReturn> QueryPage<TFirst, TSecond, TReturn>(this IDbConnection conn, ISearchModel<TFirst, TSecond, TReturn> search)
        {
            var r = conn.QueryPage<TFirst, TSecond, TReturn>(search.GetSql(), search.Map, search.SplitOn, search.page.Value, search.psize.Value, search.GetOrderBy(), (object)search.Parameters);
            r.QueryString = search;
            return r;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="conn"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static PagerDataModel<TReturn> QueryPage<TFirst, TSecond, TThird, TReturn>(this IDbConnection conn, ISearchModel<TFirst, TSecond, TThird, TReturn> search)
        {
            var r = conn.QueryPage<TFirst, TSecond, TThird, TReturn>(search.GetSql(), search.Map, search.SplitOn, search.page.Value, search.psize.Value, search.GetOrderBy(), (object)search.Parameters);
            r.QueryString = search;
            return r;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="conn"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static PagerDataModel<TReturn> QueryPage<TFirst, TSecond, TThird, TFourth, TReturn>(this IDbConnection conn, ISearchModel<TFirst, TSecond, TThird, TFourth, TReturn> search)
        {
            var r = conn.QueryPage<TFirst, TSecond, TThird, TFourth, TReturn>(search.GetSql(), search.Map, search.SplitOn, search.page.Value, search.psize.Value, search.GetOrderBy(), (object)search.Parameters);
            r.QueryString = search;
            return r;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TFifth"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="conn"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public static PagerDataModel<TReturn> QueryPage<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(this IDbConnection conn, ISearchModel<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> search)
        {
            var r = conn.QueryPage<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(search.GetSql(), search.Map, search.SplitOn, search.page.Value, search.psize.Value, search.GetOrderBy(), (object)search.Parameters);
            r.QueryString = search;
            return r;
        }

        /// <summary>
        /// 分页返回动态对象
        /// </summary>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="order"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static PagerDataModel<dynamic> QueryPage(this IDbConnection cnn, string sql, string order, int page = 1, int pageSize = 20, dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null)
        {
            var sqls = GetSql(sql, order, page, pageSize);
            var p = ExecuteCount(cnn, (object)param, sqls.SqlCount, page, pageSize);
            var list = cnn.Query(
                    sqls.SqlPage,
                    (object)param, transaction,
                    buffered,
                    commandTimeout,
                    CommandType.Text);

            var r = new PagerDataModel<dynamic>(list);
            r.Page = p.page;
            r.PageSize = p.pagesize;
            //r.TotalPageCount = p.pagecount;
            r.TotalCount = p.totalCount;
            return r;
        }
        
        /// <summary>
        /// 单表分页
        /// </summary>
        /// <typeparam name="TReturn">返回类型</typeparam>
        /// <param name="cnn"></param>
        /// <param name="sql">sql语句，不带order</param>
        /// <param name="order">order 排序</param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页码大小</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">事务</param>
        /// <param name="buffered">true 返回 list</param>
        /// <param name="commandTimeout">超时</param>
        /// <returns></returns>
        public static PagerDataModel<TReturn> QueryPage<TReturn>(this IDbConnection cnn, string sql, int page = 1, int pageSize = 20, string order = "order by (Select null)", dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null)
        {
            order = order.ToSafeString().ToSqlSafeStr();
            var sqls = GetSql(sql, order, page, pageSize);
            var p = ExecuteCount(cnn, (object)param, sqls.SqlCount, page, pageSize);
            var list = cnn.Query<TReturn>(
                    sqls.SqlPage,
                    (object)param, transaction,
                    buffered,
                    commandTimeout,
                    CommandType.Text);

            var r = new PagerDataModel<TReturn>(list);
            r.Page = p.page;
            r.PageSize = p.pagesize;
            //r.TotalPageCount = p.pagecount;
            r.TotalCount = p.totalCount;
            return r;
        }
        
        /// <summary>
        /// 两表联合查询分页
        /// </summary>
        /// <typeparam name="TFirst">表1</typeparam>
        /// <typeparam name="TSecond">表2</typeparam>
        /// <typeparam name="TReturn">返回类型</typeparam>
        /// <param name="cnn"></param>
        /// <param name="sql">sql 语句</param>
        /// <param name="order">排序</param>
        /// <param name="map">
        ///         lambda 指定表关系 
        ///         例:新闻表.分类属性 = 分类表
        ///         (T_News,T_Class) => new { T_News.F_Class = T_Class  }
        /// </param>
        /// <param name="page">页码</param>
        /// <param name="pageSize">页码大小</param>
        /// <param name="param">参数</param>
        /// <param name="transaction">事务</param>
        /// <param name="buffered"></param>
        /// <param name="splitOn"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static PagerDataModel<TReturn> QueryPage<TFirst, TSecond, TReturn>(
            this IDbConnection cnn, string sql, Func<TFirst, TSecond, TReturn> map, string splitOn = "Id", int page = 1, int pageSize = 20, string order = "order by (Select null)", dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null
            )
        {
            var sqls = GetSql(sql, order, page, pageSize);
            var p = ExecuteCount(cnn, (object)param, sqls.SqlCount, page, pageSize);
            var list = cnn.Query<TFirst, TSecond, TReturn>(
                    sqls.SqlPage,
                    map, (object)param, transaction,
                    buffered,
                    splitOn,
                    commandTimeout,
                    CommandType.Text);

            var r = new PagerDataModel<TReturn>(list);
            r.Page = p.page;
            r.PageSize = p.pagesize;
            //r.TotalPageCount = p.pagecount;
            r.TotalCount = p.totalCount;
            return r;
        }
        
        /// <summary>
        /// 三表联合分页
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="order"></param>
        /// <param name="map"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="splitOn"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static PagerDataModel<TReturn> QueryPage<TFirst, TSecond, TThird, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TReturn> map, string splitOn = "Id", int page = 1, int pageSize = 20, string order = "order by (Select null)", dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null)
        {

            var sqls = GetSql(sql, order, page, pageSize);
            var p = ExecuteCount(cnn, (object)param, sqls.SqlCount, page, pageSize);

            var list = cnn.Query<TFirst, TSecond, TThird, TReturn>(
                    sqls.SqlPage,
                    map, (object)param, transaction,
                    buffered,
                    splitOn,
                    commandTimeout,
                    CommandType.Text);

            var r = new PagerDataModel<TReturn>(list);
            r.Page = p.page;
            r.PageSize = p.pagesize;
            //r.TotalPageCount = p.pagecount;
            r.TotalCount = p.totalCount;
            return r;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="order"></param>
        /// <param name="map"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="splitOn"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static PagerDataModel<TReturn> QueryPage<TFirst, TSecond, TThird, TFourth, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TFourth, TReturn> map, string splitOn = "Id", int page = 1, int pageSize = 20, string order = "order by (Select null)", dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null)
        {

            var sqls = GetSql(sql, order, page, pageSize);
            var p = ExecuteCount(cnn, (object)param, sqls.SqlCount, page, pageSize);

            var list = cnn.Query<TFirst, TSecond, TThird, TFourth, TReturn>(
                    sqls.SqlPage,
                    map, (object)param, transaction,
                    buffered,
                    splitOn,
                    commandTimeout,
                    CommandType.Text);

            var r = new PagerDataModel<TReturn>(list);
            r.Page = p.page;
            r.PageSize = p.pagesize;
            //r.TotalPageCount = p.pagecount;
            r.TotalCount = p.totalCount;
            return r;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <typeparam name="TSecond"></typeparam>
        /// <typeparam name="TThird"></typeparam>
        /// <typeparam name="TFourth"></typeparam>
        /// <typeparam name="TFifth"></typeparam>
        /// <typeparam name="TReturn"></typeparam>
        /// <param name="cnn"></param>
        /// <param name="sql"></param>
        /// <param name="order"></param>
        /// <param name="map"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="param"></param>
        /// <param name="transaction"></param>
        /// <param name="buffered"></param>
        /// <param name="splitOn"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static PagerDataModel<TReturn> QueryPage<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(this IDbConnection cnn, string sql, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map, string splitOn = "Id", int page = 1, int pageSize = 20, string order = "order by (Select null)", dynamic param = null, IDbTransaction transaction = null, bool buffered = true, int? commandTimeout = null)
        {

            var sqls = GetSql(sql, order, page, pageSize);
            var p = ExecuteCount(cnn, (object)param, sqls.SqlCount, page, pageSize);
            var list = cnn.Query<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>(
                    sqls.SqlPage,
                    map, (object)param, transaction,
                    buffered,
                    splitOn,
                    commandTimeout,
                    CommandType.Text);

            var r = new PagerDataModel<TReturn>(list);
            r.Page = p.page;
            r.PageSize = p.pagesize;
            //r.TotalPageCount = p.pagecount;
            r.TotalCount = p.totalCount;

            //SetRetrunValue(r, p);
            return r;
        }

        private struct Page
        {
            public int page;
            public int pagesize;
            public int pagecount;
            public int totalCount;
        }

        private static Page ExecuteCount(IDbConnection cnn, dynamic param, string sql, int page, int pagesize)
        {
            if (page < 1) page = 1;
            if (pagesize < 1) pagesize = 1;

            var r = new Page();
            //string sqlCount = String.Format("set ROWCOUNT 1;select count(0) from ({0}) a;", sql);
            r.totalCount = cnn.Query<int>(sql, (object)param, null, true, null, CommandType.Text).FirstOrDefault();
            r.page = page;
            r.pagecount = (int)Math.Ceiling(((double)r.totalCount / pagesize));
            r.pagesize = pagesize;
            if (r.page > r.pagecount) r.page = r.pagecount;
            return r;
        }
        
        /// <summary>
        /// 返回两个结果集。一个count 一个常规
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="orderby"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        private static SqlModel GetSql(string sql, string orderby = "order by (select null)", int page = 1, int pageSize = 20)
        {

            string first, end;

            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 1;

            #region 查找from的正确位置 （跳过字查询）
            //查找from的位置
            var indexOfSelect = sql.IndexOf(" from ", StringComparison.OrdinalIgnoreCase);
            var selectField = sql.Substring(0, indexOfSelect);
            while (selectField.StringCount("(") != selectField.StringCount(")"))
            {
                indexOfSelect = sql.IndexOf(" from ", indexOfSelect + 1, StringComparison.OrdinalIgnoreCase);
                selectField = sql.Substring(0, indexOfSelect);
            }
            #endregion

            first = String.Format("select * from ({1}", pageSize, sql.Substring(0, indexOfSelect));
            end = sql.Substring(indexOfSelect, sql.Length - indexOfSelect);


            
            int pageStart = (pageSize * (page - 1)) + 1;
            int pageEnd = pageSize * page;

            string sqlPage =
                String.Format(@"
                        {0},row_number() 
                        over({1}) sman_row_number {2})
                            sman_table where sman_row_number BETWEEN {3} and {4} order by sman_row_number",
                first, orderby, end, pageStart, pageEnd
                );
            string sqlCount = String.Format("set ROWCOUNT 1;select count(0) {0}", sql.Substring(indexOfSelect));
            return new SqlModel
            {
                SqlPage = sqlPage,
                SqlCount = sqlCount
            };
        }

    }
}
