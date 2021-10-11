using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Firm.Core.Common.BLL.Controls
{
    public class CommonBaseControl : System.Web.UI.UserControl
    {
        /// <summary>
        /// Service集合类
        /// </summary>
        protected CommonService ServiceMain = null;

        public CommonBaseControl()
        {
            ServiceMain = new CommonService();
        }
    }
}
