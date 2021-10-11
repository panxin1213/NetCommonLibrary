using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace System.Config
{
    [Serializable()]
    public class RewriterRuleCollection : CollectionBase
    {
        /// <summary>        
        /// 向集合中添加新规则        
        /// </summary>        
        /// <param name="r">RewriterRule对象</param>        
        public virtual void Add(RewriterRule r)
        {
            this.InnerList.Add(r);
        }
        /// <summary>        
        /// 获取或设置项        
        /// </summary>        
        public RewriterRule this[int index]
        {
            get
            {
                return (RewriterRule)this.InnerList[index];
            }
            set { this.InnerList[index] = value; }
        }
    }//end class
}
