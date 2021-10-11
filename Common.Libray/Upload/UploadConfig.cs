using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Globalization;
using System.Linq.Expressions;
using ChinaBM.Common;

namespace System.Web.Upload
{
    /// <summary>
    /// 
    /// </summary>
    public enum Enum_Upload_AllowType
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 
        /// </summary>
        Image = 1,
        /// <summary>
        /// 
        /// </summary>
        Doc = 2,
        /// <summary>
        /// 
        /// </summary>
        File = 4,
        /// <summary>
        /// 
        /// </summary>
        App = 8,
        /// <summary>
        /// 
        /// </summary>
        Office = 16,
        /// <summary>
        /// 
        /// </summary>
        All = Image | Doc | File | App
    }

    /// <summary>
    /// 设置
    /// </summary>
    public class UploadConfig
    {
        /// <summary>
        /// 自定义验证，可以调用一些外部sevice验证权限啥的 如果返回非null字符。表示验证不通过
        /// </summary>
        public Func<string> Validate = null;

        private string _bottonText = "上传图片";
        /// <summary>
        /// 上传按钮文字
        /// </summary>
        public string ButtonText
        {
            get
            {
                return _bottonText;
            }
            set
            {
                _bottonText = value;
            }
        }
        /// <summary>
        /// 是否多文件上传
        /// </summary>
        public bool MultipleUpload { get; set; }
        /// <summary>
        /// 格式化路径
        /// 支持的格式
        /// {y}=4位年
        /// {w}=第w周
        /// {m}=2位月份
        /// {d}=2位日期
        /// {h}=2位小时
        /// {mi}=2位分钟
        /// {s}=2位秒数
        /// {mid}=用户id
        /// {r}=随机8位字符 (Guid.NewGuid().GetHashCode()).ToString("x"))
        /// {guid}=GUID  Guid.NewGuid().ToString("n");
        /// 例:{y}/{m}/{d}/{h}{mi}{s}_{r}_{guid}
        /// :{y}/{w}/{h}{mi}{s}_{r} //上传不频繁
        /// </summary>
        public string PathTemplate { get; set; }

        public string Mid { get; set; }


        /// <summary>
        /// 上传类型
        /// </summary>
        public Enum_Upload_AllowType AllowType { get; set; }

        private int _maxLength = 1000;
        /// <summary>
        /// 最大kb数
        /// </summary>
        public int MaxLength
        {
            get { return _maxLength; }
            set { _maxLength = value; }
        }
        /// <summary>
        /// 生成一个格式化后的路径
        /// </summary>
        /// <returns></returns>
        public virtual string GetPath()
        {
            return FormatPath(PathTemplate).Replace("{mid}", Mid ?? "");
        }

        /// <summary>
        /// 上传目录
        /// </summary>
        public static readonly string UploadPath = ConfigurationManager.AppSettings["UploadPath"] ?? "/up";

        /// <summary>
        /// 临时目录s
        /// </summary>
        public static readonly string UploadTempPath = UploadPath + "/Temp";

        /// <summary>
        /// 允许上传的类型
        /// </summary>
        public static readonly IDictionary<Enum_Upload_AllowType, string[]> AllowTypes = new Dictionary<Enum_Upload_AllowType, string[]>() {
            {Enum_Upload_AllowType.Image,".jpg|.gif|.jpeg|.png".Split('|')},
            {Enum_Upload_AllowType.Doc,".doc|.docx".Split('|')},
            {Enum_Upload_AllowType.File,".rar|.zip|.doc|.docx|.xls|.xlsx|.txt|.jpg|.gif|.jpeg|.png|.pdf|.ppt|.pptx".Split('|')},
            {Enum_Upload_AllowType.Office,".pdf|.doc|.docx|.xls|.xlsx|.txt|.ppt|.pptx".Split('|')},
            //{Enum_Upload_AllowType.App,"".Split('|')}
        };
        /// <summary>
        /// 按Enum_Upload_AllowType取支持上传的文件
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static IDictionary<string, bool> GetAllowFileExt(Enum_Upload_AllowType types)
        {
            var r = new List<string>();
            foreach (var ss in AllowTypes)
            {
                if (types.HasFlag(ss.Key))
                {
                    r.AddRange(ss.Value);
                }
            }
            return r.Distinct().ToDictionary(a => a, a => true);
        }

        /// <summary>
        /// 格式化上传路径
        /// </summary>
        /// <param name="t">
        /// 支持的格式
        /// {y}=4位年
        /// {w}=第w周
        /// {m}=2位月份
        /// {d}=2位日期
        /// {h}=2位小时
        /// {mi}=2位分钟
        /// {s}=2位秒数
        /// {mid}=用户id
        /// {r}=随机8位字符 (Guid.NewGuid().GetHashCode()).ToString("x"))
        /// {guid}=GUID  Guid.NewGuid().ToString("n");
        /// 例:{y}/{m}/{d}/{h}{mi}{s}_{r}_{guid}
        /// :{y}/{w}/{h}{mi}{s}_{r} //上传不频繁
        /// </param>
        /// <returns></returns>
        public static string FormatPath(string t)
        {
            DateTime now = DateTime.Now;
            Guid guid = Guid.NewGuid();

            System.Globalization.GregorianCalendar gc = new GregorianCalendar();
            string week = gc.GetWeekOfYear(now, CalendarWeekRule.FirstDay, DayOfWeek.Monday).ToString();

            t = t.Replace("{y}", now.Year.ToString())
                .Replace("{m}", now.Month.ToString("00"))
                .Replace("{d}", now.Day.ToString("00"))
                .Replace("{h}", now.Hour.ToString("00"))
                .Replace("{mi}", now.Minute.ToString("00"))
                .Replace("{s}", now.Second.ToString("00"))
                .Replace("{w}", week)
                .Replace("{r}", guid.GetHashCode().ToString("x"))
                .Replace("{guid}", guid.ToString("n"));
            return t;
        }

        /// <summary>
        /// 附件上传完后调用方法
        /// </summary>
        public virtual void UploadCallBack(HttpPostedFileBase file, ref UploadMessage msg)
        {
        }


        public static bool UploadFile(HttpPostedFileBase Filedata, UploadConfig config, out UploadMessage m)
        {
            m = Filedata.SaveFile(config);
            return true;
        }
    }
    /// <summary>
    /// 上传后的消息
    /// </summary>
    public class UploadMessage
    {
        /// <summary>
        /// 错误
        /// </summary>
        public IList<string> Errors;
        /// <summary>
        /// 原先的文件名
        /// </summary>
        public string OriginalFileName { get; set; }
        /// <summary>
        /// 上传后文件路径
        /// </summary>
        public string FilePath { get; set; }
    }



}
