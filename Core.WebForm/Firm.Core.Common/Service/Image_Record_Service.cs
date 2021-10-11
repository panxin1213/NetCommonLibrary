using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Firm.Model;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Text.RegularExpressions;
using Common.Library.Log;

namespace Firm.Core.Common.Service
{
    public class Image_Record_Service : ServiceBase<D_Image_Record>
    {
        public Image_Record_Service(IDbConnection db)
            : base(db)
        {

        }

        public D_Image_Record GetByMd5(string md5)
        {
            var sql = "select * from D_Image_Record where F_Image_Record_Md5=@md5";

            return db.Query<D_Image_Record>(sql, new { md5 }).SingleOrDefault();
        }

        /// <summary>
        /// 更新数量
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="num"></param>
        /// <param name="injson"></param>
        /// <returns></returns>
        public bool UpdateNum(string filepath, int num, List<string> injson, IDbTransaction trans = null)
        {
            if (num == 0)
            {
                return false;
            }

            var m = db.Query<D_Image_Record>("select * from D_Image_Record where F_Image_Record_FilePath=@filepath", new { filepath }, trans).SingleOrDefault();

            if (m == null)
            {
                return false;
            }

            m.F_Image_Record_Num += num;

            if (num > 0)
            {
                m.F_Image_Record_InRecord = (m.F_Image_Record_InRecord + "," + string.Join(",", injson)).Trim(',');
            }
            else
            {
                m.F_Image_Record_InRecord = string.Join(",", m.F_Image_Record_InRecord.Split(',').Where(a => !injson.Any(b => a.Equals(b, StringComparison.OrdinalIgnoreCase)))); 
            }

            return db.Update(m, trans);


        }

        /// <summary>
        /// 更新数量，自动检索对象中绝对路径文件
        /// </summary>
        /// <param name="m"></param>
        /// <param name="injson"></param>
        /// <returns></returns>
        public void UpdateNumByObject(object m, string injson, bool isadd, IDbTransaction trans = null)
        {
            if (m == null)
            {
                return;
            }

            var istrans = false;

            if (trans == null)
            {
                trans = db.BeginTransaction();
                istrans = true;
            }

            try
            {
                var regex = new Regex(@"(/(((?!(/|\.|""|\[|\])).)*)){1,}\.(jpg|gif|png|jpeg|doc|docx|xls|xlsx|txt|rar|zip)", RegexOptions.IgnoreCase);

                var num = isadd ? 1 : -1;
                
                var l = new List<string>();

                foreach (Match match in regex.Matches(m.ToJson()))
                {
                    if (!l.Any(a => a.Equals(match.Value, StringComparison.OrdinalIgnoreCase)))
                    {
                        l.Add(match.Value);
                        UpdateNum(match.Value, num, new List<string> { injson }, trans);
                    }
                }
            }
            catch (Exception e)
            {
                if (istrans)
                {
                    trans.Rollback();
                }
                Logger.Error(this, e.Message, e);
                throw e;
            }

            if (istrans)
            {
                trans.Commit();
            }

            return;
        }

    }
}
