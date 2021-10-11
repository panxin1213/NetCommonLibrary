using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WeiXinManage.Model.Card
{
    public class CardMainModel : BaseModel
    {
        /// <summary>
        /// 卡券类型。
        //  团购券：GROUPON; 折扣券：DISCOUNT; 礼品券：GIFT; 
        //  代金券：CASH; 通用券：GENERAL_COUPON; 
        //  会员卡：MEMBER_CARD; 景点门票：SCENIC_TICKET；
        //  电影票：MOVIE_TICKET； 飞机票：BOARDING_PASS； 
        //  会议门票：MEETING_TICKET； 汽车票：BUS_TICKET;
        /// </summary>
        public string card_type
        {
            get;
            set;
        }

        /// <summary>
        /// 团购券专用字段，团购详情。
        /// </summary>
        public string deal_detail { get; set; }

        /// <summary>
        /// 礼品券专用，表示礼品名字。
        /// </summary>
        public string gift { get; set; }

        /// <summary>
        /// least_cost字段为代金券专用，表示起用金额（单位为分）。
        /// </summary>
        public int? least_cost { get; set; }

        /// <summary>
        /// 代金券专用，表示减免金额（单位为分）
        /// </summary>
        public int? reduce_cost { get; set; }

        /// <summary>
        /// 折扣券专用字段，表示打折额度（百分比），例：填30为七折团购详情。
        /// </summary>
        public int? discount { get; set; }


        /// <summary>
        /// 基础信息模型
        /// </summary>
        public BaseInfoModel base_info { get; set; }


        public string ReturnString { get; set; }
    }

    public enum CardType
    {
        /// <summary>
        /// 团购券
        /// </summary>
        GROUPON,
        /// <summary>
        /// 折扣券
        /// </summary>
        DISCOUNT,
        /// <summary>
        /// 礼品券
        /// </summary>
        GIFT,
        /// <summary>
        /// 代金券
        /// </summary>
        CASH,
        /// <summary>
        /// 通用券
        /// </summary>
        GENERAL_COUPON,
        /// <summary>
        /// 会员卡
        /// </summary>
        MEMBER_CARD,
        /// <summary>
        /// 景点门票
        /// </summary>
        SCENIC_TICKET,
        /// <summary>
        /// 电影票
        /// </summary>
        MOVIE_TICKET,
        /// <summary>
        /// 飞机票
        /// </summary>
        BOARDING_PASS,
        /// <summary>
        /// 会议门票
        /// </summary>
        MEETING_TICKET,
        /// <summary>
        /// 汽车票
        /// </summary>
        BUS_TICKET
    }
}
