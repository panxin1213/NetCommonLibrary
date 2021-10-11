using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;

namespace Common.Library
{
    public class Conn
    {
        static readonly string _connectionString = ConfigurationManager.ConnectionStrings["Cms"].ConnectionString;
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


        /// <summary>
        /// 根据webconfig数据库字符串链接key去数据库字符串链接
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static SqlConnection GetByKey(string key)
        {
            var q = new SqlConnection(ConfigurationManager.ConnectionStrings[key].ConnectionString);
            q.Open();
            return q;
        }
    }
}
