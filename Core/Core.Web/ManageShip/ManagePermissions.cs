using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using Common.Library.Plugin;
using Core.Admin.Controllers;

namespace Core.Web.ManageShip
{
    public class ManagePermissions
    {
        /// <summary>
        /// 取得所有有Description备注的Controller，Action中必须有Description备注才能被取出
        /// </summary>
        /// <returns></returns>
        public static IDictionary<string, List<ControllerModel>> GetAllPermissions()
        {
            IDictionary<string, List<ControllerModel>> icdiction = new Dictionary<string, List<ControllerModel>>();

            var ab =
                PluginManager.GetAssemblies()
                    .Select(i => i.GetTypes().Where(t => typeof(ManageController).IsAssignableFrom(t)))
                    .Aggregate((t1, t2) => t1.Union(t2).ToArray());

            foreach (var c in ab)
            {
                DescriptionAttribute cd = null;

                var cg = c.GetCustomAttributes(typeof(DescriptionAttribute), true);

                if (cg.Length > 0)
                {
                    cd = cg[0] as DescriptionAttribute;
                }
                if (cd != null)
                {
                    string controllerName = c.Name.Left(c.Name.LastIndexOf("Controller", StringComparison.OrdinalIgnoreCase)); ;
                    List<ControllerModel> list = new List<ControllerModel>();
                    if (!icdiction.ContainsKey(controllerName))
                    {
                        //icdiction.Add(controllerName, list);
                        icdiction.Add(cd.Description,list);
                    }

                    var typeOfMethods = c.GetMethods().Where(a => a.IsPublic && !a.IsSpecialName);
                    foreach (var m in typeOfMethods)
                    {
                        var dl = m.GetCustomAttributes(typeof(DescriptionAttribute), true);
                        if (dl.Length == 0)
                        {
                            continue;
                        }

                        var d = dl[0] as DescriptionAttribute;
                        if (d != null)
                        {
                            ControllerModel cm = new ControllerModel();
                            cm.Action = m.Name;
                            cm.Controller = controllerName;
                            cm.Des = d.Description;
                            icdiction[cd.Description].Add(cm);
                        }
                    }

                }
            }

            return icdiction;
        }
    }

    public struct ControllerModel
    {
        /// <summary>
        /// Action备注
        /// </summary>
        public string Des { get; set; }
        /// <summary>
        /// Action所属Controller
        /// </summary>
        public string Controller { get; set; }
        /// <summary>
        /// Action值
        /// </summary>
        public string Action { get; set; }
    }
}
