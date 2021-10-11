using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace System.Data
{
    /// <summary>
    /// 翻页基本参数接口
    /// </summary>
    public interface IPagerParameters {
        /// <summary>
        /// 当前页
        /// </summary>
        int? page { get; set; }
        /// <summary>
        /// 页码大小
        /// </summary>
        int? psize { get; set; }

    }
    /// <summary>
    /// 翻页参数
    /// </summary>
    public class PagerParameters : IPagerParameters
    {
        #region IPagerParameters 实现
        /// <summary>
        /// 页码最大值
        /// </summary>
        public int PageSizeMax = 50;
        private int? _page = 1;
        private int? _psize = 10;
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
                if (value.HasValue && value.Value > 0)
                    _psize = value > PageSizeMax ? PageSizeMax : value;
                else
                    _psize = 10;
            }
        }
        #endregion
        
    }

}
