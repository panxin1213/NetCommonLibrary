using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Config
{
    /// <summary>    
    /// 重写规则的数据对象    
    /// </summary>    
    [Serializable()]
    public class RewriterRule
    {
        private string mLookFor;
        private string mSendTo;
        /// <summary>        
        /// 查找规则       
        /// </summary>        
        public string LookFor
        {
            get { return this.mLookFor; }
            set { this.mLookFor = value; }
        }
        /// <summary>        
        /// 重写规则        
        /// </summary>        
        public string SendTo
        {
            get { return this.mSendTo; }
            set { this.mSendTo = value; }
        }

        public string Host { get; set; }
    }//end class
}
