using Firm.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Firm.Core.Admin.Service
{
    public class Admin_Logs_Service : ServiceBase<D_Admin_Logs>
    {
        public Admin_Logs_Service(IDbConnection db)
            : base(db)
        {

        }


        /// <summary>
        /// 插入数据，包含主键
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool InsertWithKeyField(Firm.Model.D_Admin_Logs entity, IDbTransaction trans = null)
        {
            return db.InsertWithKeyField(entity, trans);
        }
    }
}
