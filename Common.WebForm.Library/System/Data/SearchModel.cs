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
using ChinaBM.Common;
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

    public abstract class SearchModel<TReturn> : SearchModel, ISearchModel<TReturn>
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
        /// 
        /// </summary>
        /// <param name="sql"></param>
        public SearchModel(string sql)
        {
            SqlBuilder = new StringBuilder(sql);
            _parameters = new DynamicParameters();

            if (System.Web.HttpContext.Current != null)
            {
                var propertys = this.GetType().GetProperties();
                var n = System.Web.HttpContext.Current.Request.QueryString;

                foreach (var property in propertys)
                {
                    var nv = n.GetValues(property.Name);
                    if (nv == null)
                    {
                        continue;
                    }
                    var v = nv.Where(a => !String.IsNullOrEmpty(a)).ToList();
                    if (v == null || v.Count == 0)
                    {
                        continue;
                    }

                    if (property.PropertyType.IsArray)
                    {
                        var actype = property.PropertyType.Assembly.GetType(property.PropertyType.FullName.Replace("[]", ""));
                        var array = (property.PropertyType.InvokeMember("Set", BindingFlags.CreateInstance, null, null, new object[] { v.Count }));
                        for (var i = 0; i < v.Count; i++)
                        {
                            property.PropertyType.GetMethod("SetValue", new Type[2] { actype, typeof(int) }).Invoke(array, new object[] { Convert.ChangeType(v[i], actype), i });
                        }
                        property.SetValue(this, array, null);
                    }
                    else
                    {
                        var sv = v.FirstOrDefault();
                        if (!String.IsNullOrEmpty(sv))
                        {
                            var chtype = property.PropertyType;
                            if (chtype == typeof(DateTime?))
                            {
                                property.SetValue(this, ConvertKit.Convert<DateTime?>(sv, null), null);
                            }
                            else if (chtype == typeof(bool?))
                            {
                                property.SetValue(this, ConvertKit.Convert<bool?>(sv, null), null);
                            }
                            else if (chtype == typeof(int?))
                            {
                                property.SetValue(this, ConvertKit.Convert<int?>(sv, null), null);
                            }
                            else
                            {
                                property.SetValue(this, Convert.ChangeType(sv, chtype), null);
                            }
                        }
                    }
                }
            }

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


            return InitArrayParms();
        }
        /// <summary>
        /// 运行前调用
        /// </summary>
        /// <returns></returns>
        public virtual string GetOrderBy()
        {
            return "order by (Select null)";
        }
        private string InitArrayParms()
        {
            StringBuilder r = SqlBuilder;
            foreach (var a in enumerableValues)
            {
                r = r.Replace(a.Key, a.Value);
            }
            return r.ToString();
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
                    _psize = value > PageSizeMax ? value : value;
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
                            if (enumerable[i].ToInt() == 0)
                            {
                                continue;
                            }
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
                _inited = true;
            }
        }

        public object GetParameters()
        {
            if (Parameters == null)
                Init();
            return (object)Parameters;
        }
    }
}
