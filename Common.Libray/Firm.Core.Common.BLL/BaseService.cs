using Common.Library;
using Common.Library.Log;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Firm.Core.Common.BLL
{
    /// <summary>
    /// Service链接字符串父类
    /// </summary>
    public abstract class BaseService
    {
        private string DataKey { get; set; }

        protected BaseService(string datakey)
        {
            DataKey = datakey;
        }

        protected IDbConnection db
        {
            get
            {
                try
                {
                    var ServiceMain = System.Web.HttpContext.Current.GetServiceMain();
                    var _db = ServiceMain.DBList.TryGetValue(DataKey, null);
                    if (_db == null)
                    {
                        _db = Conn.GetByKey(DataKey);
                        ServiceMain.DBList.Add(DataKey, _db);
                    }
                    return _db;
                }
                catch (Exception e)
                {
                    Logger.Error(this, e.Message, e);
                }
                return null;
            }
        }
    }
}
