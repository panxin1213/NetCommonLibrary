using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace Firm.Core.Common.BLL.Pages
{
    public abstract class CommonBasePage<T> : PageBase<T> where T : class
    {
        /// <summary>
        /// Service集合类
        /// </summary>
        protected CommonService ServiceMain = null;

        public CommonBasePage()
        {
            ServiceMain = new CommonService();
        }
    }


    public abstract class CommonBasePage : PageBase
    {
        /// <summary>
        /// Service集合类
        /// </summary>
        protected CommonService ServiceMain = null;

        public CommonBasePage()
        {
            ServiceMain = new CommonService();
        }
    }
}
