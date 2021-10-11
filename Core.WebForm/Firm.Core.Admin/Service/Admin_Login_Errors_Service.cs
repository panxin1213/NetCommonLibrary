using Firm.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Firm.Core.Admin.Service
{
    public class Admin_Login_Errors_Service : ServiceBase<D_Admin_Login_Errors>
    {
        public Admin_Login_Errors_Service(IDbConnection db)
            : base(db)
        {


        }
    }
}
