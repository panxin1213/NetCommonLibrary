using Firm.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Firm.Core.Admin.SearchModel
{
    public class Admin_Record_SearchModel : SearchModel<D_Admin_Record, D_Admin, D_Admin_Record>
    {
        public string type { get; set; }

        public string key { get; set; }

        public string tablename { get; set; }

        /// <summary>
        /// 更新时间段，开始
        /// </summary>
        private DateTime? _starttime;
        public DateTime? starttime
        {
            get
            {
                if (_starttime != null)
                {
                    return _starttime.ToDateTime(DateTime.Now).Date;
                }

                return null;
            }
            set
            {
                _starttime = value;
            }
        }

        /// <summary>
        /// 更新时间段，结束
        /// </summary>
        private DateTime? _endtime;
        public DateTime? endtime
        {
            get
            {
                if (_endtime != null)
                {
                    return _endtime.ToDateTime(DateTime.Now).AddDays(1).Date.AddSeconds(-1);
                }

                return null;
            }
            set
            {
                _endtime = value;
            }
        }

        public Admin_Record_SearchModel()
            : base("select * from D_Admin_Record a left join D_Admin b on a.F_A_Record_AdminId=b.F_Admin_Id where 1=1"
            , "F_Admin_Id", (a, b) =>
            {
                a.D_Admin = b;
                return a;
            })
        {
        }



        public override void BulildSql()
        {
            SqlBuilder.AppendIfHasValue(type, " and F_A_Record_Type=@type ");
            SqlBuilder.AppendIfHasValue(key, " and F_Admin_Name=@key ");
            SqlBuilder.AppendIfHasValue(tablename, " and F_A_Record_TableName=@tablename ");

            SqlBuilder.AppendIfHasValue(starttime, " and F_A_Record_Create>@starttime ");
            SqlBuilder.AppendIfHasValue(endtime, " and F_A_Record_Create<@endtime");
        }


        public override string GetOrderBy()
        {
            return " order by F_A_Record_Create desc ";
        }
    }
}
