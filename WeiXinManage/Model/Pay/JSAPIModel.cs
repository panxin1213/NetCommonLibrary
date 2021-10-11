using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace WeiXinManage.Model
{
    /// <summary>
    /// h5调用支付所需字段模型
    /// </summary>
    public class JSAPIModel
    {
        /// <summary>
        /// 公众账号ID
        /// </summary>
        public string appId { get; set; }


        private string _timeStamp = DateTime.Now.DateTicks().ToSafeString();

        /// <summary>
        /// 时间戳
        /// </summary>
        public string timeStamp
        {
            get
            {
                return _timeStamp;
            }
        }


        private string _nonceStr = Membership.GeneratePassword(32, 1);
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonceStr
        {
            get
            {
                return _nonceStr;
            }
        }

        /// <summary>
        /// 订单详情扩展字符串
        /// </summary>
        public string package
        {
            get;
            set;
        }

        /// <summary>
        /// 签名方式
        /// </summary>
        public string signType
        {
            get
            {
                return "MD5";
            }
        }

        /// <summary>
        /// 商户密钥
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 签名方式
        /// </summary>
        public string paySign
        {
            get
            {
                var properties = this.GetType().GetProperties();

                var emptyfields = new List<string>();

                var d = new Dictionary<string, string>();

                foreach (var property in properties)
                {
                    if (new[] { "PaySign" }.Any(a => a.Equals(property.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        continue;
                    }

                    var v = property.GetValue(this, null);
                    if (String.IsNullOrWhiteSpace(v.ToSafeString()))
                    {
                        emptyfields.Add(property.Name);
                    }
                    if (!new[] { "Key" }.Any(a => a.Equals(property.Name, StringComparison.OrdinalIgnoreCase)) && !String.IsNullOrWhiteSpace(v.ToSafeString()))
                    {
                        d.Add(property.Name, v.ToSafeString());
                    }
                }

                if (emptyfields.Any())
                {
                    return "(" + string.Join(",", emptyfields) + ")不能为空";
                }

                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(string.Join("&", d.OrderBy(a => a.Key).Select(a => a.Key + "=" + a.Value)) + "&key=" + Key, "MD5").ToUpper();
            }
        }
    }
}
