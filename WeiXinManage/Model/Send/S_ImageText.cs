using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinManage.Model
{
    /// <summary>
    /// 图文信息
    /// </summary>
    public class S_ImageText : SendBase
    {
        public S_ImageText()
        {
            Articles = new List<Article>();
            base.MsgType = "news";
        }

        /// <summary>
        /// 图文消息个数，限制为10条以内，必须
        /// </summary>
        protected int ArticleCount
        {
            get
            {
                return Articles.Count();
            }
        }

        public List<Article> Articles { get; set; }
    }
}
