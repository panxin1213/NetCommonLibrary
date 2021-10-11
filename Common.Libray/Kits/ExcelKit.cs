#region 暂时全部注释，需要加载Excel.BLL才能使用
//using System;
//using System.Reflection;
//using System.Data;
//using System.Data.OleDb;
//using System.Collections.Generic;

//namespace ChinaBM.Common
//{
//    /// <summary>
//    /// ExcelClass 的摘要说明。
//    /// </summary>
//    public class ExcelClass
//    {
//        /// <summary>
//        /// 构建ExcelClass类
//        /// </summary>
//        public ExcelClass()
//        {
//            this.m_objExcel = new Excel.Application();
//        }
//        /// <summary>
//        /// 构建ExcelClass类
//        /// </summary>
//        /// <param name="objExcel">Excel.Application</param>
//        public ExcelClass(Excel.Application objExcel)
//        {
//            this.m_objExcel = objExcel;
//        }

//        /// <summary>
//        /// 列标号
//        /// </summary>
//        private string AList = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

//        /// <summary>
//        /// 获取描述区域的字符
//        /// </summary>
//        /// <param name="x"></param>
//        /// <param name="y"></param>
//        /// <returns></returns>
//        public string GetAix(int x, int y)
//        {
//            char[] AChars = AList.ToCharArray();
//            if (x >= 26) { return ""; }
//            string s = "";
//            s = s + AChars[x - 1].ToString();
//            s = s + y.ToString();
//            return s;
//        }

//        /// <summary>
//        /// 给单元格赋值1
//        /// </summary>
//        /// <param name="x">行号</param>
//        /// <param name="y">列号</param>
//        /// <param name="align">对齐（CENTER、LEFT、RIGHT）</param>
//        /// <param name="text">值</param>
//        public void setValue(int y, int x, string align, string text)
//        {
//            Excel.Range range = sheet.get_Range(this.GetAix(x, y), miss);
//            range.set_Value(miss, text);
//            if (align.ToUpper() == "CENTER")
//            {
//                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
//            }
//            if (align.ToUpper() == "LEFT")
//            {
//                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
//            }
//            if (align.ToUpper() == "RIGHT")
//            {
//                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
//            }
//        }

//        /// <summary>
//        /// 给单元格赋值2
//        /// </summary>
//        /// <param name="x">行号</param>
//        /// <param name="y">列号</param>
//        /// <param name="text">值</param>
//        public void setValue(int y, int x, string text)
//        {
//            Excel.Range range = sheet.get_Range(this.GetAix(x, y), miss);
//            range.set_Value(miss, text);
//        }

//        /// <summary>
//        /// 给单元格赋值3
//        /// </summary>
//        /// <param name="x">行号</param>
//        /// <param name="y">列号</param>
//        /// <param name="text">值</param>
//        /// <param name="font">字符格式</param>
//        /// <param name="color">颜色</param>
//        public void setValue(int y, int x, string text, System.Drawing.Font font, System.Drawing.Color color)
//        {
//            this.setValue(x, y, text);
//            Excel.Range range = sheet.get_Range(this.GetAix(x, y), miss);
//            range.Font.Size = font.Size;
//            range.Font.Bold = font.Bold;
//            range.Font.Color = color;
//            range.Font.Name = font.Name;
//            range.Font.Italic = font.Italic;
//            range.Font.Underline = font.Underline;
//        }

//        /// <summary>
//        /// 插入新行
//        /// </summary>
//        /// <param name="y">模板行号</param>
//        public void insertRow(int y)
//        {
//            Excel.Range range = sheet.get_Range(GetAix(1, y), GetAix(25, y));
//            range.Copy(miss);
//            range.Insert(Excel.XlDirection.xlDown, miss);
//            range.get_Range(GetAix(1, y), GetAix(25, y));
//            range.Select();
//            sheet.Paste(miss, miss);

//        }

//        /// <summary>
//        /// 把剪切内容粘贴到当前区域
//        /// </summary>
//        public void past()
//        {
//            string s = "a,b,c,d,e,f,g";
//            sheet.Paste(sheet.get_Range(this.GetAix(10, 10), miss), s);
//        }
//        /// <summary>
//        /// 设置边框
//        /// </summary>
//        /// <param name="x1"></param>
//        /// <param name="y1"></param>
//        /// <param name="x2"></param>
//        /// <param name="y2"></param>
//        /// <param name="Width"></param>
//        public void setBorder(int x1, int y1, int x2, int y2, int Width)
//        {
//            Excel.Range range = sheet.get_Range(this.GetAix(x1, y1), this.GetAix(x2, y2));
//            range.Borders.Weight = Width;
//        }
//        public void mergeCell(int x1, int y1, int x2, int y2)
//        {
//            Excel.Range range = sheet.get_Range(this.GetAix(x1, y1), this.GetAix(x2, y2));
//            range.Merge(true);
//        }

//        public Excel.Range getRange(int x1, int y1, int x2, int y2)
//        {
//            Excel.Range range = sheet.get_Range(this.GetAix(x1, y1), this.GetAix(x2, y2));
//            return range;
//        }

//        private object miss = Missing.Value; //忽略的参数OLENULL 
//        private Excel.Application m_objExcel;//Excel应用程序实例 
//        private Excel.Workbooks m_objBooks;//工作表集合 
//        private Excel.Workbook m_objBook;//当前操作的工作表 
//        private Excel.Worksheet sheet;//当前操作的表格 

//        public Excel.Worksheet CurrentSheet
//        {
//            get
//            {
//                return sheet;
//            }
//            set
//            {
//                this.sheet = value;
//            }
//        }

//        public Excel.Workbooks CurrentWorkBooks
//        {
//            get
//            {
//                return this.m_objBooks;
//            }
//            set
//            {
//                this.m_objBooks = value;
//            }
//        }

//        public Excel.Workbook CurrentWorkBook
//        {
//            get
//            {
//                return this.m_objBook;
//            }
//            set
//            {
//                this.m_objBook = value;
//            }
//        }

//        /// <summary>
//        /// 打开Excel文件
//        /// </summary>
//        /// <param name="filename">路径</param>
//        public void OpenExcelFile(string filename)
//        {
//            UserControl(false);

//            m_objExcel.Workbooks.Open(filename, miss, miss, miss, miss, miss, miss, miss,
//                                    miss, miss, miss, miss, miss, miss, miss);

//            m_objBooks = (Excel.Workbooks)m_objExcel.Workbooks;

//            m_objBook = m_objExcel.ActiveWorkbook;
//            sheet = (Excel.Worksheet)m_objBook.ActiveSheet;
//        }
//        public void UserControl(bool usercontrol)
//        {
//            if (m_objExcel == null) { return; }
//            m_objExcel.UserControl = usercontrol;
//            m_objExcel.DisplayAlerts = usercontrol;
//            m_objExcel.Visible = usercontrol;
//        }
//        public void CreateExceFile()
//        {
//            UserControl(false);
//            m_objBooks = (Excel.Workbooks)m_objExcel.Workbooks;
//            m_objBook = (Excel.Workbook)(m_objBooks.Add(miss));
//            sheet = (Excel.Worksheet)m_objBook.ActiveSheet;
//        }
//        public void SaveAs(string FileName)
//        {
//             m_objBook.SaveAs(FileName, miss, miss, miss, miss,
//             miss, Excel.XlSaveAsAccessMode.xlNoChange,
//             Excel.XlSaveConflictResolution.xlLocalSessionChanges,
//             miss, miss, miss, miss);
//            //m_objBook.Close(false, miss, miss); 
//        }
//        public void ReleaseExcel()
//        {
//            m_objExcel.Quit();
//            System.Runtime.InteropServices.Marshal.ReleaseComObject((object)m_objExcel);
//            System.Runtime.InteropServices.Marshal.ReleaseComObject((object)m_objBooks);
//            System.Runtime.InteropServices.Marshal.ReleaseComObject((object)m_objBook);
//            System.Runtime.InteropServices.Marshal.ReleaseComObject((object)sheet);
//            m_objExcel = null;
//            m_objBooks = null;
//            m_objBook = null;
//            sheet = null;
//            GC.Collect();
//        }

//        /////////////////////////////////
//        public bool KillAllExcelApp()
//        {
//            try
//            {
//                if (m_objExcel != null) // isRunning是判断xlApp是怎么启动的flag.
//                {
//                    m_objExcel.Quit();
//                    System.Runtime.InteropServices.Marshal.ReleaseComObject(m_objExcel);
//                    //释放COM组件，其实就是将其引用计数减1
//                    //System.Diagnostics.Process theProc;
//                    foreach (System.Diagnostics.Process theProc in System.Diagnostics.Process.GetProcessesByName("EXCEL"))
//                    {
//                        //先关闭图形窗口。如果关闭失败...有的时候在状态里看不到图形窗口的excel了，
//                        //但是在进程里仍然有EXCEL.EXE的进程存在，那么就需要杀掉它:p
//                        if (theProc.CloseMainWindow() == false)
//                        {
//                            theProc.Kill();
//                        }
//                    }
//                    m_objExcel = null;
//                    return true;
//                }
//            }
//            catch
//            {
//                return false;
//            }
//            return true;
//        }
//        /////////////////////////////////////////////


//        public static DataSet GetDataSet(string path)
//        {
//            // 读取Excel数据，填充DataSet
//            // 连接字符串            
//            string xlsPath = path;
//            string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" +
//                            "Extended Properties=\"Excel 8.0;HDR=No;IMEX=1\";" + // 指定扩展属性为 Microsoft Excel 8.0 (97) 9.0 (2000) 10.0 (2002)，并且第一行作为数据返回，且以文本方式读取
//                            "data source=" + xlsPath;
//            string sql_F = "SELECT * FROM [{0}]";

//            OleDbConnection conn = null;
//            OleDbDataAdapter da = null;
//            DataTable tblSchema = null;
//            IList<string> tblNames = null;

//            // 初始化连接，并打开
//            conn = new OleDbConnection(connStr);
//            conn.Open();

//            // 获取数据源的表定义元数据                        
//            //tblSchema = conn.GetSchema("Tables");
//            tblSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

//            //GridView1.DataSource = tblSchema;
//            //GridView1.DataBind();

//            // 关闭连接
//            //conn.Close();

//            tblNames = new List<string>();
//            foreach (DataRow row in tblSchema.Rows)
//            {
//                tblNames.Add((string)row["TABLE_NAME"]); // 读取表名
//            }

//            // 初始化适配器
//            da = new OleDbDataAdapter();
//            // 准备数据，导入DataSet
//            DataSet ds = new DataSet();

//            foreach (string tblName in tblNames)
//            {
//                da.SelectCommand = new OleDbCommand(String.Format(sql_F, tblName), conn);
//                try
//                {
//                    da.Fill(ds, tblName);
//                }
//                catch
//                {
//                    // 关闭连接
//                    if (conn.State == ConnectionState.Open)
//                    {
//                        conn.Close();
//                    }
//                    throw;
//                }
//            }

//            // 关闭连接
//            if (conn.State == ConnectionState.Open)
//            {
//                conn.Close();
//            }
//            return ds;
//        }
//    }
//}
#endregion


using System;
using System.Linq.Expressions;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
namespace ChinaBM.Common
{
    public static class ExcelTool
    {
        private static IEnumerable<string> getFields<T>(Expression<Func<T, object>>[] propertys)
        {
            foreach (var x in propertys)
            {
                var y = x.Body.RemoveConvert() as System.Linq.Expressions.MemberExpression;
                if (y == null)
                    throw new Exception("不支持的表达式 “" + x.Body + "”");
                yield return y.Member.Name;
            }
        }

        public static string GetExcelString<T>(IEnumerable<T> m, string encode, params string[] containPropertys) where T : class
        {
            if (m == null || m.Count() == 0)
            {
                return string.Empty;
            }

            var strb = new StringBuilder();

            strb.AppendLine(@"<html xmlns:v=""urn:schemas-microsoft-com:vml"" xmlns:o=""urn:schemas-microsoft-com:office:office"" xmlns:x=""urn:schemas-microsoft-com:office:excel"" xmlns=""http://www.w3.org/TR/REC-html40"">
 <head>
  <meta http-equiv=""Content-Type"" content=""text/html; charset=" + encode + @"""/>
  <meta name=""ProgId"" content=""Excel.Sheet""/>
  <meta name=""Generator"" content=""WPS Office ET""/>
  <!--[if gte mso 9]>
   <xml>
    <o:DocumentProperties>
     <o:Author>Administrator</o:Author>
     <o:Created>2014-02-26T10:53:23</o:Created>
     <o:LastSaved>2014-02-26T10:54:03</o:LastSaved>
    </o:DocumentProperties>
    <o:CustomDocumentProperties>
     <o:KSOProductBuildVer dt:dt=""string"">2052-9.1.0.4429</o:KSOProductBuildVer>
    </o:CustomDocumentProperties>
   </xml>
  <![endif]-->
  <style>
<!-- @page
	{margin:0.98in 0.75in 0.98in 0.75in;
	mso-header-margin:0.51in;
	mso-footer-margin:0.51in;}
tr
	{mso-height-source:auto;
	mso-ruby-visibility:none;}
col
	{mso-width-source:auto;
	mso-ruby-visibility:none;}
br
	{mso-data-placement:same-cell;}
.font0
	{color:windowtext;
	font-size:12.0pt;
	font-weight:400;
	font-style:normal;
	text-decoration:none;
	font-family:""宋体"";
	mso-generic-font-family:auto;
	mso-font-charset:134;}
.font1
	{color:#FFFFFF;
	font-size:10.0pt;
	font-weight:700;
	font-style:normal;
	text-decoration:none;
	font-family:""微软雅黑"";
	mso-generic-font-family:auto;
	mso-font-charset:134;}
.font2
	{color:#000000;
	font-size:12.0pt;
	font-weight:400;
	font-style:normal;
	text-decoration:none;
	font-family:""微软雅黑"";
	mso-generic-font-family:auto;
	mso-font-charset:134;}
.style0
	{mso-number-format:""General"";
	text-align:general;
	vertical-align:middle;
	white-space:nowrap;
	mso-rotate:0;
	mso-pattern:auto;
	mso-background-source:auto;
	color:windowtext;
	font-size:12.0pt;
	font-weight:400;
	font-style:normal;
	text-decoration:none;
	font-family:宋体;
	mso-generic-font-family:auto;
	mso-font-charset:134;
	border:none;
	mso-protection:locked visible;
	mso-style-name:""Normal"";
	mso-style-id:0;}
.style16
	{mso-number-format:""_ * \#\,\#\#0\.00_ \;_ * \\-\#\,\#\#0\.00_ \;_ * \0022-\0022??_ \;_ \@_ "";
	mso-style-name:""Comma"";
	mso-style-id:3;}
.style17
	{mso-number-format:""_ \0022\00A5\0022* \#\,\#\#0\.00_ \;_ \0022\00A5\0022* \\-\#\,\#\#0\.00_ \;_ \0022\00A5\0022* \0022-\0022??_ \;_ \@_ "";
	mso-style-name:""Currency"";
	mso-style-id:4;}
.style18
	{mso-number-format:""_ * \#\,\#\#0_ \;_ * \\-\#\,\#\#0_ \;_ * \0022-\0022_ \;_ \@_ "";
	mso-style-name:""Comma[0]"";
	mso-style-id:6;}
.style19
	{mso-number-format:""0%"";
	mso-style-name:""Percent"";
	mso-style-id:5;}
.style20
	{mso-number-format:""_ \0022\00A5\0022* \#\,\#\#0_ \;_ \0022\00A5\0022* \\-\#\,\#\#0_ \;_ \0022\00A5\0022* \0022-\0022_ \;_ \@_ "";
	mso-style-name:""Currency[0]"";
	mso-style-id:7;}
td
	{mso-style-parent:style0;
	padding-top:1px;
	padding-right:1px;
	padding-left:1px;
	mso-ignore:padding;
	mso-number-format:""General"";
	text-align:general;
	vertical-align:middle;
	white-space:nowrap;
	mso-rotate:0;
	mso-pattern:auto;
	mso-background-source:auto;
	color:windowtext;
	font-size:12.0pt;
	font-weight:400;
	font-style:normal;
	text-decoration:none;
	font-family:宋体;
	mso-generic-font-family:auto;
	mso-font-charset:134;
	border:none;
	mso-protection:locked visible;}
.xl22
	{mso-style-parent:style0;
	mso-pattern:auto none;
	background:#0000FF;
	color:#FFFFFF;
	font-size:10.0pt;
	font-weight:700;
	font-family:微软雅黑, sans-serif;}
.xl23
	{mso-style-parent:style0;
	text-align:right;
	color:#000000;
	font-family:微软雅黑, sans-serif;}
 -->  </style>
  <!--[if gte mso 9]>
   <xml>
    <x:ExcelWorkbook>
     <x:ExcelWorksheets>
      <x:ExcelWorksheet>
       <x:Name>Sheet1</x:Name>
       <x:WorksheetOptions>
        <x:DefaultRowHeight>285</x:DefaultRowHeight>
        <x:Selected/>
        <x:Panes>
         <x:Pane>
          <x:Number>3</x:Number>
          <x:ActiveCol>0</x:ActiveCol>
          <x:ActiveRow>0</x:ActiveRow>
          <x:RangeSelection>A1:D372</x:RangeSelection>
         </x:Pane>
        </x:Panes>
        <x:ProtectContents>False</x:ProtectContents>
        <x:ProtectObjects>False</x:ProtectObjects>
        <x:ProtectScenarios>False</x:ProtectScenarios>
        <x:PageBreakZoom>100</x:PageBreakZoom>
        <x:Print>
         <x:ValidPrinterInfo/>
         <x:PaperSizeIndex>9</x:PaperSizeIndex>
        </x:Print>
       </x:WorksheetOptions>
      </x:ExcelWorksheet>
      <x:ExcelWorksheet>
       <x:Name>Sheet2</x:Name>
       <x:WorksheetOptions>
        <x:DefaultRowHeight>285</x:DefaultRowHeight>
        <x:Panes>
         <x:Pane>
          <x:Number>3</x:Number>
          <x:ActiveCol>0</x:ActiveCol>
          <x:ActiveRow>0</x:ActiveRow>
         </x:Pane>
        </x:Panes>
        <x:ProtectContents>False</x:ProtectContents>
        <x:ProtectObjects>False</x:ProtectObjects>
        <x:ProtectScenarios>False</x:ProtectScenarios>
        <x:PageBreakZoom>100</x:PageBreakZoom>
        <x:Print>
         <x:ValidPrinterInfo/>
         <x:PaperSizeIndex>9</x:PaperSizeIndex>
        </x:Print>
       </x:WorksheetOptions>
      </x:ExcelWorksheet>
      <x:ExcelWorksheet>
       <x:Name>Sheet3</x:Name>
       <x:WorksheetOptions>
        <x:DefaultRowHeight>285</x:DefaultRowHeight>
        <x:Panes>
         <x:Pane>
          <x:Number>3</x:Number>
          <x:ActiveCol>0</x:ActiveCol>
          <x:ActiveRow>0</x:ActiveRow>
         </x:Pane>
        </x:Panes>
        <x:ProtectContents>False</x:ProtectContents>
        <x:ProtectObjects>False</x:ProtectObjects>
        <x:ProtectScenarios>False</x:ProtectScenarios>
        <x:PageBreakZoom>100</x:PageBreakZoom>
        <x:Print>
         <x:ValidPrinterInfo/>
         <x:PaperSizeIndex>9</x:PaperSizeIndex>
        </x:Print>
       </x:WorksheetOptions>
      </x:ExcelWorksheet>
     </x:ExcelWorksheets>
     <x:ProtectStructure>False</x:ProtectStructure>
     <x:ProtectWindows>False</x:ProtectWindows>
     <x:WindowHeight>9920</x:WindowHeight>
     <x:WindowWidth>23880</x:WindowWidth>
    </x:ExcelWorkbook>
   </xml>
  <![endif]-->
 </head>
 <body link=""blue"" vlink=""purple"">
  <table width=""672"" border=""0"" cellpadding=""0"" cellspacing=""0"" style='width:504.00pt;border-collapse:collapse;table-layout:fixed;'>
   <col width=""134"" style='mso-width-source:userset;mso-width-alt:4288;'/>
   <col width=""141"" style='mso-width-source:userset;mso-width-alt:4512;'/>
   <col width=""133"" style='mso-width-source:userset;mso-width-alt:4256;'/>
   <col width=""264"" style='mso-width-source:userset;mso-width-alt:8448;'/>
   <tr height=""22"" style='height:16.50pt;'>");

            var type = m.ElementAt(0).GetType();

            var properties = type.GetProperties().ToList();
            var pl = new List<PropertyInfo>();
            var cpl = new Dictionary<PropertyInfo, List<string>>();
            var cpt = new Dictionary<PropertyInfo, Type>();

            var ass = Assembly.Load("Base.Model");
            var assTypes = ass.GetExportedTypes();

            if (containPropertys != null && containPropertys.Length > 0)
            {
                foreach (var item in containPropertys)
                {

                    var p = properties.SingleOrDefault(a => a.Name.Equals(item, StringComparison.OrdinalIgnoreCase));
                    if (p != null)
                    {
                        pl.Add(p);
                    }
                    else if (item.Split('.').Length > 1)
                    {
                        var arrl = item.Split('.');
                        var navType = assTypes.SingleOrDefault(a => a.Name.Equals(arrl[arrl.Length - 2]));

                        if (navType != null)
                        {
                            var cp = navType.GetProperties().SingleOrDefault(a => a.Name.Equals(arrl[arrl.Length - 1]));
                            if (cp != null)
                            {
                                cpt.Add(cp, navType);
                                pl.Add(cp);
                                cpl.Add(cp, arrl.Take(arrl.Length - 1).ToList());
                            }
                        }
                    }
                }
                properties = pl;
            }

            if (properties.Count == 0)
            {
                return string.Empty;
            }

            foreach (var item in properties)
            {
                strb.AppendLine("<td class=\"xl22\" height=\"22\" width=\"134\" style='height:16.50pt;width:100.50pt;' x:str>" + item.GetDescription(cpt.ContainsKey(item) ? cpt[item] : type) + "</td>");
            }
            strb.AppendLine(" </tr>");

            foreach (var item in m)
            {
                strb.AppendLine(" <tr>");
                foreach (var pitem in properties)
                {
                    object o = null;

                    if (cpl.ContainsKey(pitem))
                    {
                        var thisType = type;
                        object to = item;
                        foreach (var titem in cpl[pitem])
                        {
                            var tp = thisType.GetProperty(titem);
                            if (tp != null)
                            {
                                to = tp.GetValue(to, null);
                            }
                            thisType = tp.PropertyType;
                            if (to == null)
                            {
                                break;
                            }
                        }
                        if (to != null)
                        {
                            o = pitem.GetValue(to, null);
                        }
                    }
                    else
                    {
                        o = pitem.GetValue(item, null);
                    }
                    strb.AppendLine("<td class='xl23' style='text-align:right;font-family:微软雅黑, sans-serif;' x:str>" + o + "</td>");
                }
                strb.AppendLine(" </tr>");
            }
            strb.AppendLine("</table> </body> </html>");


            return strb.ToString();
        }

        public static string GetExcelString<T>(IEnumerable<T> m, params string[] containPropertys) where T : class,new()
        {
            return GetExcelString<T>(m, "gb2312", containPropertys);
        }

        public static string GetExcelString<T>(IEnumerable<T> m, string templetpath, Encoding encode = null)
        {
            if (!File.Exists(templetpath))
            {
                throw new Exception("模板不能空");
                return string.Empty;
            }
            encode = encode == null ? Encoding.Default : encode;
            var s = "";

            using (var stream = new StreamReader(templetpath, encode))
            {
                s = stream.ReadToEnd();
            }

            var trregex = new Regex("<tr(((?!(<tr|{%))(\\s|\\S))*){%(((?!</tr>)(\\s|\\S))*)</tr>", RegexOptions.IgnoreCase);
            var tdr = new Regex("{%(((?!}).)*)}", RegexOptions.IgnoreCase);
            if (tdr.IsMatch(s))
            {
                var matchs = tdr.Matches(s);

                foreach (Match mc in matchs)
                {
                    s = s.Replace(mc.Value, mc.Value.RemoveHtmlTag());
                }
            }
            var match = trregex.Match(s);

            if (match.Success)
            {
                var templet = match.Value;

                var type = typeof(T);

                var properties = type.GetProperties();

                var all = "";

                foreach (var item in m)
                {
                    var v = templet.ToString();

                    foreach (var p in properties)
                    {
                        var value = p.GetValue(item, null);

                        v = v.Replace("{%" + p.Name + "%}", value != null ? value.ToString() : "");

                    }

                    if (tdr.IsMatch(v))
                    {
                        v = tdr.Replace(v, "");
                    }

                    all += v;

                }

                return s.Replace(templet, all);

            }


            return string.Empty;
        }
    }
}