using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Core.Admin
{
    public class AdminConn
    {
        static readonly string _connectionString = ConfigurationManager.ConnectionStrings["Admin"].ConnectionString;
        /// <summary>
        /// 获取一个连接
        /// </summary>
        /// <returns></returns>
        public static SqlConnection Get()
        {
            var q = new SqlConnection(_connectionString);
            q.Open();
            return q;
        }
    }
}
