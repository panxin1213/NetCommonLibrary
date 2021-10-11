using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinManage.Model
{
    public class BaseModel
    {
        public void LoadParam(Dictionary<string, object> d, out string error)
        {
            error = "";
            if (d == null)
            {
                error = "字典为空";
                return;
            }

            try
            {
                var type = this.GetType();

                //遍历当前模型，给基础字段赋值
                foreach (var item in type.GetProperties())
                {
                    if (item.PropertyType.IsBaseType() && item.GetSetMethod() != null)
                    {
                        if (d.ContainsKey(item.Name))
                        {
                            item.SetValue(this, d[item.Name], null);
                        }
                    }
                    else
                    {
                        if (item.PropertyType.IsArray && d.ContainsKey(item.Name))
                        {
                            if (item.PropertyType.ToSafeString().Equals("System.String[]"))
                            {
                                var o = d[item.Name] as object[];

                                if (o != null)
                                {
                                    item.SetValue(this, o.Select(a => a.ToSafeString()).ToArray(), null);
                                }

                            }
                        }
                        else if (item.PropertyType.BaseType == typeof(BaseModel) && d.ContainsKey(item.Name))
                        {
                            var m = Activator.CreateInstance(item.PropertyType) as BaseModel;
                            m.LoadParam(d[item.Name] as Dictionary<string, object>, out error);
                            item.SetValue(this, m, null);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                error = e.Message;
                return;
            }
        }
    }
}
