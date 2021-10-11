using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Model
{
    [MetadataType(typeof(MetaData))]
    public partial class T_Image_Record
    {
        public class MetaData
        {
            [Display(Name = "编号")]
            public int F_Image_Record_Id { get; set; }

            [Display(Name = "文件路径")]
            public string F_Image_Record_FilePath { get; set; }

            [Display(Name = "使用次数")]
            public int F_Image_Record_Num { get; set; }

            [Display(Name = "使用所在")]
            public string F_Image_Record_InRecord { get; set; }

            [Display(Name = "Md5记录")]
            public string F_Image_Record_Md5 { get; set; }
        }
    }
}
