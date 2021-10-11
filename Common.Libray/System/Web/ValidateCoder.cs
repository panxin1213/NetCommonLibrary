using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web
{
    public class ValidateCoder
    {
        protected static string SessionKey = "__ValiedCode";

        static string code = "AGJKLPQRSUXY3456";
        public static string GenerateCheckCode(int len = 4)
        {
            //定义验证码长度
            int codelength = len;
            int number;
            string RandomCode = string.Empty;
            Random r = new Random();
            for (int i = 0; i < codelength; i++)
            {
                number = r.Next(code.Length);

                RandomCode += code[number];
            }
            HttpContext.Current.Session.Add(SessionKey, RandomCode);
            return RandomCode;
        }
        public static bool HasSession()
        {
            return !string.IsNullOrEmpty(HttpContext.Current.Session[SessionKey] as string);
        }
        public static bool CheckCode(string code)
        {
            if (HttpContext.Current.Session[SessionKey] as string == "")
            {
                HttpContext.Current.Session[SessionKey] = null;
                return true;
            }

            if (HttpContext.Current.Session[SessionKey] == null)
            {
                return false;
            }

            bool v = HasSession() ? (code ?? "a").Equals(HttpContext.Current.Session[SessionKey] as string, StringComparison.OrdinalIgnoreCase) : false;
            HttpContext.Current.Session[SessionKey] = "";
            return v;
        }
    }
}
