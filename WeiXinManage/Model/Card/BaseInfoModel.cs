using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WeiXinManage.Model.Card
{
    public class BaseInfoModel : BaseModel
    {
        /// <summary>
        /// 卡劵id
        /// </summary>
        [Required]
        public string id { get; set; }

        /// <summary>
        /// 卡劵商户Logo
        /// </summary>
        [Required]
        public string logo_url { get; set; }


        [Required]
        /// <summary>
        /// Code展示类型
        /// </summary>
        public CodeTypeEnum code_type { get; set; }

        /// <summary>
        /// 支付功能结构体，swipe_card结构
        /// </summary>
        public PayInfoModel pay_info { get; set; }

        /// <summary>
        /// 是否设置该会员卡中部的按钮同时支持微信支付刷卡和会员卡二维码
        /// </summary>
        public bool is_pay_and_qrcode { get; set; }

        /// <summary>
        /// 商户名字,字数上限为12个汉字。
        /// </summary>
        [Required]
        public string brand_name { get; set; }

        [Required]
        /// <summary>
        /// 卡券名，字数上限为9个汉字
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 券颜色。按色彩规范标注填写Color010-Color100 
        /// </summary>
        public string color
        {
            get;
            set;
        }

        [Required]
        /// <summary>
        /// 卡券使用提醒，字数上限为16个汉字。   (使用时向服务员出示此券)
        /// </summary>
        public string notice { get; set; }

        [Required]
        /// <summary>
        /// 卡券使用说明，字数上限为1024个汉字。
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 商品信息。      
        /// </summary>
        [Required]
        public SkuModel sku { get; set; }

        /// <summary>
        /// 使用日期，有效期的信息。
        /// </summary>
        [Required]
        public DataInfoModel date_info { get; set; }

        /// <summary>
        /// 门店位置ID。
        /// </summary>
        public string[] location_id_list { get; set; }

        /// <summary>
        /// 支持全部门店，填写true或false，与location_id_list互斥
        /// </summary>
        public bool use_all_locations
        {
            get;
            set;
        }

        /// <summary>
        /// 是否自定义Code码。填写true或false，默认为false。
        /// </summary>
        public bool use_custom_code { get; set; }

        /// <summary>
        /// 卡券是否可转赠，填写true或false,true代表可转赠默认为true。
        /// </summary>
        public bool can_share { get; set; }

        /// <summary>
        /// 是否指定用户领取，填写true或false。默认为否。
        /// </summary>
        public bool bind_openid { get; set; }

        /// <summary>
        /// 客服电话  
        /// </summary>
        public string service_phone { get; set; }

        /// <summary>
        /// 第三方来源名，例如同程旅游、大众点评。
        /// </summary>
        public string source { get; set; }


        #region 子类


        /// <summary>
        /// 支付功能结构体，swipe_card结构
        /// </summary>
        public class PayInfoModel : BaseModel
        {
            public SwipeCard swipe_card { get; set; }



            public class SwipeCard : BaseModel
            {
                public bool is_swipe_card { get; set; }
            }
        }

        /// <summary>
        /// 商品信息。        
        /// </summary>
        public class SkuModel : BaseModel
        {
            /// <summary>
            /// 卡券库存的数量，不支持填写0，上限为100000000。
            /// </summary>
            public int quantity { get; set; }

            /// <summary>
            /// 卡券全部库存的数量，上限为100000000。
            /// </summary>
            public int total_quantity { get; set; }
        }

        /// <summary>
        /// 使用日期，有效期的信息。
        /// </summary>
        public class DataInfoModel : BaseModel
        {
            /// <summary>
            /// 使用时间的类型
            /// DATE_TYPE_FIX_TIME_RANGE 表示固定日期区间，DATE_TYPE_FIX_TERM表示固定时长（自领取后按天算），DATE_TYPE_PERMANENT 表示永久有效（会员卡类型专用）。
            /// </summary>
            public string type
            {
                get;
                set;
            }

            /// <summary>
            /// type为DATE_TYPE_FIX_TIME_RANGE时专用
            /// ，表示起用时间。从1970年1月1日00:00:00至起用时间的秒数，最终需转换为字符串形态传入，下同。（单位为秒）
            /// </summary>
            public int? begin_timestamp { get; set; }

            /// <summary>
            /// type为DATE_TYPE_FIX_TIME_RANGE时专用
            /// ，表示结束时间。（单位为秒）
            /// </summary>
            public int? end_timestamp { get; set; }

            /// <summary>
            /// type为DATE_TYPE_FIX_TERM时专用
            /// ，表示自领取后多少天内有效，领取后当天有效填写0。
            /// （单位为天）
            /// </summary>
            public int? fixed_term { get; set; }

            /// <summary>
            /// type为DATE_TYPE_FIX_TERM时专用
            /// ，表示自领取后多少天开始生效。（单位为天）
            /// </summary>
            public int? fixed_begin_term { get; set; }
        }


        public enum CodeTypeEnum
        {
            /// <summary>
            /// 文本
            /// </summary>
            CODE_TYPE_TEXT,
            /// <summary>
            /// 一维码
            /// </summary>
            CODE_TYPE_BARCODE,
            /// <summary>
            /// 二维码
            /// </summary>
            CODE_TYPE_QRCODE,
            /// <summary>
            /// 仅显示二维码 
            /// </summary>
            CODE_TYPE_ONLY_QRCODE,
            /// <summary>
            /// 仅显示一维码
            /// </summary>
            CODE_TYPE_ONLY_BARCODE,
            /// <summary>
            /// 不显示任何码型
            /// </summary>
            CODE_TYPE_NONE

        }

        #endregion
    }
}
