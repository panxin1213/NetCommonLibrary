using System.Collections.Generic;


namespace System.Data.Pager
{
    /// <summary>
    /// 翻页数据
    /// </summary>
    public interface IPagerData
    {
        /// <summary>
        /// 当前页
        /// </summary>
        int Page { get; set; }
        /// <summary>
        /// 页码大小
        /// </summary>
        int PageSize { get; set; }
        /// <summary>
        /// 总条数
        /// </summary>
        int TotalCount { get; set; }
        /// <summary>
        /// 总数据量
        /// </summary>
        int TotalPageCount { get; }
    }

    /// <summary>
    /// 分页数据接口
    /// </summary>
    public interface IPagerDataAndQuery : IPagerData
    {
        /// <summary>
        /// 包括Model搜索值的对象
        /// </summary>
        ISearchQueryString QueryString { get; }

    }
    /// <summary>
    /// 页面搜索模型接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPagerDataModel<T> : IList<T>, IPagerDataAndQuery
    {

    }

    /// <summary>
    /// 分页数据模型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagerDataModel<T> : List<T>, IPagerDataModel<T>
    {
        /// <summary>
        /// 构造
        /// </summary>
        public PagerDataModel() : base() { }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="collection">与List一至</param>
        public PagerDataModel(IEnumerable<T> collection) : base(collection) { }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="capacity">与List一至</param>
        public PagerDataModel(int capacity) : base(capacity) { }

        private int _page = 1;
        private int _pagesize = 1;
        /// <summary>
        /// 当前页码
        /// </summary>
        public int Page
        {
            get { return _page; }
            set
            {
                if (value < 1)
                    _page = 1;
                else
                    _page = value;
            }
        }

        /// <summary>
        /// 页码大小
        /// </summary>
        public int PageSize
        {
            get { return _pagesize; }
            set
            {
                if (value < 1)
                    _pagesize = 1;
                else
                    _pagesize = value;
            }
        }
        /// <summary>
        /// 总条数
        /// </summary>
        public int TotalCount { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPageCount
        {
            get
            {
                var pagecount = TotalCount / PageSize;

                if (TotalCount % PageSize > 0)
                {
                    return pagecount + 1;
                }
                return pagecount;
            }
        }



        private ISearchQueryString _query;
        /// <summary>
        /// 搜索参数集合
        /// </summary>
        public ISearchQueryString QueryString
        {
            get
            {
                return _query;
            }
            //internal set
            set
            {
                _query = value;
            }
        }

        /// <summary>
        /// 全部元素转换成另一元素
        /// </summary>
        /// <typeparam name="TOutput"></typeparam>
        /// <param name="converter"></param>
        /// <returns></returns>
        public new PagerDataModel<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
        {
            if (converter == null)
            {
                throw new ArgumentNullException("converter", "转换方法参数“converter”不能为Null");
            }
            var list = new PagerDataModel<TOutput>(this.Count)
            {
                Page = this.Page,
                PageSize = this.PageSize,
                QueryString = this.QueryString,
                TotalCount = this.TotalCount,
                //TotalPageCount = this.TotalPageCount,
            };
            list.AddRange(base.ConvertAll(converter));

            return list;
        }


    }



}
