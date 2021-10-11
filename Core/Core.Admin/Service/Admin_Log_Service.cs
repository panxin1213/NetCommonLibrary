using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using Admin.Model;

namespace Core.Admin.Service
{
    public class Admin_Log_Service : ServiceBase<T_Admin_Log>
    {
        public Admin_Log_Service(IDbConnection db)
            : base(db)
        {

        }


        public bool Insert(T_Admin_Log m)
        {
            return db.InsertWithKeyField(m);
        }
    }
}
