using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WeiXinManage.Model
{
    /// <summary>
    /// 更新会员信息模型
    /// </summary>
    public class UpdateUserInfoModel
    {
        [Required]
        /// <summary>
        /// 卡券Code码。
        /// </summary>
        public string code { get; set; }

        [Required]
        /// <summary>
        /// 卡券ID。
        /// </summary>
        public string card_id { get; set; }

        /// <summary>
        /// 支持商家激活时针对单个会员卡分配自定义的会员卡背景。
        /// </summary>
        public string background_pic_url { get; set; }

        /// <summary>
        /// 需要设置的积分全量值，传入的数值会直接显示
        /// </summary>
        public int? bonus { get; set; }

        /// <summary>
        /// 本次积分变动值，传负数代表减少
        /// </summary>
        public int? add_bonus { get; set; }

        /// <summary>
        /// 商家自定义积分消耗记录，不超过14个汉字
        /// </summary>
        public string record_bonus { get; set; }

        /// <summary>
        /// 需要设置的余额全量值，传入的数值会直接显示在卡面,单位(分)
        /// </summary>
        public int? balance { get; set; }

        /// <summary>
        /// 本次余额变动值，传负数代表减少,单位(分)
        /// </summary>
        public int? add_balance { get; set; }

        /// <summary>
        /// 商家自定义金额消耗记录，不超过14个汉字。
        /// </summary>
        public string record_balance { get; set; }

        /// <summary>
        /// 创建时字段custom_field1定义类型的最新数值，限制为4个汉字，12字节。    
        /// </summary>
        public string custom_field_value1 { get; set; }

        /// <summary>
        /// 创建时字段custom_field2定义类型的最新数值，限制为4个汉字，12字节。 
        /// </summary>
        public string custom_field_value2 { get; set; }

        /// <summary>
        /// 创建时字段custom_field3定义类型的最新数值，限制为4个汉字，12字节。
        /// </summary>
        public string custom_field_value3 { get; set; }

        /// <summary>
        /// 控制原生消息结构体，包含各字段的消息控制字段
        /// </summary>
        public NotifyOptional notify_optional { get; set; }


        /// <summary>
        /// 控制原生消息结构体，包含各字段的消息控制字段
        /// </summary>
        public class NotifyOptional
        {
            /// <summary>
            /// 积分变动时是否触发系统模板消息，默认为true
            /// </summary>
            public bool is_notify_bonus { get; set; }

            /// <summary>
            /// 余额变动时是否触发系统模板消息，默认为true
            /// </summary>
            public bool is_notify_balance { get; set; }

            /// <summary>
            /// 自定义group1变动时是否触发系统模板消息，默认为false。（2、3同理）
            /// </summary>
            public bool is_notify_custom_field1 { get; set; }
        }

        /// <summary>
        /// 返回json结构数据
        /// </summary>
        /// <param name="err"></param>
        /// <returns></returns>
        public string ToJson(out string err)
        {
            err = "";
            if (bonus < 0)
            {
                err = "总积分必须大于0";
                return "";
            }

            if (balance < 0)
            {
                err = "总金额必须大于0";
                return "";
            }

            var properties = this.GetType().GetProperties();

            var emptyfields = new List<string>();

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
            }

            if (emptyfields.Any())
            {
                err = "(" + string.Join(",", emptyfields) + ")不能为空";
                return "";
            }

            return this.ToJson();
        }
    }
}
