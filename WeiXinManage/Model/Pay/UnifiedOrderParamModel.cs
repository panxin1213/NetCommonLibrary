using ChinaBM.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Xml.Linq;

namespace WeiXinManage.Model
{
    public class UnifiedOrderParamModel
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

        /// <summary>
        /// 设备号，不必填
        /// </summary>
        public string Device_Info { get; set; }


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

        [Required]
        /// <summary>
        /// 商品描述 String 128
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 商品详情 String  6000  不必填
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// 附加数据 不必填 String 127
        /// </summary>
        public string Attach { get; set; }

        private string _out_trade_no = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random(Guid.NewGuid().GetHashCode()).Next(10000, 99999) + new Random(Guid.NewGuid().GetHashCode()).Next(10000, 99999) + new Random(Guid.NewGuid().GetHashCode()).Next(10000, 99999);
        /// <summary>
        /// 商户订单号 
        /// </summary>
        public string Out_trade_no
        {
            get
            {
                return _out_trade_no;
            }
        }

        /// <summary>
        /// 标价金额   单位为分
        /// </summary>
        public int Total_fee { get; set; }

        /// <summary>
        /// 终端IP
        /// </summary>
        public string Spbill_create_ip
        {
            get
            {
                return HttpKit.CurrentRequestIP;
            }
        }

        /// <summary>
        /// 交易起始时间	不必填 20091227091010
        /// </summary>
        public string Time_start { get; set; }

        /// <summary>
        /// 交易结束时间	不必填 20091227091010
        /// </summary>
        public string Time_expire { get; set; }

        /// <summary>
        /// 订单优惠标记    不必填
        /// </summary>
        public string Goods_tag { get; set; }

        [Required]
        /// <summary>
        /// 通知地址
        /// </summary>
        public string Notify_url { get; set; }

        [Required]
        /// <summary>
        /// 交易类型     JSAPI，NATIVE，APP
        /// </summary>
        public string Trade_type { get; set; }

        /// <summary>
        /// 商品ID    不必填
        /// </summary>
        public string Product_id { get; set; }

        /// <summary>
        /// 指定支付方式   不必填   上传此参数no_credit--可限制用户不能使用信用卡支付
        /// </summary>
        public string Limit_pay { get; set; }

        /// <summary>
        /// 用户标识     不必填
        /// </summary>
        public string Openid { get; set; }

        /// <summary>
        /// 场景信息     不必填
        /// </summary>
        public string scene_info { get; set; }



        /// <summary>
        /// 返回xml格式提交字符串
        /// </summary>
        /// <param name="key">商户key</param>
        /// <param name="err"></param>
        /// <returns></returns>
        public string GetXmlString(string key, out string err)
        {
            err = "";

            if (Total_fee <= 0)
            {
                err = "金额必须大于0";
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
