using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Common.Library;

namespace Admin.Model
{
    [MetadataType(typeof(MetaData))]
    public partial class T_Robot
    {
        public T_Robot()
        {
            F_Robot_Create = DateTime.Now;
            F_Robot_Update = DateTime.Now;
        }

        public class MetaData
        {
            [Display(Name = "编号")]
            public int F_Robot_Id { get; set; }

            [Display(Name = "名称")]
            public string F_Robot_Name { get; set; }

            [Display(Name = "类型")]
            [DataType("RobotType")]
            public string F_Robot_Type { get; set; }

            [Display(Name = "PC跳转地址")]
            [RegularExpression(Constants.REGEX_URL_ADDRESS, ErrorMessage = "地址格式错误")]
            public string F_Robot_Link { get; set; }

            public System.DateTime F_Robot_Create { get; set; }

            public System.DateTime F_Robot_Update { get; set; }

            [DataType("ManageIsLock")]
            public bool F_Robot_IsLock { get; set; }

            [Display(Name = "移动跳转地址")]
            [RegularExpression(Constants.REGEX_URL_ADDRESS, ErrorMessage = "地址格式错误")]
            public string F_Robot_MobileLink { get; set; }
        }
    }
}
