using Common.Library.Log;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Xml.Linq;

namespace WeiXinManage.Model
{
    /// <summary>
    /// 查询订单模型
    /// </summary>
    public class OrderQueryModel
    {
        [Required]
        /// <summary>
        /// 公众账号ID
        /// </summary>
        public string AppId { get; set; }



        [Required]
        /// <summary>
        /// 商户号
        /// </summary>
        public string Mch_Id { get; set; }


        private string nonce_str = Membership.GeneratePassword(32, 1);
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string Nonce_Str
        {
            get
            {
                return nonce_str;
            }
        }

        /// <summary>
        /// 微信订单号
        /// </summary>
        public string Transaction_id { get; set; }


        /// <summary>
        /// 商户订单号
        /// </summary>
        public string Out_trade_no { get; set; }



        /// <summary>
        /// 返回xml格式提交字符串
        /// </summary>
        /// <param name="key">商户key</param>
        /// <param name="err"></param>
        /// <returns></returns>
        public string GetXmlString(string key, out string err)
        {
            err = "";

            if (String.IsNullOrWhiteSpace(Transaction_id) && String.IsNullOrWhiteSpace(Out_trade_no))
            {
                err = "微信订单号和商户订单号必须传一个";
                return "";
            }

            var properties = this.GetType().GetProperties();

            var emptyfields = new List<string>();

            var d = new Dictionary<string, string>();

            foreach (var property in properties)
            {
                var v = property.GetValue(this, null);
                var attrs = property.GetCustomAttributes(typeof(RequiredAttribute), false);
                if (attrs != null && attrs.Any())
                {
                    if (v == null)
                    {
                        emptyfields.Add(property.Name);
                    }
                }
                if (!String.IsNullOrWhiteSpace(v.ToSafeString()))
                {
                    d.Add(property.Name.ToLower(), v.ToSafeString());
                }
            }

            if (emptyfields.Any())
            {
                err = "(" + string.Join(",", emptyfields) + ")不能为空";
                return "";
            }

            //Logger.Error("", "str:" + (string.Join("&", d.OrderBy(a => a.Key).Select(a => a.Key + "=" + a.Value)) + "&key=" + key), null);

            var sign = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(string.Join("&", d.OrderBy(a => a.Key).Select(a => a.Key + "=" + a.Value)) + "&key=" + key, "MD5").ToUpper();
            d.Add("sign", sign);

            var document = new XElement("xml");

            foreach (var item in d)
            {
                document.Add(new XElement(item.Key, item.Value));
            }

            return document.ToString();
        }
    }
}
