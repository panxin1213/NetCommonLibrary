using Common.Library;
using Core.Common.Service;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Upload;
using Dapper;
using Dapper.Contrib.Extensions;
using Admin.Model;

namespace Core.Admin.Upload
{
    public class RecordUploadConfig : UploadConfig
    {
        public override void UploadCallBack(System.Web.HttpPostedFileBase file, ref UploadMessage msg)
        {
            var md5 = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(file.FileName + file.ContentLength, "MD5").ToLower();

            using (IDbConnection db = Conn.Get())
            {
                var _irsv = new Image_Record_Service(db);

                var irm = _irsv.GetByMd5(md5);

                if (irm != null)
                {
                    msg.FilePath = irm.F_Image_Record_FilePath;
                }
                else
                {
                    irm = new T_Image_Record { F_Image_Record_FilePath = msg.FilePath, F_Image_Record_InRecord = "", F_Image_Record_Md5 = md5, F_Image_Record_Num = 0 };
                    _irsv.Insert(irm);
                }
            }
        }
    }
}
