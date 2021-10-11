using Common.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firm.Core.Common.BLL
{
    public class ServiceMain : IDisposable
    {
        ~ServiceMain()
        {
            Dispose();
        }

        private Dictionary<string, IDbConnection> dblist = new Dictionary<string, IDbConnection>();

        /// <summary>
        /// 数据库链接集合
        /// </summary>
        public Dictionary<string, IDbConnection> DBList
        {
            get
            {
                return dblist;
            }
        }

        public void Dispose()
        {
            while (dblist.Count > 0)
            {
                var item = dblist.ElementAt(0);
                if (item.Value != null)
                {
                    item.Value.Close();
                    item.Value.Dispose();
                    dblist.Remove(item.Key);
                }
            }
        }
    }
}
