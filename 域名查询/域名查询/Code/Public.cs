using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace 域名查询.Code
{
    public class Public
    {
        public static string UrlEncode(string str, System.Text.Encoding encode)
        {
            StringBuilder sb = new StringBuilder();
            byte[] byStr = encode.GetBytes(str); //默认是System.Text.Encoding.Default.GetBytes(str)
            for (int i = 0; i < byStr.Length; i++)
            {
                sb.Append(@"%" + Convert.ToString(byStr[i], 16));
            }

            return (sb.ToString());
        }
        public static string UrlEncode(string str)
        {
            return UrlEncode(str, System.Text.Encoding.UTF8);
        }

    }
}
