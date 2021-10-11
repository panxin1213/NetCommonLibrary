using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Pager;
using System.Reflection;
using Dapper;
using System.Dynamic;
using System.ComponentModel.DataAnnotations;
using System.Collections;
namespace System.Data
{
    /// <summary>
    /// 搜索模型，用于五表联合
    /// </summary>
    /// <typeparam name="TFirst"></typeparam>
    /// <typeparam name="TSecond"></typeparam>
    /// <typeparam name="TThird"></typeparam>
    /// <typeparam name="TFourth"></typeparam>
    /// <typeparam name="TFifth"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    public abstract class SearchModel<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> : SearchModel, ISearchModel<TFirst, TSecond, TThird, TFourth, TFifth, TReturn>
    {
        private string _splitOn = "id";

        /// <summary>
        /// 告诉dapper另一张表的开始字段
        /// </summary>
        public string SplitOn
        {
            get
            {
                return _splitOn;
            }
        }


        private Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> _map;

        /// <summary>
        /// 指定关系
        /// </summary>
        public Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> Map
        {
            get
            {
                return _map;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="spliton">告诉dapper另一张表的开始字段</param>
        /// <param name="map">表关系</param>
        public SearchModel(string sql, string spliton, Func<TFirst, TSecond, TThird, TFourth, TFifth, TReturn> map) :
            base(sql)
        {
            _splitOn = spliton;
            _map = map;
        }
    }

    /// <summary>
    /// 搜索模型，用于四表联合
    /// </summary>
    /// <typeparam name="TFirst"></typeparam>
    /// <typeparam name="TSecond"></typeparam>
    /// <typeparam name="TThird"></typeparam>
    /// <typeparam name="TFourth"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    public abstract class SearchModel<TFirst, TSecond, TThird, TFourth, TReturn> : SearchModel, ISearchModel<TFirst, TSecond, TThird, TFourth, TReturn>
    {
        private string _splitOn = "id";

        /// <summary>
        /// 告诉dapper另一张表的开始字段
        /// </summary>
        public string SplitOn
        {
            get
            {
                return _splitOn;
            }
        }


        private Func<TFirst, TSecond, TThird, TFourth, TReturn> _map;

        /// <summary>
        /// 指定关系
        /// </summary>
        public Func<TFirst, TSecond, TThird, TFourth, TReturn> Map
        {
            get
            {
                return _map;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="spliton">告诉dapper另一张表的开始字段</param>
        /// <param name="map">指定关系</param>
        public SearchModel(string sql, string spliton, Func<TFirst, TSecond, TThird, TFourth, TReturn> map) :
            base(sql)
        {
            _splitOn = spliton;
            _map = map;
        }
    }

    /// <summary>
    /// 搜索模型，用于三表联合
    /// </summary>
    /// <typeparam name="TFirst"></typeparam>
    /// <typeparam name="TSecond"></typeparam>
    /// <typeparam name="TThird"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    public abstract class SearchModel<TFirst, TSecond, TThird, TReturn> : SearchModel, ISearchModel<TFirst, TSecond, TThird, TReturn>
    {
        private string _splitOn = "id";

        /// <summary>
        /// 告诉dapper另一张表的开始字段
        /// </summary>
        public string SplitOn
        {
            get
            {
                return _splitOn;
            }
        }


        private Func<TFirst, TSecond, TThird, TReturn> _map;

        /// <summary>
        /// 指定关系
        /// </summary>
        public Func<TFirst, TSecond, TThird, TReturn> Map
        {
            get
            {
                return _map;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="spliton">告诉dapper另一张表的开始字段</param>
        /// <param name="map">指定关系</param>
        public SearchModel(string sql, string spliton, Func<TFirst, TSecond, TThird, TReturn> map) :
            base(sql)
        {
            _splitOn = spliton;
            _map = map;
        }
    }

    /// <summary>
    /// 搜索模型，用于两表联合
    /// </summary>
    /// <typeparam name="TFirst"></typeparam>
    /// <typeparam name="TSecond"></typeparam>
    /// <typeparam name="TReturn"></typeparam>
    public abstract class SearchModel<TFirst, TSecond, TReturn> : SearchModel, ISearchModel<TFirst, TSecond, TReturn>
    {
        private string _splitOn = "id";

        /// <summary>
        /// 告诉dapper另一张表的开始字段
        /// </summary>
        public string SplitOn
        {
            get
            {
                return _splitOn;
            }
        }


        private Func<TFirst, TSecond, TReturn> _map;

        /// <summary>
        /// 指定关系
        /// </summary>
        public Func<TFirst, TSecond, TReturn> Map
        {
            get
            {
                return _map;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="spliton">告诉dapper另一张表的开始字段</param>
        /// <param name="map">指定关系</param>
        public SearchModel(string sql, string spliton, Func<TFirst, TSecond, TReturn> map) :
            base(sql)
        {
            _splitOn = spliton;
            _map = map;
        }
    }

    public abstract class SearchModel<TReturn> : SearchModel
    {
        public SearchModel(string sql) : base(sql) { }

    }

    /// <summary>
    /// 单表搜索模型
    /// </summary>
    public abstract class SearchModel : ISearchModel
    {
        private IDictionary<string, string> enumerableValues = new Dictionary<string, string>();

        internal DynamicParameters _parameters;

        /// <summary>
        /// 动态参数
        /// </summary>
        public DynamicParameters Parameters
        {
            get
            {
                return _parameters;
            }
        }

        /// <summary>
        /// 动态语句
        /// </summary>
        protected readonly StringBuilder SqlBuilder;

        /// <summary>
        /// 可在GetSqlCondition中移除动态语句字典，用于非List(SearchModel)调用时 sman 20141113
        /// </summary>
        protected Dictionary<string, string> SqlBuilderRemovable = new Dictionary<string, string>();

        /// <summary>
        /// 基础select 语句 sman 20141113
        /// </summary>
        private string _sqlBaseSelect;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        protected SearchModel(string sql)
        {
            _sqlBaseSelect = sql;
            SqlBuilder = new StringBuilder();
            _parameters = new DynamicParameters();
        }

        /// <summary>
        /// 是否已经初始化
        /// </summary>
        private bool _inited = false;

        /// <summary>
        /// 初始化，一次
        /// </summary>
        private void Init()
        {
            if (!_inited)
            {
                BulildSql();
                InitQueryString();
                InitArrayParms();
                _inited = true;
            }
        }

        /// <summary>
        /// 替换sql参数
        /// </summary>
        /// <param name="objs"></param>
        protected void FormatSql(params object[] objs)
        {
            if (objs == null || objs.Length == 0)
            {
                return;
            }

            _sqlBaseSelect = string.Format(_sqlBaseSelect, objs);
        }

        /// <summary>
        /// 获取查询条件的sql
        /// exceptCondition 参数通常用于需要使用相同SearchModel，不同Select 字段，分组查询等
        /// 
        /// 说明：
        /// 1.SearchModel中通过 SqlBuilderRemovable 添加查询条件
        /// 例：if (!String.IsNullOrEmpty(state)) SqlBuilderRemovable.Add("state", " and f_state = @state");
        /// 2.Service中调用GetSqlCondition中传入相同的key的字符串数组，侧获取到的条件中此key中的条件便被移除。
        /// var sql =
        ///      string.Format(
        ///            "select f_state,count(0) as c from table with(nolock) where 1=1 {0} group by f_state",
        ///            s.GetSqlCondition("state"));
        ///    return db.Query(sql, s.GetParameters())
        ///        .ToDictionary(_ => (string)_.f_state, _ => (int)_.c);
        /// </summary>
        /// <param name="exceptCondition">要排除的，使用SqlBuilderRemovable添加的语句的key</param>
        /// <returns></returns>
        public string GetSqlCondition(params string[] exceptCondition)
        {
            Init();
            foreach (var ex in exceptCondition)
            {
                SqlBuilderRemovable.Remove(ex); //按key移除转过来的条件
            }
            return SqlBuilder.ToString() + String.Join(" ", SqlBuilderRemovable.Select(_ => _.Value));
        }

        /// <summary>
        /// 获取所有参数，用于传参
        /// </summary>
        /// <returns></returns>
        public object GetParameters()
        {
            if (Parameters == null)
            Init();
            return (object)Parameters;
        }

        /// <summary>
        /// 
        /// </summary>
        public IDictionary<string, object> QueryString { get; private set; }

        /// <summary>
        /// 在子类中构造查询语句
        /// </summary>
        public abstract void BulildSql();

        /// <summary>
        /// 运行前调用
        /// </summary>
        /// <returns></returns>
        public string GetSql()
        {

            Init();

            return _sqlBaseSelect + SqlBuilder.ToString();
        }

        /// <summary>
        /// 可用于在生成url进行初始化参数值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual IDictionary<string, object> Add(string key, object value)
        {
            if (string.IsNullOrEmpty(key)) new Dictionary<string, object> { };
            var r = new Dictionary<string, object>(this.QueryString);
            if (r.ContainsKey(key))
            {
                r[key] = value;
            }
            else
            {
                r.Add(key, value);
            }
            return r;
        }
        /// <summary>
        /// 运行前调用
        /// </summary>
        /// <returns></returns>
        public virtual string GetOrderBy()
        {
            return "order by (Select null)";
        }
        private void InitArrayParms()
        {
            foreach (var a in enumerableValues)
            {
                SqlBuilder.Replace(a.Key, a.Value);
            }
        }

        #region IPagerParameters 实现
        /// <summary>
        /// 页码最大值
        /// </summary>
        public int PageSizeMax = 50;
        private int? _page = 1;
        private int? _psize = 20;
        /// <summary>
        /// 当前页
        /// </summary>
        public int? page
        {
            get
            {

                return _page;
            }
            set
            {
                if (value.HasValue && value.Value > 0)
                    _page = value;
                else
                    page = 1;
            }
        }
        /// <summary>
        /// 页码大小
        /// </summary>
        public int? psize
        {
            get
            {
                return _psize;
            }
            set
            {
                if (value.HasValue)
                    _psize = value > PageSizeMax ? PageSizeMax : value;
                else
                    _psize = 20;
            }
        }
        #endregion

        #region ISearchQueryString 实现
        /// <summary>
        /// 按继承的模型字段,取得所有搜索条件参数 
        /// </summary>
        /// <returns></returns>
        public void InitQueryString()
        {
            _parameters = new DynamicParameters();
            enumerableValues.Clear();
            var x = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var r = new Dictionary<string, object>();

            var nullableBoolen = typeof(bool?);


            foreach (PropertyInfo y in x)
            {
                if (!y.CanWrite) continue;

                var v = y.GetValue(this, null);



                var t = Nullable.GetUnderlyingType(y.PropertyType) ?? y.PropertyType;
                if (typeof(DateTime).IsAssignableFrom(t))
                {
                    var datatype = y.GetCustomAttributes(typeof(DataTypeAttribute), false).SingleOrDefault() as DataTypeAttribute;
                    if (null != v)
                    {

                        if (datatype != null && datatype.DataType == DataType.Date)
                        {
                            r.Add(y.Name, ((DateTime)v).ToString("yyyy-MM-dd"));
                            Parameters.Add(y.Name, v.ToString());
                        }
                        else
                        {
                            r.Add(y.Name, ((DateTime)v).ToString("yyyy-MM-dd hh:mm:ss"));
                            Parameters.Add(y.Name, v.ToString());
                        }
                    }
                    else
                    {

                        Parameters.Add(y.Name, null);
                    }
                }
                else if (t.IsEnum)
                {
                    var datatype = y.GetCustomAttributes(typeof(DataTypeAttribute), false).SingleOrDefault() as DataTypeAttribute;
                    if (datatype != null && datatype.DataType == DataType.Text)
                    {
                        r.Add(y.Name, v.ToString());
                        Parameters.Add(y.Name, v.ToString());
                    }
                    else
                    {
                        r.Add(y.Name, v);
                        Parameters.Add(y.Name, v);
                    }

                }
                else if (t.IsValueType || t.FullName == "System.String")
                {
                    r.Add(y.Name, v);
                    Parameters.Add(y.Name, v);
                }
                else if (t.IsArray || typeof(IEnumerable).IsAssignableFrom(t))
                {
                    var d = v as IEnumerable;
                    if (d != null)
                    {
                        var j = 0;
                        var enumerable = d as object[] ?? d.Cast<object>().ToArray();
                        for (var i = 0; i < enumerable.Count(); i++)
                        {
                            //if (enumerable[i].ToInt() == 0)
                            //{
                            //    continue;
                            //}
                            r.Add(y.Name + "[" + j + "]", enumerable[i]);
                            j++;
                        }
                    }
                    if (typeof(IEnumerable<string>).IsAssignableFrom(t))
                    {
                        enumerableValues.Add(String.Format("@{0}", y.Name), String.Format("'{0}'", (v as IEnumerable<string>).ToSqlSafeStrs()));
                        continue;
                    }
                    else if (typeof(IEnumerable<int>).IsAssignableFrom(t))
                    {
                        enumerableValues.Add(String.Format("@{0}", y.Name), (v as IEnumerable<int>).ToSqlSafeNums());
                        continue;
                    }
                    else if (typeof(IEnumerable<int?>).IsAssignableFrom(t))
                    {
                        enumerableValues.Add(String.Format("@{0}", y.Name), (v as IEnumerable<int?>).ToSqlSafeNums());
                        continue;
                    }
                    else if (typeof(IEnumerable<long>).IsAssignableFrom(t))
                    {
                        enumerableValues.Add(String.Format("@{0}", y.Name), (v as IEnumerable<long>).ToSqlSafeNums());
                        continue;
                    }

                    

                    throw new Exception(String.Format("搜索模型，不支持此集合类型 {0}", t));
                }
                else
                {
                    throw new Exception(String.Format("搜索模型，不支持此类型 {0}", t));
                }



            }

            QueryString = r;

        }

        #endregion

        public virtual string GetSearchTitle()
        {
            throw new Exception("请重写SearchModel 的GetSearchTitle方法 ");
        }

        public virtual string GetSearchMetaKey()
        {
            throw new Exception("请重写SearchModel 的GetSearchMetaKey方法 ");
        }

        public virtual string GetSearchMetaContent()
        {
            throw new Exception("请重写SearchModel 的GetSearchMetaContent方法 ");
        }

        public virtual Dictionary<string, List<string>> GetSearchKeyList()
        {
            throw new Exception("请重写SearchModel 的GetSearchKeyList方法 否则无法调用Html.GetSEOTitleKeyAndContent ");
        }
    }

}
