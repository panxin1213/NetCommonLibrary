using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Xml.Linq;

namespace WeiXinManage.Model
{
    public class HongBaoModel
    {

        [Required]
        /// <summary>
        /// 公众账号ID
        /// </summary>
        public string wxappid { get; set; }


        //private string _timeStamp = DateTime.Now.DateTicks().ToSafeString();

        ///// <summary>
        ///// 时间戳
        ///// </summary>
        //public string timeStamp
        //{
        //    get
        //    {
        //        return _timeStamp;
        //    }
        //}


        private string _nonceStr = Membership.GeneratePassword(32, 1);



        [Required]
        /// <summary>
        /// 随机字符串
        /// </summary>
        public string nonce_str
        {
            get
            {
                return _nonceStr;
            }
        }


        [Required]
        /// <summary>
        /// 商户订单号
        /// </summary>
        public string mch_billno
        {
            get;
            set;
        }


        [Required]
        /// <summary>
        /// 商户名称
        /// </summary>
        public string send_name
        {
            get;
            set;
        }


        [Required]
        /// <summary>
        /// 用户openid
        /// </summary>
        public string re_openid
        {
            get;
            set;
        }


        [Required]
        /// <summary>
        /// 付款金额
        /// </summary>
        public int total_amount
        {
            get;
            set;
        }


        [Required]
        /// <summary>
        /// 红包发放总人数
        /// </summary>
        public int total_num
        {
            get;
            set;
        }


        [Required]
        /// <summary>
        /// 红包祝福语
        /// </summary>
        public string wishing
        {
            get;
            set;
        }


        [Required]
        /// <summary>
        /// Ip地址
        /// </summary>
        public string client_ip
        {
            get;
            set;
        }


        [Required]
        /// <summary>
        /// 活动名称
        /// </summary>
        public string act_name
        {
            get;
            set;
        }


        [Required]
        /// <summary>
        /// 备注
        /// </summary>
        public string remark
        {
            get;
            set;
        }


        [Required]
        /// <summary>
        /// 场景id
        /// </summary>
        public string scene_id
        {
            get;
            set;
        }


        [Required]
        /// <summary>
        /// 商户号
        /// </summary>
        public string mch_id
        {
            get;
            set;
        }

        /// <summary>
        /// 返回xml格式提交字符串
        /// </summary>
        /// <param name="key">商户key</param>
        /// <param name="err"></param>
        /// <returns></returns>
        public string GetXmlString(string key, out string err)
        {
            err = "";

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
