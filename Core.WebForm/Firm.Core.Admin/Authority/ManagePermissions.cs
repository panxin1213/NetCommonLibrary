using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Web;
using System.Configuration;

namespace Firm.Core.Admin.Authority
{
    public class ManagePermissions
    {
        /// <summary>
        /// 取得所有有Description备注的Controller，Action中必须有Description备注才能被取出
        /// </summary>
        /// <returns></returns>
        public static IDictionary<string, List<PermissionsModel>> GetAllPermissions()
        {
            IDictionary<string, List<PermissionsModel>> icdiction = new Dictionary<string, List<PermissionsModel>>();


            var ab = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName.StartsWith(ConfigurationManager.AppSettings["AdminMainAssemblName"] ?? "Firm.Web"))
                .FirstOrDefault().GetTypes().Where(a => a.Namespace != null && (a.Namespace.StartsWith("Firm.Web.manage") || a.Namespace.StartsWith((ConfigurationManager.AppSettings["AdminMainAssemblName"] ?? "Firm.Web") + ".manage")))
                .Where(a => typeof(WebBase).IsAssignableFrom(a)).GroupBy(a => a.Namespace).ToDictionary(a => a.Key, a => a.ToList());

            foreach (var c in ab)
            {
                var list = new List<PermissionsModel>();

                foreach (var item in c.Value)
                {
                    var des = item.GetCustomAttributes(typeof(DescriptionAttribute), true).FirstOrDefault() as DescriptionAttribute;
                    if (des != null)
                    {
                        list.Add(new PermissionsModel { ClassName = item.Name, Des = des.Description, NameSpace = c.Key });
                    }
                }

                if (list.Count > 0)
                {
                    icdiction.Add(c.Key, list);
                }

            }

            return icdiction;
        }

        public class PermissionsModel
        {
            /// <summary>
            /// Action备注
            /// </summary>
            public string Des { get; set; }

            /// <summary>
            /// 命名空间
            /// </summary>
            public string NameSpace { get; set; }

            /// <summary>
            /// 类名
            /// </summary>
            public string ClassName { get; set; }
        }
    }
}
