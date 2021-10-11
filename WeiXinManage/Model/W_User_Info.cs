using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinManage.Model
{
    [Serializable]
    public class W_User_Info
    {
        public int Subscribe { get; set; }

        public string Openid { get; set; }

        public string NickName { get; set; }

        public int Sex { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Province { get; set; }

        public string Language { get; set; }

        public string HeadImgurl { get; set; }

        public long Subscribe_time { get; set; }

        /// <summary>
        /// auth授权获取，用户唯一标识
        /// </summary>
        public string Unionid { get; set; }

        /// <summary>
        /// 用户特权信息，json 数组，如微信沃卡用户为（chinaunicom）
        /// </summary>
        public object[] Privilege { get; set; }

        /// <summary>
        /// 返回内容
        /// </summary>
        public string ReturnString { get; set; }


        public string Card_Code { get; set; }

        public string Balance { get; set; }

        public string Bonus { get; set; }

        /// <summary>
        /// JSon字符串转换为W_User_Info
        /// </summary>
        /// <param name="jsonstring"></param>
        /// <returns></returns>
        public static W_User_Info Parse(string jsonstring)
        {

            var m = new W_User_Info();

            try
            {
                var d = jsonstring.JsonToDictionary();

                foreach (var item in m.GetType().GetProperties())
                {
                    var v = d.SingleOrDefault(a => a.Key.Equals(item.Name, StringComparison.OrdinalIgnoreCase)).Value;
                    item.SetValue(m, v, null);
                }

                if (String.IsNullOrEmpty(m.Openid))
                {
                    m = null;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return m;
        }
    }
}
