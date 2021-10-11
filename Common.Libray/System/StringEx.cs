using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using System.Web.Script.Serialization;
using System.Diagnostics;
using Common.Library.Log;
using ChinaBM.Common;

namespace System
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class StringEx
    {
        /// <summary>
        /// 按查找的字符截取
        /// </summary>
        /// <param name="value"></param>
        /// <param name="find"></param>
        /// <param name="findstart"></param>
        /// <param name="cutstart"></param>
        /// <returns></returns>
        public static string SubStringAt(this string value, string find, int findstart = 0, int cutstart = 0)
        {
            if (String.IsNullOrEmpty(value)) return String.Empty;
            if (String.IsNullOrEmpty(find)) return value;
            if (findstart > value.Length) return String.Empty;
            int i = value.IndexOf(find, findstart);
            if (i > -1)
            {
                if (i >= value.Length - cutstart) return String.Empty;
                return value.Substring(cutstart, i - cutstart);
            }
            return value;

        }
        /// <summary>
        /// 按查找的字符截取（从后向前）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="find"></param>
        /// <param name="findstart"></param>
        /// <param name="cutstart"></param>
        /// <returns></returns>
        public static string LastSubStringAt(this string value, string find, int findstart = -1, int cutstart = 0)
        {
            if (String.IsNullOrEmpty(value)) return String.Empty;
            if (String.IsNullOrEmpty(find)) return value;
            if (findstart == -1) findstart = value.Length;
            int i = value.LastIndexOf(find, findstart);

            if (i > -1)
            {
                if (i >= value.Length - cutstart) return String.Empty;
                return value.Substring(cutstart, i - cutstart);
            }
            return value;

        }
        /// <summary>
        /// 如果字符串为Null或空用 返回 defalut
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defalut"></param>
        /// <returns>如果字符串为Null或空用 返回 defalut</returns>
        public static string IsNullOrEmpty(this string value, object defalut)
        {
            return String.IsNullOrEmpty(value) ? string.Format("{0}", defalut) : value;
        }
        /// <summary>
        /// 查找指定字符在当前字符中的次数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="find"></param>
        /// <param name="compa"></param>
        /// <returns></returns>
        public static int StringCount(this string value, string find, StringComparison compa = StringComparison.OrdinalIgnoreCase)
        {
            //return new Regex(find, RegexOptions.IgnoreCase).Matches(value).Count;

            int count = 0; //计数器         



            int vlen = value.Length;
            int flen = find.Length;
            for (int i = 0; i <= vlen - flen; i++)
            {
                if (value.Substring(i, flen).Equals(find, compa))
                {
                    count++;
                }
            }
            return count;
        }
        /// <summary>
        /// 返回 非 null字符 or def
        /// </summary>
        /// <param name="value"></param>
        /// <param name="def"></param>
        /// <returns></returns>
        public static string NonNull(this string value, string def = "")
        {
            return value != null ? value : def;
        }
        /// <summary>
        /// 安全Trim
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TrimOrDefault(this String value)
        {
            return value != null ? value.Trim() : string.Empty;
        }
        /// <summary>
        ///Md5加密
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string Md5(this String s)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(s)));
            t2 = t2.Replace("-", "");
            return t2;
        }

        public static string Left(this String s, int size)
        {
            if (s.Length <= size)
            {
                return s;
            }

            if (size < 1)
            {
                return s;
            }

            return s.Substring(0, size);
        }

        #region CutString 从字符串的指定位置截取指定长度
        /// <summary>
        ///  从字符串的指定位置截取指定长度
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <param name="startIndex">开始截取位置</param>
        /// <param name="length">截取长度</param>
        /// <returns>截取出的新字符串</returns>
        public static string CutString(this String s, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex = startIndex - length;
                    }
                }
                if (startIndex > s.Length)
                {
                    return string.Empty;
                }
            }
            else
            {
                if (length < 0)
                {
                    return string.Empty;
                }
                if (length + startIndex > 0)
                {
                    length = length + startIndex;
                    startIndex = 0;
                }
                else
                {
                    return string.Empty;
                }
            }
            if (s.Length - startIndex < length)
            {
                length = s.Length - startIndex;
            }
            return s.Substring(startIndex, length);
        }
        #endregion

        #region CutString 从字符串的指定位置开始截取到字符串尾部
        /// <summary>
        ///  从字符串的指定位置开始截取到字符串尾部
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <param name="startIndex">开始截取位置</param>
        /// <returns>截取出的新字符串</returns>
        public static string CutString(this String s, int startIndex)
        {
            return CutString(s, startIndex, s.Length);
        }
        #endregion

        #region CutString 从字符串的指定位置截取指定长度（字符串如果操过指定长度则将超出的部分用指定字符串代替）
        /// <summary>
        /// 截取指定长度的字符串（字符串如果操过指定长度则将超出的部分用指定字符串代替）
        /// </summary>
        /// <param name="targetString">要检查的字符串</param>
        /// <param name="startIndex">起始位置</param>
        /// <param name="length">指定长度</param>
        /// <param name="newString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string CutString(this String targetString, int startIndex, int length, string newString)
        {
            string resultString = targetString;

            if (String.IsNullOrEmpty(targetString))
            {
                return string.Empty;
            }

            //Byte[] utf8Bytes = Encoding.UTF8.GetBytes(targetString);
            //foreach (char @char in Encoding.UTF8.GetChars(utf8Bytes))
            //{    
            //    //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
            //    if ((@char > '\u0800' && @char < '\u4e00') || (@char > '\xAC00' && @char < '\xD7A3'))
            //    {
            //        //当截取的起始位置超出字段串长度时
            //        if (startIndex >= targetString.Length)
            //        {
            //            return string.Empty;
            //        }
            //        return targetString.Substring(startIndex, ((length + startIndex) > targetString.Length) ? (targetString.Length - startIndex) : length);                      
            //    }
            //}
            if (length >= 0)
            {
                byte[] defaultBytes = Encoding.Default.GetBytes(targetString);
                //当字符串长度大于起始位置
                int endIndex = defaultBytes.Length;
                if (defaultBytes.Length > startIndex)
                {
                    //当要截取的长度在字符串的有效长度范围内
                    if (defaultBytes.Length > (startIndex + length))
                    {
                        endIndex = length + startIndex;
                    }
                    else
                    {
                        //当不在有效范围内时,只取到字符串的结尾
                        length = defaultBytes.Length - startIndex;
                        newString = string.Empty;
                    }
                    int realLength = length;
                    int[] resultFlag = new int[length];
                    int flag = 0;
                    for (int i = startIndex, j = 0; i < endIndex; i++, j++)
                    {
                        if (defaultBytes[i] > 127)
                        {
                            flag++;
                            if (flag == 3)
                            {
                                flag = 1;
                            }
                        }
                        else
                        {
                            flag = 0;
                        }
                        resultFlag[j] = flag;
                    }
                    if ((defaultBytes[endIndex - 1] > 127) && (resultFlag[length - 1] == 1))
                    {
                        realLength = length + 1;
                    }
                    byte[] resultBytes = new byte[realLength];
                    Array.Copy(defaultBytes, startIndex, resultBytes, 0, realLength);
                    resultString = Encoding.Default.GetString(resultBytes);
                    resultString = resultString + newString;
                }
                else
                {
                    throw new Exception("开始长度必须小于字符串长度");
                }
            }
            return resultString;
        }
        #endregion

        #region CutString 从字符串的指定位置开始截取到字符串尾部 （字符串如果操过指定长度则将超出的部分用指定字符串代替）
        /// <summary>
        /// 从字符串的指定位置开始截取到字符串尾部（字符串如果操过指定长度则将超出的部分用指定字符串代替）
        /// </summary>
        /// <param name="targetString">要检查的字符串</param>
        /// <param name="length">指定长度</param>
        /// <param name="newString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string CutString(this String targetString, int length, string newString)
        {
            return CutString(targetString, 0, length, newString);
        }
        #endregion

        #region 过滤HTML为安全的HTML

        private static readonly string allow_tags = "|img|p|a|b|strong|table|tr|td|thead|tbody|tfoot|br|i|sup|sub|h1|h2|h3|h4|h5|h6|ul|dl|dt|dd|";
        private static readonly string allow_attributes = "|src|href|style|title|class|alt|";

        private static readonly Regex Regex_SafeHtmlRemoveContentTags = new Regex(@"\<(script|head).*?\>.+?\</\1.*?\>|<!--.*?-->|<\!.*?>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);

        private static readonly Regex Regex_SafeHtmlAllTags = new Regex(@"(</?)(\w+)(\s*(?:[\w-]+\s*(?:=\s*(?:""[^""]*""|'[^']*'|[^ >]*)\s*)*)*)(/?\s*>)", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex Regex_SafeHtmlSpace = new Regex(@"\s+", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        /// <summary>
        /// 过滤HTML为安全的字符
        /// AppSettings 设置 allow_tags 为允许的标签 如: “a|b|table|tr|td”
        /// AppSettings 设置 allow_attributes 为允许包含的属性 如 “href|src”
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToSafeHTML(this String s)
        {
            if (s == null)
            {
                return "";
            }
            string r = Regex_SafeHtmlRemoveContentTags.Replace(s ?? "", "");
            //r = Regex_SafeHtmlAllTags.Replace(r, new MatchEvaluator(CheckSafeHtml));
            r = Regex_SafeHtmlSpace.Replace(r, " ");
            return r;
        }
        /// <summary>
        /// 判断文本框混合输入长度
        /// </summary>
        /// <param name="str">要判断的字符串</param>
        /// <param name="i">长度</param>
        /// <returns></returns>
        public static bool ChangeByte(string str, int i)
        {
            byte[] b = Encoding.Default.GetBytes(str);
            int m = b.Length;
            if (m < i) return true;
            else return false;
        }

        /// <summary>
        /// 分析每个标签
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private static string CheckSafeHtml(Match match)
        {
            //允许的标签
            var tag = match.Groups[2].ToString().ToLower();
            if (allow_tags.IndexOf("|" + tag + "|") != -1)
            {
                var strat = match.Groups[1].ToString();
                if (tag == "a" && strat != "</") //为所有A标签添加 “新窗口打开”
                    tag = "a target=\"_blank\"";
                return strat + tag + Regex_SafeHtmlAttribute.Replace(match.Groups[3].Value, new MatchEvaluator(CheckSafeAttr)).TrimEnd() + match.Groups[4];
            }

            return "";
        }

        private static readonly Regex Regex_SafeHtmlAttribute = new Regex(@"\b([^\s=]+)\s*(?:=\s*(""[^""]*""|'[^']*'|[^ >]*))?", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        private static readonly Regex Regex_SafeHtmlAttributeBadwords = new Regex(@"\b(expression|behavior|javascript|\-moz\-binding)\b", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline);
        /// <summary>
        /// 过滤非正常的属性
        /// </summary>
        /// <param name="match"></param>
        /// <returns></returns>
        private static string CheckSafeAttr(Match match)
        {
            var attname = match.Groups[1].ToString().ToLower();
            if (attname.Equals("target")) return "";
            if (allow_attributes.IndexOf("|" + attname + "|") != -1)
            {

                var attvalue = System.Web.HttpUtility.HtmlDecode(match.Groups[2].ToString());

                if (attvalue.Length > 0)
                {
                    var mark = attvalue.Substring(0, 1);
                    if ((mark.Equals("'") || mark.Equals("\"")))
                    {
                        var markend = attvalue.Substring(attvalue.Length - 1, 1);
                        if (attvalue.Length > 1 && (markend.Equals("'") || markend.Equals("\"")))
                        {
                            attvalue = attvalue.Substring(1, attvalue.Length - 2);
                        }
                        else
                        {
                            attvalue = attvalue.Substring(1, attvalue.Length - 1);
                        }
                    }
                    else
                    {
                        mark = "\"";
                    }
                    //return String.Format("{0}={1}{2}{1}", attname, mark, System.Web.HttpUtility.HtmlEncode(Regex_SafeHtmlAttributeBadwords.Replace(attvalue, "#")));
                    return attname + "=" + mark + System.Web.HttpUtility.HtmlEncode(Regex_SafeHtmlAttributeBadwords.Replace(attvalue, "#")) + mark;
                }
                else
                {
                    return attname;
                }

                //return match.ToString();
            }
            return "";

        }

        #endregion

        #region RemoveHtmlTag 移除Html标记
        /// <summary>
        /// 移除Html标记
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <returns></returns>
        public static string RemoveHtmlTag(this String targetString)
        {
            if (targetString == null)
            {
                return "";
            }
            targetString = targetString.Replace("&lt;", "<").Replace("&gt;", ">").Replace("\t", "").Replace("\r", "").Replace("\n", "");
            targetString = Regex.Replace(targetString, @"<[^>]*>", string.Empty, RegexOptions.IgnoreCase);
            targetString = Regex.Replace(targetString, @"&([a-zA-Z]{1,6});", string.Empty, RegexOptions.IgnoreCase);

            return targetString;
        }
        #endregion

        #region ClearBr 清除字符串中的回车/换行/空格
        /// <summary>
        ///  清除字符串中的回车/换行
        /// </summary>
        /// <param name="targetString">目标字符串</param>
        /// <returns>处理后的新字符串</returns>
        public static string ClearBr(this String targetString)
        {
            Regex regex = new Regex(@"(\r\n)", RegexOptions.IgnoreCase);
            for (Match match = regex.Match(targetString); match.Success; match = match.NextMatch())
            {
                targetString = targetString.Replace(match.Groups[0].ToString(), "");
            }
            return targetString.Replace(" ", "");
        }
        #endregion

        /// <summary>
        /// 去除分隔符分隔的字符串中重复内容
        /// </summary>
        /// <param name="split">字符串分隔符</param>
        /// <returns></returns>
        public static string RemoveRepeat(this String s, string split)
        {
            var strs = s.Split(new string[] { split }, StringSplitOptions.RemoveEmptyEntries);

            var list = new List<string>();

            for (var i = 0; i < strs.Length; i++)
            {
                if (!list.Contains(strs[i]))
                {
                    list.Add(strs[i]);
                }
            }
            var str = String.Join(split, list.ToArray());

            if (String.IsNullOrEmpty(str))
            {
                return "-";
            }
            return str;

        }


        /// <summary>
        /// 去除分隔符分隔的字符串中重复及要移除的内容
        /// </summary>
        /// <param name="split">字符串分隔符</param>
        /// <param name="removeString">要去除的字符</param>
        /// <returns></returns>
        public static string RemoveRepeat(this String s, string split, string removeString)
        {
            var strs = s.Split(new string[] { split }, StringSplitOptions.RemoveEmptyEntries);

            var list = new List<string>();

            for (var i = 0; i < strs.Length; i++)
            {
                if (!list.Contains(strs[i]) && strs[i] != removeString)
                {
                    list.Add(strs[i]);
                }
            }

            return String.Join(split, list.ToArray());

        }

        /// <summary>
        /// 对应num位左侧添加字符c
        /// </summary>
        /// <param name="c">要添加的字符</param>
        /// <param name="num">要添加字符的字符串中字符位置</param>
        /// <returns></returns>
        public static string LeftNumAddChar(this String s, char c, int num)
        {
            if (String.IsNullOrEmpty(s))
            {
                return s;
            }

            if (s.Length <= num)
            {
                return s;
            }

            var d = "";
            for (int i = s.Length - 1, j = 1; i >= 0; i--, j++)
            {
                if (j % num == 0)
                {
                    d = s[i] + d;
                    d = c + d;
                }
                else
                {
                    d = s[i] + d;
                }
            }

            return d;
        }


        #region EnterToBr 替换回车换行符为html换行符
        private static Regex duoyubr = new Regex(@"<br />((\s|&nbsp;| |)+)?<br />", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        ///  替换回车换行符为html换行符
        /// </summary>
        /// <param name="targetString"></param>
        /// <returns></returns>
        public static string EnterToBr(this String s)
        {
            string resultString;
            if (s == null)
            {
                resultString = string.Empty;
            }
            else
            {
                //s = s.Replace(" ", "&nbsp;");
                s = s.Replace("\r\n", "<br />");
                s = s.Replace("\n", "<br />");
                s = new Regex(@"<br />((\s|&nbsp;| |)+)?<br />").Replace(s, "");
                resultString = s;
            }
            return resultString;
        }

        #endregion

        /// <summary>
        /// 替换BR为回车，nbsp为空格
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string BrToEnter(this String s)
        {
            string resultString;
            if (s == null)
            {
                resultString = string.Empty;
            }
            else
            {
                s = s.Replace("&nbsp;", " ");
                s = Regex.Replace(s, "<br(( )+)?(/)?>", "\r\n", RegexOptions.IgnoreCase);
                resultString = s;
            }
            return resultString;
        }


        /// <summary>
        /// 把字符串转换成密码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToPassword(this string str)
        {
            string output = System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
            //if (Half)//16位MD5加密（取32位加密的9~25字符）
            output = output.Substring(8, 16);
            return output;
        }

        /// <summary>
        /// 返回缩略图地址
        /// </summary>
        /// <param name="str"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="mode"></param>
        /// <param name="noThumb"></param>
        /// <returns></returns>
        public static string ToImageThumb(this string str, int width = 0, int height = 0, ThumbMode mode = ThumbMode.Cut, bool? noThumb = null)
        {
            if (noThumb == null)
            {
                noThumb = ConvertKit.Convert(System.Configuration.ConfigurationManager.AppSettings["noThumb"], false);

            }


            if (noThumb.Value)
            {
                return str;
            }

            if (String.IsNullOrEmpty(str))
            {
                str = "/css/images/no_img.png";
            }

            if (str.StartsWith("http://", StringComparison.OrdinalIgnoreCase))
            {
                return str;
            }

            var s = "/u/{0}-{1}-{2}{3}";
            var m = "c";
            var d = new Dictionary<ThumbMode, string>() { { ThumbMode.Cut, "c" }, { ThumbMode.Height, "h" }, { ThumbMode.Width, "w" }, { ThumbMode.WidthAndHeight, "a" }, { ThumbMode.WidthOrHeight, "o" } };
            if (d.ContainsKey(mode))
            {
                m = d[mode];
            }

            var thumQuestion = ConvertKit.Convert(ConfigurationManager.AppSettings["ThumQuestion"], false);

            var domain = System.Configuration.ConfigurationManager.AppSettings["domain"].ToSafeString();

            return (thumQuestion ? "/AppCode/M.ashx?thumpath=" : ("http://" + domain)) + string.Format(s, width, height, m, str);
        }


        /// <summary>
        /// string返回一个DateTime?对象,如果string=""那么返回null
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string str)
        {
            return ToDateTime(str, null);
        }



        /// <summary>
        /// string返回一个DateTime?对象,如果string=""那么返回defaultValue
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string str, DateTime? defaultValue)
        {
            if (str == "")
            {
                return defaultValue;
            }
            else
            {
                try
                {
                    return Convert.ToDateTime(str);
                }
                catch
                {

                    return defaultValue;
                }

            }
        }

        #region 取得字符串首字母

        /// <summary>
        /// 获取首字母
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FirstAlphabet(this string str)
        {
            var s = "";
            foreach (var item in str)
            {
                s += OnlyFirstAlphabet(item.ToString());
            }
            return s;
        }

        private static string OnlyFirstAlphabet(string str)
        {
            if (str.CompareTo("吖") < 0)
            {
                string s = str.Substring(0, 1).ToUpper();
                if (char.IsNumber(s, 0))
                {
                    return "0";
                }
                else
                {
                    return s;
                }
            }
            else if (str.CompareTo("八") < 0)
            {
                return "A";
            }
            else if (str.CompareTo("嚓") < 0)
            {
                return "B";
            }
            else if (str.CompareTo("咑") < 0)
            {
                return "C";
            }
            else if (str.CompareTo("妸") < 0)
            {
                return "D";
            }
            else if (str.CompareTo("发") < 0)
            {
                return "E";
            }
            else if (str.CompareTo("旮") < 0)
            {
                return "F";
            }
            else if (str.CompareTo("铪") < 0)
            {
                return "G";
            }
            else if (str.CompareTo("讥") < 0)
            {
                return "H";
            }
            else if (str.CompareTo("咔") < 0)
            {
                return "J";
            }
            else if (str.CompareTo("垃") < 0)
            {
                return "K";
            }
            else if (str.CompareTo("嘸") < 0)
            {
                return "L";
            }
            else if (str.CompareTo("拏") < 0)
            {
                return "M";
            }
            else if (str.CompareTo("噢") < 0)
            {
                return "N";
            }
            else if (str.CompareTo("妑") < 0)
            {
                return "O";
            }
            else if (str.CompareTo("七") < 0)
            {
                return "P";
            }
            else if (str.CompareTo("亽") < 0)
            {
                return "Q";
            }
            else if (str.CompareTo("仨") < 0)
            {
                return "R";
            }
            else if (str.CompareTo("他") < 0)
            {
                return "S";
            }
            else if (str.CompareTo("哇") < 0)
            {
                return "T";
            }
            else if (str.CompareTo("夕") < 0)
            {
                return "W";
            }
            else if (str.CompareTo("丫") < 0)
            {
                return "X";
            }
            else if (str.CompareTo("帀") < 0)
            {
                return "Y";
            }
            else if (str.CompareTo("咗") < 0)
            {
                return "Z";
            }
            else
            {
                return "0";
            }
        }

        #endregion

        #region Unicode与gb互转
        public static string ConvertUniToGB(this string unicodeString)
        {
            return Regex.Unescape(unicodeString);
        }

        public static string ConvertToUnicode(this string strGB)
        {
            return Regex.Escape(strGB);
        }

        #endregion

        #region 字符串替换，字典全部替换

        /// <summary>
        /// 字符串替换，字典全部替换
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="d">替换字典(key元字符串,value替换后字符串)</param>
        /// <param name="onlyone">替换一次</param>
        /// <param name="excludehtml">排除html标签内</param>
        /// <returns></returns>
        public static string Replace(this string str, Dictionary<string, string> d, bool onlyone = true, bool excludehtml = true)
        {
            if (d == null || d.Count == 0)
            {
                return str;
            }

            var filter = new TrieFilter(d.Select(a => a.Key).ToArray(), RegexOptions.IgnoreCase);

            return filter.Replace(str, d.GroupBy(a => a.Key.ToLower()).ToDictionary(a => a.Key, a => a.FirstOrDefault().Value), onlyone, excludehtml);
        }

        #endregion

        /// <summary>
        /// Json字符串转Dictionary字典
        /// </summary>
        /// <param name="str"></param>
        /// <returns>格式错误返回空字典</returns>
        public static Dictionary<string, object> JsonToDictionary(this string str)
        {
            try
            {
                JavaScriptSerializer serialize = new JavaScriptSerializer();
                var o = serialize.DeserializeObject(str) as Dictionary<string, object>;

                return o;
            }
            catch
            {
                return new Dictionary<string, object>();
            }
        }


        /// <summary>
        /// Json字符串转object数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns>格式错误返回空字典</returns>
        public static object[] JsonToObjectArray(this string str)
        {
            try
            {
                JavaScriptSerializer serialize = new JavaScriptSerializer();
                var o = serialize.DeserializeObject(str) as object[];

                return o;
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// 过滤低位非打印字符
        /// </summary>
        /// <param name="tmp"></param>
        /// <returns></returns>
        public static string ReplaceLowOrderASCIICharacters(this string tmp)
        {
            StringBuilder info = new StringBuilder();
            foreach (char cc in tmp)
            {
                int ss = (int)cc;
                if (((ss >= 0) && (ss <= 8)) || ((ss >= 11) && (ss <= 12)) || ((ss >= 14) && (ss <= 32)))
                    info.AppendFormat(" ", ss);//&#x{0:X};
                else info.Append(cc);
            }
            return info.ToString();
        }



        /// <summary>
        /// 分句方法，将一段文字分解为多句话的集合（去除空字符串和重复字符串）
        /// </summary>
        /// <param name="s">需要分句的文字</param>
        /// <param name="words">每句字数(中文占2个)</param>
        /// <returns></returns>
        public static List<string> Clause(this string s, int words)
        {
            if (String.IsNullOrWhiteSpace(s))
            {
                return new List<string>();
            }
            var strs = s.Split(new[] { '.', '。', '!', '！', '\n', ';' }).Where(a => !String.IsNullOrWhiteSpace(a)).Distinct();

            var rl = new List<string>();

            foreach (var item in strs)
            {
                var bytes = System.Text.Encoding.Default.GetBytes(item);

                var l = new List<byte>();
                var ll = new List<string>();
                var twocount = new List<byte>();
                for (var i = 0; i < bytes.Length; i++)
                {
                    if (bytes[i] > 127)
                    {
                        twocount.Add(bytes[i]);
                    }
                    l.Add(bytes[i]);
                    if (l.Count > words || i >= bytes.Length - 1)
                    {

                        if (twocount.Count % 2 == 1)
                        {
                            l.Add(bytes[i + 1]);
                            i++;
                        }

                        ll.Add(System.Text.Encoding.Default.GetString(l.ToArray()).ToSafeString().Trim());
                        l.Clear();
                        twocount.Clear();
                    }
                }

                rl.AddRange(ll);
            }

            return rl;
        }


        /// <summary>
        /// 获取字符串中间的字符
        /// </summary>
        /// <param name="s">要获取的字符串</param>
        /// <param name="middlenumber">获取中部字符数</param>
        /// <returns></returns>
        public static string GetStringMiddleCount(this string s, int middlenumber)
        {
            if (String.IsNullOrWhiteSpace(s))
            {
                return s;
            }

            if (s.Length <= middlenumber)
            {
                return s;
            }

            var start = (s.Length - middlenumber) / 2;

            if (start == 0)
            {
                return s;
            }

            return s.Substring(start, middlenumber);
        }

        /// <summary>
        /// 标点符号正则
        /// </summary>
        private static Regex PunctuationRegex = new Regex(@"[\p{P}*]", RegexOptions.Compiled);

        /// <summary>
        /// 判断key在s中存在的比例（方法效率280W无匹配字符串，耗时0.8秒，匹配字符串27个字符,1W无匹配字符串，耗时20毫秒左右）
        /// </summary>
        /// <param name="instr">源字符串</param>
        /// <param name="befound">需要查询的字符串(befound包含instr)</param>
        /// <returns>100以内，百分比</returns>
        public static int ExistPercent(this string instr, string befound)
        {
            //标点符号替换为空防止中英文写切断
            var key = PunctuationRegex.Replace(befound, " ");
            var s = PunctuationRegex.Replace(instr, " ");

            //Stopwatch _timer = new Stopwatch();
            //_timer.Start();
            if (String.IsNullOrWhiteSpace(s) || String.IsNullOrWhiteSpace(key))
            {
                return 0;
            }

            var incount = 0;//存在连续字符数

            for (var i = 0; i < s.Length; i++)
            {
                //获取字符串包含字符的位置数组
                var indexlist = key.InsertOfList(s[i]);

                if (indexlist.Length > 0)
                {
                    //包含字符匹配处理
                    foreach (var index in indexlist)
                    {
                        //当前字符
                        var sing = s[i].ToSafeString();
                        if (key.Length <= index)
                        {
                            continue;
                        }
                        var king = key[index].ToSafeString();
                        var isbreak = false;//匹配后break当前foreach

                        for (var j = i + 1; j < s.Length && (index + j - i) < key.Length; j++)
                        {
                            sing += s[j];
                            king += key[index + j - i];
                            if (sing == king && j + 1 < s.Length && (index + j - i + 1) < key.Length)
                            {
                                incount++;
                            }
                            else
                            {
                                if (king.Length > 2 || sing == king)
                                {
                                    key = key.Substring(0, index) + key.Substring(index + j - i);
                                    i = j - 1;
                                }
                                break;
                            }
                            if (j == i + 1)
                            {
                                isbreak = true;
                                incount++;
                            }
                        }
                        if (isbreak)
                        {
                            break;
                        }
                    }
                }
            }

            return (((double)(incount) / (double)befound.Length) * 100).ToInt();
        }

        /// <summary>
        /// 字符串中包含字符的位置集合
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int[] InsertOfList(this string str, char value)
        {
            var l = new List<int>();

            for (var i = 0; i < str.Length; i++)
            {
                if (str[i] == value)
                {
                    l.Add(i);
                }
            }
            return l.ToArray();
        }



        #region 格式化Html标签，保留生成只有P和IMG的Html代码



        //private static Regex emptyRegex = new Regex(@"<(((?!(\bimg\b|\bp\b|\btable\b|\btbody\b|\bth\b|\btr\b|\btd\b|>))(.|\s))*)>", RegexOptions.Compiled | RegexOptions.IgnoreCase);//去掉p和img以外的其他元素
        private static Regex emptyRegex = new Regex(@"<(((?!(\bimg\b|\bp\b|>))(.|\s))*)>", RegexOptions.Compiled | RegexOptions.IgnoreCase);//去掉p和img以外的其他元素
        private static string splitkey = "$##|##$";//分隔字符串替换key
        private static Regex splitRegex = new Regex(@"\$##\|##\$", RegexOptions.Compiled | RegexOptions.IgnoreCase);//分隔字符串正则
        private static Regex elementRegex = new Regex(@"##x(?<id>\d+)x##", RegexOptions.Compiled | RegexOptions.IgnoreCase);//替换已被p包裹或未被p包裹的img标签
        private static Regex removestyleRegex = new Regex(@"style=('|"")(((?!\1).)*)\1", RegexOptions.Compiled | RegexOptions.IgnoreCase);//去style属性


        #region FormattingHtml 格式化html字符串(去掉样式，除p和img的其他标签)


        private static Regex pregex = new Regex(@"<p(((?!>)(.|\s))*)>(((?!</p>)(.|\s))*)</p>", RegexOptions.Compiled | RegexOptions.IgnoreCase);//p标签元素
        //private static Regex duoyup = new Regex(@"<p>((\s|&nbsp;| |)+)?</p>", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex pregexend = new Regex(@"<p[^>]>((<br>|<br/>|<br />|\s)+)?</p>", RegexOptions.Compiled | RegexOptions.IgnoreCase);//直接结束P标签元素
        private static Regex moreenterreplaceone = new Regex(@"\r\n((\s)+)?\r\n", RegexOptions.Compiled);//多个换行替换成一个
        private static Regex enterpRegex = new Regex(@"\r\n((\s|:|：)+)?</p>", RegexOptions.Compiled);//多个换行替换成一个
        private static Regex enteremptyRegex = new System.Text.RegularExpressions.Regex(@"\r\n(\s)+", RegexOptions.Compiled);
        private static Regex zhongWenRegex = new Regex(@"[\u4e00-\u9fbb]{1}", RegexOptions.Compiled);
        /// <summary>
        /// 格式化html字符串(去掉样式，除p和img的其他标签)
        /// </summary>
        /// <param name="htmlstr">需要格式化的字符串</param>
        /// <param name="needremoveNoPAndImg">是否过滤除p以外的标签</param>
        /// <returns></returns>
        public static string FormattingHtml(this string htmlstr, bool needremoveNoPAndImg = true)
        {
            if (String.IsNullOrWhiteSpace(htmlstr))
            {
                return string.Empty;
            }
            var s = htmlstr;
            if (needremoveNoPAndImg)
            {
                s = removestyleRegex.Replace(emptyRegex.Replace(htmlstr, ""), "");//过滤除开p的其他元素
            }

            s = pregexend.Replace(s, "");
            var str = s;
            var plist = new Dictionary<int, string>();
            var matchs = pregex.Matches(s);
            for (var i = 0; i < matchs.Count; i++)
            {
                var match = matchs[i];

                str = str.Replace(match.Value, splitkey + ("##x" + i + "x##") + splitkey);
                plist.Add(i, match.Value);
            }

            str = string.Join("", splitRegex.Split(str).Where(a => !String.IsNullOrWhiteSpace(a)).Select(a =>
            {
                var match = elementRegex.Match(a);
                var pstr = "";
                if (!match.Success)
                {
                    pstr = AppendHtmlP(a);
                }
                else
                {
                    pstr = plist[match.Groups["id"].Value.ToInt()];

                }

                if (pstr.IndexOf("<img", StringComparison.OrdinalIgnoreCase) == -1 && String.IsNullOrWhiteSpace(pstr.RemoveHtmlTag().Replace("\r\n", "").Replace("&nbsp;", "").Trim()))
                {
                    return "";
                }

                while (moreenterreplaceone.IsMatch(pstr))
                {
                    pstr = moreenterreplaceone.Replace(pstr, "\r\n");
                }
                pstr = enteremptyRegex.Replace(enterpRegex.Replace(pstr, "</p>"), "\r\n");

                if (!zhongWenRegex.IsMatch(pstr))
                {
                    if (pstr.IndexOf("<img ", StringComparison.OrdinalIgnoreCase) == -1 && pstr.Length < 15)
                    {
                        pstr = "";
                    }
                }

                return pstr;
            }).ToList());

            return str;
        }



        /// <summary>
        /// 字符串增加P标签，img标签会Split成多个P
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private static string AppendHtmlP(string s)
        {
            var imgregex = new Regex(@"<img(((?!/>).)*)/>", RegexOptions.Compiled | RegexOptions.IgnoreCase);

            var matchs = imgregex.Matches(s);

            var plist = new Dictionary<int, string>();

            for (var i = 0; i < matchs.Count; i++)
            {
                var match = matchs[i];

                s = s.Replace(match.Value, splitkey + ("##x" + i + "x##") + splitkey);
                plist.Add(i, match.Value);
            }


            var newstr = string.Join("", splitRegex.Split(s).Where(a => !String.IsNullOrWhiteSpace(a)).Select(a =>
            {
                var str = "<p>" + a + "</p>";
                var match = elementRegex.Match(str);
                if (match.Success)
                {
                    str = str.Replace(match.Value, plist[match.Groups["id"].Value.ToInt()]);
                }
                return str;
            }));

            return newstr;
        }

        #endregion

        #endregion


        /// <summary >
        /// 判断是否有中文
        /// </summary >
        /// <param name="str" ></param >
        /// <returns ></returns >
        public static bool IsIncludeChinese(this string str)
        {
            Regex regex = new Regex("[\u4e00-\u9fa5]");
            Match m = regex.Match(str);
            return m.Success;
        }




        /// <summary>
        /// 字符串随机位置插入字符串
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="insert">插入字符串</param>
        /// <param name="excludehtml">判断不在html标签中</param>
        /// <returns></returns>
        public static string RandomInsertString(this string str, string insert, bool excludehtml = true)
        {
            if (String.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            if (String.IsNullOrEmpty(insert))
            {
                return str;
            }

            var count = 0;
            while (true)
            {
                if (count == 10)
                {
                    return str;
                }
                var num = new Random(Guid.NewGuid().GetHashCode()).Next(0, str.Length);

                var start = str.Substring(0, num);
                var end = str.Substring(num);

                if (excludehtml)
                {
                    if (start.StringCount("<a") != start.StringCount("</a>") || start.StringCount("<") != start.StringCount(">"))
                    {
                        count++;
                        continue;
                    }
                }

                return start + insert + end;
            }
        }



        /// <summary>
        /// 根据详细地址字符串提取地区名称
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string ExtractCityName(this string address)
        {
            if (String.IsNullOrWhiteSpace(address))
            {
                return "";
            }

            var pros = new Dictionary<int, string>
            {
                {1,"北京"},
                {2,"天津"},
                {3,"河北"},
                {4,"山西"},
                {5,"内蒙古"},
                {6,"辽宁"},
                {7,"吉林"},
                {8,"黑龙江"},
                {9,"上海"},
                {10,"江苏"},
                {11,"浙江"},
                {12,"安徽"},
                {13,"福建"},
                {14,"江西"},
                {15,"山东"},
                {16,"河南"},
                {17,"湖北"},
                {18,"湖南"},
                {19,"广东"},
                {20,"广西"},
                {21,"海南"},
                {22,"重庆"},
                {23,"四川"},
                {24,"贵州"},
                {25,"云南"},
                {26,"西藏"},
                {27,"陕西"},
                {28,"甘肃"},
                {29,"青海"},
                {30,"宁夏"},
                {31,"新疆"},
                {32,"香港"},
                {33,"澳门"},
                {34,"台湾"}
            };


            var citys = new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(1,"北京"),
                new KeyValuePair<int, string>(2,"天津"),
                new KeyValuePair<int, string>(3,"石家庄"),
                new KeyValuePair<int, string>(3,"唐山"),
                new KeyValuePair<int, string>(3,"秦皇岛"),
                new KeyValuePair<int, string>(3,"邯郸"),
                new KeyValuePair<int, string>(3,"邢台"),
                new KeyValuePair<int, string>(3,"保定"),
                new KeyValuePair<int, string>(3,"张家口"),
                new KeyValuePair<int, string>(3,"承德"),
                new KeyValuePair<int, string>(3,"沧州"),
                new KeyValuePair<int, string>(3,"廊坊"),
                new KeyValuePair<int, string>(3,"衡水"),
                new KeyValuePair<int, string>(4,"太原"),
                new KeyValuePair<int, string>(4,"大同"),
                new KeyValuePair<int, string>(4,"阳泉"),
                new KeyValuePair<int, string>(4,"长治"),
                new KeyValuePair<int, string>(4,"晋城"),
                new KeyValuePair<int, string>(4,"朔州"),
                new KeyValuePair<int, string>(4,"晋中"),
                new KeyValuePair<int, string>(4,"运城"),
                new KeyValuePair<int, string>(4,"忻州"),
                new KeyValuePair<int, string>(4,"临汾"),
                new KeyValuePair<int, string>(4,"吕梁"),
                new KeyValuePair<int, string>(5,"呼和浩特"),
                new KeyValuePair<int, string>(5,"包头"),
                new KeyValuePair<int, string>(5,"乌海"),
                new KeyValuePair<int, string>(5,"赤峰"),
                new KeyValuePair<int, string>(5,"通辽"),
                new KeyValuePair<int, string>(5,"鄂尔多斯"),
                new KeyValuePair<int, string>(5,"呼伦贝尔"),
                new KeyValuePair<int, string>(5,"巴彦淖尔"),
                new KeyValuePair<int, string>(5,"乌兰察布"),
                new KeyValuePair<int, string>(5,"兴安"),
                new KeyValuePair<int, string>(5,"锡林郭勒"),
                new KeyValuePair<int, string>(5,"阿拉善"),
                new KeyValuePair<int, string>(6,"沈阳"),
                new KeyValuePair<int, string>(6,"大连"),
                new KeyValuePair<int, string>(6,"鞍山"),
                new KeyValuePair<int, string>(6,"抚顺"),
                new KeyValuePair<int, string>(6,"本溪"),
                new KeyValuePair<int, string>(6,"丹东"),
                new KeyValuePair<int, string>(6,"锦州"),
                new KeyValuePair<int, string>(6,"营口"),
                new KeyValuePair<int, string>(6,"阜新"),
                new KeyValuePair<int, string>(6,"辽阳"),
                new KeyValuePair<int, string>(6,"盘锦"),
                new KeyValuePair<int, string>(6,"铁岭"),
                new KeyValuePair<int, string>(6,"朝阳"),
                new KeyValuePair<int, string>(6,"葫芦岛"),
                new KeyValuePair<int, string>(7,"长春"),
                new KeyValuePair<int, string>(7,"吉林"),
                new KeyValuePair<int, string>(7,"四平"),
                new KeyValuePair<int, string>(7,"辽源"),
                new KeyValuePair<int, string>(7,"通化"),
                new KeyValuePair<int, string>(7,"白山"),
                new KeyValuePair<int, string>(7,"松原"),
                new KeyValuePair<int, string>(7,"白城"),
                new KeyValuePair<int, string>(7,"延边"),
                new KeyValuePair<int, string>(8,"哈尔滨"),
                new KeyValuePair<int, string>(8,"齐齐哈尔"),
                new KeyValuePair<int, string>(8,"鸡西"),
                new KeyValuePair<int, string>(8,"鹤岗"),
                new KeyValuePair<int, string>(8,"双鸭山"),
                new KeyValuePair<int, string>(8,"大庆"),
                new KeyValuePair<int, string>(8,"伊春"),
                new KeyValuePair<int, string>(8,"佳木斯"),
                new KeyValuePair<int, string>(8,"七台河"),
                new KeyValuePair<int, string>(8,"牡丹江"),
                new KeyValuePair<int, string>(8,"黑河"),
                new KeyValuePair<int, string>(8,"绥化"),
                new KeyValuePair<int, string>(8,"大兴安岭"),
                new KeyValuePair<int, string>(9,"上海"),
                new KeyValuePair<int, string>(10,"南京"),
                new KeyValuePair<int, string>(10,"无锡"),
                new KeyValuePair<int, string>(10,"徐州"),
                new KeyValuePair<int, string>(10,"常州"),
                new KeyValuePair<int, string>(10,"苏州"),
                new KeyValuePair<int, string>(10,"南通"),
                new KeyValuePair<int, string>(10,"连云港"),
                new KeyValuePair<int, string>(10,"淮安"),
                new KeyValuePair<int, string>(10,"盐城"),
                new KeyValuePair<int, string>(10,"扬州"),
                new KeyValuePair<int, string>(10,"镇江"),
                new KeyValuePair<int, string>(10,"泰州"),
                new KeyValuePair<int, string>(10,"宿迁"),
                new KeyValuePair<int, string>(11,"杭州"),
                new KeyValuePair<int, string>(11,"宁波"),
                new KeyValuePair<int, string>(11,"温州"),
                new KeyValuePair<int, string>(11,"嘉兴"),
                new KeyValuePair<int, string>(11,"湖州"),
                new KeyValuePair<int, string>(11,"绍兴"),
                new KeyValuePair<int, string>(11,"金华"),
                new KeyValuePair<int, string>(11,"衢州"),
                new KeyValuePair<int, string>(11,"舟山"),
                new KeyValuePair<int, string>(11,"台州"),
                new KeyValuePair<int, string>(11,"丽水"),
                new KeyValuePair<int, string>(12,"合肥"),
                new KeyValuePair<int, string>(12,"芜湖"),
                new KeyValuePair<int, string>(12,"蚌埠"),
                new KeyValuePair<int, string>(12,"淮南"),
                new KeyValuePair<int, string>(12,"马鞍山"),
                new KeyValuePair<int, string>(12,"淮北"),
                new KeyValuePair<int, string>(12,"铜陵"),
                new KeyValuePair<int, string>(12,"安庆"),
                new KeyValuePair<int, string>(12,"黄山"),
                new KeyValuePair<int, string>(12,"滁州"),
                new KeyValuePair<int, string>(12,"阜阳"),
                new KeyValuePair<int, string>(12,"宿州"),
                new KeyValuePair<int, string>(12,"巢湖"),
                new KeyValuePair<int, string>(12,"六安"),
                new KeyValuePair<int, string>(12,"亳州"),
                new KeyValuePair<int, string>(12,"池州"),
                new KeyValuePair<int, string>(12,"宣城"),
                new KeyValuePair<int, string>(13,"福州"),
                new KeyValuePair<int, string>(13,"厦门"),
                new KeyValuePair<int, string>(13,"莆田"),
                new KeyValuePair<int, string>(13,"三明"),
                new KeyValuePair<int, string>(13,"泉州"),
                new KeyValuePair<int, string>(13,"漳州"),
                new KeyValuePair<int, string>(13,"南平"),
                new KeyValuePair<int, string>(13,"龙岩"),
                new KeyValuePair<int, string>(13,"宁德"),
                new KeyValuePair<int, string>(14,"南昌"),
                new KeyValuePair<int, string>(14,"景德镇"),
                new KeyValuePair<int, string>(14,"萍乡"),
                new KeyValuePair<int, string>(14,"九江"),
                new KeyValuePair<int, string>(14,"新余"),
                new KeyValuePair<int, string>(14,"鹰潭"),
                new KeyValuePair<int, string>(14,"赣州"),
                new KeyValuePair<int, string>(14,"吉安"),
                new KeyValuePair<int, string>(14,"宜春"),
                new KeyValuePair<int, string>(14,"抚州"),
                new KeyValuePair<int, string>(14,"上饶"),
                new KeyValuePair<int, string>(15,"济南"),
                new KeyValuePair<int, string>(15,"青岛"),
                new KeyValuePair<int, string>(15,"淄博"),
                new KeyValuePair<int, string>(15,"枣庄"),
                new KeyValuePair<int, string>(15,"东营"),
                new KeyValuePair<int, string>(15,"烟台"),
                new KeyValuePair<int, string>(15,"潍坊"),
                new KeyValuePair<int, string>(15,"济宁"),
                new KeyValuePair<int, string>(15,"泰安"),
                new KeyValuePair<int, string>(15,"威海"),
                new KeyValuePair<int, string>(15,"日照"),
                new KeyValuePair<int, string>(15,"莱芜"),
                new KeyValuePair<int, string>(15,"临沂"),
                new KeyValuePair<int, string>(15,"德州"),
                new KeyValuePair<int, string>(15,"聊城"),
                new KeyValuePair<int, string>(15,"滨州"),
                new KeyValuePair<int, string>(15,"荷泽"),
                new KeyValuePair<int, string>(16,"郑州"),
                new KeyValuePair<int, string>(16,"开封"),
                new KeyValuePair<int, string>(16,"洛阳"),
                new KeyValuePair<int, string>(16,"平顶山"),
                new KeyValuePair<int, string>(16,"安阳"),
                new KeyValuePair<int, string>(16,"鹤壁"),
                new KeyValuePair<int, string>(16,"新乡"),
                new KeyValuePair<int, string>(16,"焦作"),
                new KeyValuePair<int, string>(16,"濮阳"),
                new KeyValuePair<int, string>(16,"许昌"),
                new KeyValuePair<int, string>(16,"漯河"),
                new KeyValuePair<int, string>(16,"三门峡"),
                new KeyValuePair<int, string>(16,"南阳"),
                new KeyValuePair<int, string>(16,"商丘"),
                new KeyValuePair<int, string>(16,"信阳"),
                new KeyValuePair<int, string>(16,"周口"),
                new KeyValuePair<int, string>(16,"驻马店"),
                new KeyValuePair<int, string>(17,"武汉"),
                new KeyValuePair<int, string>(17,"黄石"),
                new KeyValuePair<int, string>(17,"十堰"),
                new KeyValuePair<int, string>(17,"宜昌"),
                new KeyValuePair<int, string>(17,"襄樊"),
                new KeyValuePair<int, string>(17,"鄂州"),
                new KeyValuePair<int, string>(17,"荆门"),
                new KeyValuePair<int, string>(17,"孝感"),
                new KeyValuePair<int, string>(17,"荆州"),
                new KeyValuePair<int, string>(17,"黄冈"),
                new KeyValuePair<int, string>(17,"咸宁"),
                new KeyValuePair<int, string>(17,"随州"),
                new KeyValuePair<int, string>(17,"恩施"),
                new KeyValuePair<int, string>(17,"神农架"),
                new KeyValuePair<int, string>(18,"长沙"),
                new KeyValuePair<int, string>(18,"株洲"),
                new KeyValuePair<int, string>(18,"湘潭"),
                new KeyValuePair<int, string>(18,"衡阳"),
                new KeyValuePair<int, string>(18,"邵阳"),
                new KeyValuePair<int, string>(18,"岳阳"),
                new KeyValuePair<int, string>(18,"常德"),
                new KeyValuePair<int, string>(18,"张家界"),
                new KeyValuePair<int, string>(18,"益阳"),
                new KeyValuePair<int, string>(18,"郴州"),
                new KeyValuePair<int, string>(18,"永州"),
                new KeyValuePair<int, string>(18,"怀化"),
                new KeyValuePair<int, string>(18,"娄底"),
                new KeyValuePair<int, string>(18,"湘西"),
                new KeyValuePair<int, string>(19,"广州"),
                new KeyValuePair<int, string>(19,"韶关"),
                new KeyValuePair<int, string>(19,"深圳"),
                new KeyValuePair<int, string>(19,"珠海"),
                new KeyValuePair<int, string>(19,"汕头"),
                new KeyValuePair<int, string>(19,"佛山"),
                new KeyValuePair<int, string>(19,"江门"),
                new KeyValuePair<int, string>(19,"湛江"),
                new KeyValuePair<int, string>(19,"茂名"),
                new KeyValuePair<int, string>(19,"肇庆"),
                new KeyValuePair<int, string>(19,"惠州"),
                new KeyValuePair<int, string>(19,"梅州"),
                new KeyValuePair<int, string>(19,"汕尾"),
                new KeyValuePair<int, string>(19,"河源"),
                new KeyValuePair<int, string>(19,"阳江"),
                new KeyValuePair<int, string>(19,"清远"),
                new KeyValuePair<int, string>(19,"东莞"),
                new KeyValuePair<int, string>(19,"中山"),
                new KeyValuePair<int, string>(19,"潮州"),
                new KeyValuePair<int, string>(19,"揭阳"),
                new KeyValuePair<int, string>(19,"云浮"),
                new KeyValuePair<int, string>(20,"南宁"),
                new KeyValuePair<int, string>(20,"柳州"),
                new KeyValuePair<int, string>(20,"桂林"),
                new KeyValuePair<int, string>(20,"梧州"),
                new KeyValuePair<int, string>(20,"北海"),
                new KeyValuePair<int, string>(20,"防城港"),
                new KeyValuePair<int, string>(20,"钦州"),
                new KeyValuePair<int, string>(20,"贵港"),
                new KeyValuePair<int, string>(20,"玉林"),
                new KeyValuePair<int, string>(20,"百色"),
                new KeyValuePair<int, string>(20,"贺州"),
                new KeyValuePair<int, string>(20,"河池"),
                new KeyValuePair<int, string>(20,"来宾"),
                new KeyValuePair<int, string>(20,"崇左"),
                new KeyValuePair<int, string>(21,"海口"),
                new KeyValuePair<int, string>(21,"三亚"),
                new KeyValuePair<int, string>(22,"重庆"),
                new KeyValuePair<int, string>(23,"成都"),
                new KeyValuePair<int, string>(23,"自贡"),
                new KeyValuePair<int, string>(23,"攀枝花"),
                new KeyValuePair<int, string>(23,"泸州"),
                new KeyValuePair<int, string>(23,"德阳"),
                new KeyValuePair<int, string>(23,"绵阳"),
                new KeyValuePair<int, string>(23,"广元"),
                new KeyValuePair<int, string>(23,"遂宁"),
                new KeyValuePair<int, string>(23,"内江"),
                new KeyValuePair<int, string>(23,"乐山"),
                new KeyValuePair<int, string>(23,"南充"),
                new KeyValuePair<int, string>(23,"眉山"),
                new KeyValuePair<int, string>(23,"宜宾"),
                new KeyValuePair<int, string>(23,"广安"),
                new KeyValuePair<int, string>(23,"达州"),
                new KeyValuePair<int, string>(23,"雅安"),
                new KeyValuePair<int, string>(23,"巴中"),
                new KeyValuePair<int, string>(23,"资阳"),
                new KeyValuePair<int, string>(23,"阿坝"),
                new KeyValuePair<int, string>(23,"甘孜"),
                new KeyValuePair<int, string>(23,"凉山"),
                new KeyValuePair<int, string>(24,"贵阳"),
                new KeyValuePair<int, string>(24,"六盘水"),
                new KeyValuePair<int, string>(24,"遵义"),
                new KeyValuePair<int, string>(24,"安顺"),
                new KeyValuePair<int, string>(24,"铜仁"),
                new KeyValuePair<int, string>(24,"黔西南"),
                new KeyValuePair<int, string>(24,"毕节"),
                new KeyValuePair<int, string>(24,"黔东南"),
                new KeyValuePair<int, string>(24,"黔南"),
                new KeyValuePair<int, string>(25,"昆明"),
                new KeyValuePair<int, string>(25,"曲靖"),
                new KeyValuePair<int, string>(25,"玉溪"),
                new KeyValuePair<int, string>(25,"保山"),
                new KeyValuePair<int, string>(25,"昭通"),
                new KeyValuePair<int, string>(25,"丽江"),
                new KeyValuePair<int, string>(25,"思茅"),
                new KeyValuePair<int, string>(25,"临沧"),
                new KeyValuePair<int, string>(25,"楚雄"),
                new KeyValuePair<int, string>(25,"红河"),
                new KeyValuePair<int, string>(25,"文山"),
                new KeyValuePair<int, string>(25,"西双版纳"),
                new KeyValuePair<int, string>(25,"大理"),
                new KeyValuePair<int, string>(25,"德宏"),
                new KeyValuePair<int, string>(25,"怒江"),
                new KeyValuePair<int, string>(25,"迪庆"),
                new KeyValuePair<int, string>(26,"拉萨"),
                new KeyValuePair<int, string>(26,"昌都"),
                new KeyValuePair<int, string>(26,"山南"),
                new KeyValuePair<int, string>(26,"日喀则"),
                new KeyValuePair<int, string>(26,"那曲"),
                new KeyValuePair<int, string>(26,"阿里"),
                new KeyValuePair<int, string>(26,"林芝"),
                new KeyValuePair<int, string>(27,"西安"),
                new KeyValuePair<int, string>(27,"铜川"),
                new KeyValuePair<int, string>(27,"宝鸡"),
                new KeyValuePair<int, string>(27,"咸阳"),
                new KeyValuePair<int, string>(27,"渭南"),
                new KeyValuePair<int, string>(27,"延安"),
                new KeyValuePair<int, string>(27,"汉中"),
                new KeyValuePair<int, string>(27,"榆林"),
                new KeyValuePair<int, string>(27,"安康"),
                new KeyValuePair<int, string>(27,"商洛"),
                new KeyValuePair<int, string>(28,"兰州"),
                new KeyValuePair<int, string>(28,"金昌"),
                new KeyValuePair<int, string>(28,"白银"),
                new KeyValuePair<int, string>(28,"天水"),
                new KeyValuePair<int, string>(28,"武威"),
                new KeyValuePair<int, string>(28,"张掖"),
                new KeyValuePair<int, string>(28,"平凉"),
                new KeyValuePair<int, string>(28,"酒泉"),
                new KeyValuePair<int, string>(28,"庆阳"),
                new KeyValuePair<int, string>(28,"定西"),
                new KeyValuePair<int, string>(28,"陇南"),
                new KeyValuePair<int, string>(28,"临夏"),
                new KeyValuePair<int, string>(28,"甘南"),
                new KeyValuePair<int, string>(29,"西宁"),
                new KeyValuePair<int, string>(29,"海东"),
                new KeyValuePair<int, string>(29,"海北"),
                new KeyValuePair<int, string>(29,"黄南"),
                new KeyValuePair<int, string>(29,"海南"),
                new KeyValuePair<int, string>(29,"果洛"),
                new KeyValuePair<int, string>(29,"玉树"),
                new KeyValuePair<int, string>(29,"海西"),
                new KeyValuePair<int, string>(30,"银川"),
                new KeyValuePair<int, string>(30,"石嘴山"),
                new KeyValuePair<int, string>(30,"吴忠"),
                new KeyValuePair<int, string>(30,"固原"),
                new KeyValuePair<int, string>(30,"中卫"),
                new KeyValuePair<int, string>(31,"乌鲁木齐"),
                new KeyValuePair<int, string>(31,"克拉玛依"),
                new KeyValuePair<int, string>(31,"吐鲁番"),
                new KeyValuePair<int, string>(31,"哈密"),
                new KeyValuePair<int, string>(31,"昌吉"),
                new KeyValuePair<int, string>(31,"博尔塔拉"),
                new KeyValuePair<int, string>(31,"巴音郭楞"),
                new KeyValuePair<int, string>(31,"阿克苏"),
                new KeyValuePair<int, string>(31,"克孜勒苏柯尔"),
                new KeyValuePair<int, string>(31,"喀什"),
                new KeyValuePair<int, string>(31,"和田"),
                new KeyValuePair<int, string>(31,"伊犁"),
                new KeyValuePair<int, string>(31,"塔城"),
                new KeyValuePair<int, string>(31,"阿勒泰"),
                new KeyValuePair<int, string>(31,"石河子"),
                new KeyValuePair<int, string>(31,"阿拉尔"),
                new KeyValuePair<int, string>(31,"图木舒克"),
                new KeyValuePair<int, string>(31,"五家渠"),
                new KeyValuePair<int, string>(32,"香港"),
                new KeyValuePair<int, string>(33,"澳门"),
                new KeyValuePair<int, string>(34,"台湾")
            };

            if (address == "random")
            {
                var km = citys.Random();

                if (pros.ContainsKey(km.Key))
                {
                    return pros[km.Key] + "," + km.Value;
                }
                return "";
            }



            var s = citys.FirstOrDefault(a => address.IndexOf(a.Value) > -1);

            if (pros.ContainsKey(s.Key))
            {
                return pros[s.Key] + "," + s.Value;
            }
            else
            {
                s = pros.FirstOrDefault(a => address.IndexOf(a.Value) > -1);

                return s.Value;
            }
        }


        /// <summary>
        /// 获取中文字符串全拼
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetChineseSpells(this string str)
        {
            return new ChineseCode().GetSpell(str);
        }

        /// <summary>
        /// 集合中是否存在相似的字符串
        /// </summary>
        /// <param name="str">需要匹配字符串</param>
        /// <param name="l">集合</param>
        /// <param name="percentage">相似百分比</param>
        /// <returns></returns>
        public static bool HasSimilar(this string str, IEnumerable<string> l, int percentage = 60)
        {
            var first = "";
            var two = "";

            foreach (var item in l)
            {
                first = str.Length >= item.Length ? str : item;
                two = str.Length >= item.Length ? item : str;

                if (first.ExistPercent(two) > percentage)
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// url相对路径转绝对路径
        /// </summary>
        /// <param name="str"></param>
        /// <param name="targetUrl"></param>
        /// <returns></returns>
        public static string RelativeUrlToAbsolutely(this string str, string targetUrl)
        {
            if (String.IsNullOrWhiteSpace(str))
            {
                return null;
            }

            if (str.StartsWith("http://"))
            {
                return str;
            }

            //相对路径转绝对路径
            var uri = new Uri(targetUrl);

            if (str.StartsWith("/"))
            {
                return "http://" + uri.Host + str;
            }

            var ss = uri.Segments.ToList();

            if (!ss.Last().EndsWith("/"))
            {
                ss.Remove(ss.Last());
            }

            if (!str.StartsWith("/"))
            {
                str = "/" + str;
            }

            str = ("http://" + uri.Host + string.Join("", ss)).Trim('/') + str;

            return str;
        }

    }


    #region 高效字符串替换类

    class TrieNode
    {
        public bool m_end;
        public Dictionary<Char, TrieNode> m_values;
        public TrieNode()
        {
            m_values = new Dictionary<Char, TrieNode>();
        }
    }

    class TrieFilter : TrieNode
    {
        private RegexOptions REG_Options;

        public TrieFilter(string[] keys)
        {
            REG_Options = RegexOptions.None;
            AddKey(keys);
        }

        public TrieFilter(string[] keys, RegexOptions options)
        {
            if (keys == null) return;
            REG_Options = options;
            AddKey(keys);
        }

        private void AddKey(string[] keys)
        {
            for (int j = 0; j < keys.Length; j++)
            {
                string key = keys[j];
                if (string.IsNullOrEmpty(key))
                {
                    return;
                }
                TrieNode node = this;
                for (int i = 0; i < key.Length; i++)
                {
                    char c = GetChar(key[i]);
                    TrieNode subnode;
                    if (!node.m_values.TryGetValue(c, out subnode))
                    {
                        subnode = new TrieNode();
                        node.m_values.Add(c, subnode);
                    }
                    node = subnode;
                }
                node.m_end = true;
            }
        }


        private char GetChar(char car)
        {

            if (REG_Options == RegexOptions.IgnoreCase)
            {
                return char.ToLower(car);
            }
            return car;
        }


        /// <summary>
        /// 检查是否包含非法字符
        /// </summary>
        /// <param name="text">输入文本</param>       
        public bool HasBadWord(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                TrieNode node;
                if (m_values.TryGetValue(GetChar(text[i]), out node))
                {
                    for (int j = i + 1; j < text.Length; j++)
                    {
                        if (node.m_values.TryGetValue(GetChar(text[j]), out node))
                        {
                            if (node.m_end)
                            {
                                return true;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 检查是否包含非法字符
        /// </summary>
        /// <param name="text">输入文本</param>      
        public string FindOne(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                TrieNode node;
                if (m_values.TryGetValue(GetChar(text[i]), out node))
                {
                    for (int j = i + 1; j < text.Length; j++)
                    {
                        if (node.m_values.TryGetValue(GetChar(text[j]), out node))
                        {
                            if (node.m_end)
                            {
                                return text.Substring(i, j + 1 - i);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            return string.Empty;
        }

        //查找所有非法字符
        public IEnumerable<string> FindAll(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                TrieNode node;
                if (m_values.TryGetValue(GetChar(text[i]), out node))
                {
                    for (int j = i + 1; j < text.Length; j++)
                    {
                        if (node.m_values.TryGetValue(GetChar(text[j]), out node))
                        {
                            if (node.m_end)
                            {
                                yield return text.Substring(i, (j + 1 - i));
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">替换字符串</param>
        /// <param name="d">替换关键词字典</param>
        /// <param name="onlyone">只替换字符串一次相同的</param>
        /// <param name="excludehtml">排除html标签内</param>
        /// <returns></returns>
        public string Replace(string text, Dictionary<string, string> d, bool onlyone = true, bool excludehtml = true)
        {
            var ori = text;

            var onlysize = 0;

            for (int i = 0; i < text.Length; i++)
            {
                TrieNode node;
                if (m_values.TryGetValue(GetChar(text[i]), out node))
                {
                    for (int j = i + 1; j < text.Length; j++)
                    {
                        if (node.m_values.TryGetValue(GetChar(text[j]), out node))
                        {
                            if (node.m_end)
                            {
                                if (node.m_values.Count > 0 && text.Length > j + 1 && node.m_values.ContainsKey(GetChar(text[j + 1])))
                                {
                                    if (j + 1 >= text.Length)
                                    {
                                        return ori;
                                    }
                                    continue;
                                }

                                var isin = excludehtml;

                                if (excludehtml)
                                {
                                    var start = text.Substring(0, i);
                                    //var end = text.Substring((j + 1));

                                    if (start.StringCount("<a") == start.StringCount("</a>") && start.StringCount("<") == start.StringCount(">"))
                                    {
                                        isin = false;
                                    }

                                }
                                if (!isin)
                                {
                                    var m = d.SingleOrDefault(a => a.Key.Equals(text.Substring(i, (j + 1 - i)), StringComparison.OrdinalIgnoreCase));
                                    if (!String.IsNullOrEmpty(m.Value))
                                    {
                                        var key = text.Substring(i, (j + 1 - i));
                                        ori = ori.Substring(0, i + onlysize) + m.Value + ori.Substring(j + 1 + onlysize);
                                        onlysize += m.Value.Length - key.Length;

                                        if (onlyone)
                                        {
                                            d[m.Key] = "";
                                        }
                                    }
                                }
                                i = j;
                            }
                            if (j + 1 >= text.Length)
                            {
                                return ori;
                            }
                        }
                        else
                        {
                            if (j + 1 >= text.Length)
                            {
                                return ori;
                            }
                            break;
                        }
                    }
                }
            }

            return ori;
        }

        /// <summary>
        /// 替换非法字符
        /// </summary>
        /// <param name="text"></param>
        /// <param name="c">用于代替非法字符(英文替换)</param>
        /// <param name="d">用于代替非法字符(中文替换)</param>
        /// <returns></returns>
        public char[] Replace(string text, char c, char d, Encoding en)
        {
            char[] chars = text.ToCharArray();

            for (int i = 0; i < text.Length; i++)
            {
                TrieNode subnode;
                if (m_values.TryGetValue(GetChar(text[i]), out subnode))
                {
                    for (int j = i + 1; j < text.Length; j++)
                    {
                        if (subnode.m_values.TryGetValue(GetChar(text[j]), out subnode))
                        {
                            if (subnode.m_end)
                            {
                                for (int t = i; t <= j; t++)
                                {
                                    if (checkChinese(chars[t].ToString(), en))
                                    {
                                        chars[t] = d;
                                    }
                                    else
                                    {
                                        chars[t] = c;
                                    }

                                }
                                i = j;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }

            return chars;
        }



        /// <summary>
        /// 判断是否中文字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool checkChinese(char word)
        {
            if ((word >= 0x4e00) && (word <= 0x9fbb)) return true;
            else return false;
        }

        /// <summary>
        /// 判断是否中文字
        /// </summary>
        /// <param name="word"></param>
        /// <returns></returns>
        public bool checkChinese(string word, Encoding en)
        {
            byte[] b = en.GetBytes(word);
            int k = 0;
            int x = 0;
            if (b.Length > 1)
            {
                k = b[0] & 0xFF;
                x = b[1] & 0xFF;
            }
            int sum = k * 256 + x;
            if ((sum >= 0x4e00)) return true;
            else return false;
        }
    }

    #endregion


    #region 中文转全拼类

    /// <summary>
    /// 获取汉字的首字母和全拼
    /// </summary>
    public class ChineseCode
    {
        protected string _CnTxt;
        protected string _EnTxt;

        /// <summary>
        /// 要获取字母的汉字
        /// </summary>
        public string CnTxt
        {
            get { return _CnTxt; }
            set { _CnTxt = value; }
        }
        /// <summary>
        /// 汉字的首字母
        /// </summary>
        public string EnTxt
        {
            get { return _EnTxt; }
            set { _EnTxt = value; }
        }
        /// <summary>
        /// 构造方法
        /// </summary>
        public ChineseCode()
        {

        }
        /// <summary>
        /// 构造方法，获取汉字首字母拼音
        /// </summary>
        /// <param name="txt"></param>
        public ChineseCode(string txt)
        {
            _CnTxt = txt;
            _EnTxt = IndexCode(_CnTxt);
        }
        /// <summary>
        /// 获取汉字的首字母
        /// </summary>
        /// <param name="IndexTxt">汉字</param>
        /// <returns>汉字的首字母</returns>
        public String IndexCode(String IndexTxt)
        {
            String _Temp = null;
            for (int i = 0; i < IndexTxt.Length; i++)
                _Temp = _Temp + GetOneIndex(IndexTxt.Substring(i, 1));
            return _Temp;
        }

        /// <summary>
        /// 单个汉字
        /// </summary>
        /// <param name="OneIndexTxt">汉字</param>
        /// <returns>首拼</returns>
        private String GetOneIndex(String OneIndexTxt)
        {
            if (Convert.ToChar(OneIndexTxt) >= 0 && Convert.ToChar(OneIndexTxt) < 256)
                return OneIndexTxt;
            else
            {
                Encoding gb2312 = Encoding.GetEncoding("gb2312");
                byte[] unicodeBytes = Encoding.Unicode.GetBytes(OneIndexTxt);
                byte[] gb2312Bytes = Encoding.Convert(Encoding.Unicode, gb2312, unicodeBytes);
                return GetX(Convert.ToInt32(
                String.Format("{0:D2}", Convert.ToInt16(gb2312Bytes[0]) - 160)
                + String.Format("{0:D2}", Convert.ToInt16(gb2312Bytes[1]) - 160)
                ));
            }
        }
        /// <summary>
        /// 根据区位得到首字母
        /// </summary>
        /// <param name="GBCode">区位</param>
        /// <returns></returns>
        private String GetX(int GBCode)
        {
            if (GBCode >= 1601 && GBCode < 1637) return "a";
            if (GBCode >= 1637 && GBCode < 1833) return "b";
            if (GBCode >= 1833 && GBCode < 2078) return "c";
            if (GBCode >= 2078 && GBCode < 2274) return "d";
            if (GBCode >= 2274 && GBCode < 2302) return "e";
            if (GBCode >= 2302 && GBCode < 2433) return "f";
            if (GBCode >= 2433 && GBCode < 2594) return "g";
            if (GBCode >= 2594 && GBCode < 2787) return "h";
            if (GBCode >= 2787 && GBCode < 3106) return "j";
            if (GBCode >= 3106 && GBCode < 3212) return "k";
            if (GBCode >= 3212 && GBCode < 3472) return "l";
            if (GBCode >= 3472 && GBCode < 3635) return "m";
            if (GBCode >= 3635 && GBCode < 3722) return "n";
            if (GBCode >= 3722 && GBCode < 3730) return "o";
            if (GBCode >= 3730 && GBCode < 3858) return "p";
            if (GBCode >= 3858 && GBCode < 4027) return "q";
            if (GBCode >= 4027 && GBCode < 4086) return "r";
            if (GBCode >= 4086 && GBCode < 4390) return "s";
            if (GBCode >= 4390 && GBCode < 4558) return "t";
            if (GBCode >= 4558 && GBCode < 4684) return "w";
            if (GBCode >= 4684 && GBCode < 4925) return "x";
            if (GBCode >= 4925 && GBCode < 5249) return "y";
            if (GBCode >= 5249 && GBCode <= 5589) return "z";
            if (GBCode >= 5601 && GBCode <= 8794)
            {
                String CodeData = "cjwgnspgcenegypbtwxzdxykygtpjnmjqmbsgzscyjsyyfpggbzgydywjkgaljswkbjqhyjwpdzlsgmr"
                + "ybywwccgznkydgttngjeyekzydcjnmcylqlypyqbqrpzslwbdgkjfyxjwcltbncxjjjjcxdtqsqzycdxxhgckbphffss"
                + "pybgmxjbbyglbhlssmzmpjhsojnghdzcdklgjhsgqzhxqgkezzwymcscjnyetxadzpmdssmzjjqjyzcjjfwqjbdzbjgd"
                + "nzcbwhgxhqkmwfbpbqdtjjzkqhylcgxfptyjyyzpsjlfchmqshgmmxsxjpkdcmbbqbefsjwhwwgckpylqbgldlcctnma"
                + "eddksjngkcsgxlhzaybdbtsdkdylhgymylcxpycjndqjwxqxfyyfjlejbzrwccqhqcsbzkymgplbmcrqcflnymyqmsqt"
                + "rbcjthztqfrxchxmcjcjlxqgjmshzkbswxemdlckfsydsglycjjssjnqbjctyhbftdcyjdgwyghqfrxwckqkxebpdjpx"
                + "jqsrmebwgjlbjslyysmdxlclqkxlhtjrjjmbjhxhwywcbhtrxxglhjhfbmgykldyxzpplggpmtcbbajjzyljtyanjgbj"
                + "flqgdzyqcaxbkclecjsznslyzhlxlzcghbxzhznytdsbcjkdlzayffydlabbgqszkggldndnyskjshdlxxbcghxyggdj"
                + "mmzngmmccgwzszxsjbznmlzdthcqydbdllscddnlkjyhjsycjlkohqasdhnhcsgaehdaashtcplcpqybsdmpjlpcjaql"
                + "cdhjjasprchngjnlhlyyqyhwzpnccgwwmzffjqqqqxxaclbhkdjxdgmmydjxzllsygxgkjrywzwyclzmcsjzldbndcfc"
                + "xyhlschycjqppqagmnyxpfrkssbjlyxyjjglnscmhcwwmnzjjlhmhchsyppttxrycsxbyhcsmxjsxnbwgpxxtaybgajc"
                + "xlypdccwqocwkccsbnhcpdyznbcyytyckskybsqkkytqqxfcwchcwkelcqbsqyjqcclmthsywhmktlkjlychwheqjhtj"
                + "hppqpqscfymmcmgbmhglgsllysdllljpchmjhwljcyhzjxhdxjlhxrswlwzjcbxmhzqxsdzpmgfcsglsdymjshxpjxom"
                + "yqknmyblrthbcftpmgyxlchlhlzylxgsssscclsldclepbhshxyyfhbmgdfycnjqwlqhjjcywjztejjdhfblqxtqkwhd"
                + "chqxagtlxljxmsljhdzkzjecxjcjnmbbjcsfywkbjzghysdcpqyrsljpclpwxsdwejbjcbcnaytmgmbapclyqbclzxcb"
                + "nmsggfnzjjbzsfqyndxhpcqkzczwalsbccjxpozgwkybsgxfcfcdkhjbstlqfsgdslqwzkxtmhsbgzhjcrglyjbpmljs"
                + "xlcjqqhzmjczydjwbmjklddpmjegxyhylxhlqyqhkycwcjmyhxnatjhyccxzpcqlbzwwwtwbqcmlbmynjcccxbbsnzzl"
                + "jpljxyztzlgcldcklyrzzgqtgjhhgjljaxfgfjzslcfdqzlclgjdjcsnclljpjqdcclcjxmyzftsxgcgsbrzxjqqcczh"
                + "gyjdjqqlzxjyldlbcyamcstylbdjbyregklzdzhldszchznwczcllwjqjjjkdgjcolbbzppglghtgzcygezmycnqcycy"
                + "hbhgxkamtxyxnbskyzzgjzlqjdfcjxdygjqjjpmgwgjjjpkjsbgbmmcjssclpqpdxcdyykypcjddyygywchjrtgcnyql"
                + "dkljczzgzccjgdyksgpzmdlcphnjafyzdjcnmwescsglbtzcgmsdllyxqsxsbljsbbsgghfjlwpmzjnlyywdqshzxtyy"
                + "whmcyhywdbxbtlmswyyfsbjcbdxxlhjhfpsxzqhfzmqcztqcxzxrdkdjhnnyzqqfnqdmmgnydxmjgdhcdycbffallztd"
                + "ltfkmxqzdngeqdbdczjdxbzgsqqddjcmbkxffxmkdmcsychzcmljdjynhprsjmkmpcklgdbqtfzswtfgglyplljzhgjj"
                + "gypzltcsmcnbtjbhfkdhbyzgkpbbymtdlsxsbnpdkleycjnycdykzddhqgsdzsctarlltkzlgecllkjljjaqnbdggghf"
                + "jtzqjsecshalqfmmgjnlyjbbtmlycxdcjpldlpcqdhsycbzsckbzmsljflhrbjsnbrgjhxpdgdjybzgdlgcsezgxlblg"
                + "yxtwmabchecmwyjyzlljjshlgndjlslygkdzpzxjyyzlpcxszfgwyydlyhcljscmbjhblyjlycblydpdqysxktbytdkd"
                + "xjypcnrjmfdjgklccjbctbjddbblblcdqrppxjcglzcshltoljnmdddlngkaqakgjgyhheznmshrphqqjchgmfprxcjg"
                + "dychghlyrzqlcngjnzsqdkqjymszswlcfqjqxgbggxmdjwlmcrnfkkfsyyljbmqammmycctbshcptxxzzsmphfshmclm"
                + "ldjfyqxsdyjdjjzzhqpdszglssjbckbxyqzjsgpsxjzqznqtbdkwxjkhhgflbcsmdldgdzdblzkycqnncsybzbfglzzx"
                + "swmsccmqnjqsbdqsjtxxmbldxcclzshzcxrqjgjylxzfjphymzqqydfqjjlcznzjcdgzygcdxmzysctlkphtxhtlbjxj"
                + "lxscdqccbbqjfqzfsltjbtkqbsxjjljchczdbzjdczjccprnlqcgpfczlclcxzdmxmphgsgzgszzqjxlwtjpfsyaslcj"
                + "btckwcwmytcsjjljcqlwzmalbxyfbpnlschtgjwejjxxglljstgshjqlzfkcgnndszfdeqfhbsaqdgylbxmmygszldyd"
                + "jmjjrgbjgkgdhgkblgkbdmbylxwcxyttybkmrjjzxqjbhlmhmjjzmqasldcyxyqdlqcafywyxqhz";

                String _gbcode = GBCode.ToString();
                int pos = (Convert.ToInt16(_gbcode.Substring(0, 2)) - 56) * 94 + Convert.ToInt16(_gbcode.Substring(_gbcode.Length - 2, 2));
                return CodeData.Substring(pos - 1, 1);
            }
            return " ";
        }
        //获取汉字的首字母和全拼

        /// <summary>
        /// 获取汉字的全拼音
        /// </summary>
        /// <param name="x">传汉字的字符串</param>
        /// <returns>汉字的字符串的拼音</returns>
        public string GetSpell(string x)
        {
            int[] iA = new int[]
              {
                   -20319 ,-20317 ,-20304 ,-20295 ,-20292 ,-20283 ,-20265 ,-20257 ,-20242 ,-20230
                   ,-20051 ,-20036 ,-20032 ,-20026 ,-20002 ,-19990 ,-19986 ,-19982 ,-19976 ,-19805
                   ,-19784 ,-19775 ,-19774 ,-19763 ,-19756 ,-19751 ,-19746 ,-19741 ,-19739 ,-19728
                   ,-19725 ,-19715 ,-19540 ,-19531 ,-19525 ,-19515 ,-19500 ,-19484 ,-19479 ,-19467
                   ,-19289 ,-19288 ,-19281 ,-19275 ,-19270 ,-19263 ,-19261 ,-19249 ,-19243 ,-19242
                   ,-19238 ,-19235 ,-19227 ,-19224 ,-19218 ,-19212 ,-19038 ,-19023 ,-19018 ,-19006
                   ,-19003 ,-18996 ,-18977 ,-18961 ,-18952 ,-18783 ,-18774 ,-18773 ,-18763 ,-18756
                   ,-18741 ,-18735 ,-18731 ,-18722 ,-18710 ,-18697 ,-18696 ,-18526 ,-18518 ,-18501
                   ,-18490 ,-18478 ,-18463 ,-18448 ,-18447 ,-18446 ,-18239 ,-18237 ,-18231 ,-18220
                   ,-18211 ,-18201 ,-18184 ,-18183 ,-18181 ,-18012 ,-17997 ,-17988 ,-17970 ,-17964
                   ,-17961 ,-17950 ,-17947 ,-17931 ,-17928 ,-17922 ,-17759 ,-17752 ,-17733 ,-17730
                   ,-17721 ,-17703 ,-17701 ,-17697 ,-17692 ,-17683 ,-17676 ,-17496 ,-17487 ,-17482
                   ,-17468 ,-17454 ,-17433 ,-17427 ,-17417 ,-17202 ,-17185 ,-16983 ,-16970 ,-16942
                   ,-16915 ,-16733 ,-16708 ,-16706 ,-16689 ,-16664 ,-16657 ,-16647 ,-16474 ,-16470
                   ,-16465 ,-16459 ,-16452 ,-16448 ,-16433 ,-16429 ,-16427 ,-16423 ,-16419 ,-16412
                   ,-16407 ,-16403 ,-16401 ,-16393 ,-16220 ,-16216 ,-16212 ,-16205 ,-16202 ,-16187
                   ,-16180 ,-16171 ,-16169 ,-16158 ,-16155 ,-15959 ,-15958 ,-15944 ,-15933 ,-15920
                   ,-15915 ,-15903 ,-15889 ,-15878 ,-15707 ,-15701 ,-15681 ,-15667 ,-15661 ,-15659
                   ,-15652 ,-15640 ,-15631 ,-15625 ,-15454 ,-15448 ,-15436 ,-15435 ,-15419 ,-15416
                   ,-15408 ,-15394 ,-15385 ,-15377 ,-15375 ,-15369 ,-15363 ,-15362 ,-15183 ,-15180
                   ,-15165 ,-15158 ,-15153 ,-15150 ,-15149 ,-15144 ,-15143 ,-15141 ,-15140 ,-15139
                   ,-15128 ,-15121 ,-15119 ,-15117 ,-15110 ,-15109 ,-14941 ,-14937 ,-14933 ,-14930
                   ,-14929 ,-14928 ,-14926 ,-14922 ,-14921 ,-14914 ,-14908 ,-14902 ,-14894 ,-14889
                   ,-14882 ,-14873 ,-14871 ,-14857 ,-14678 ,-14674 ,-14670 ,-14668 ,-14663 ,-14654
                   ,-14645 ,-14630 ,-14594 ,-14429 ,-14407 ,-14399 ,-14384 ,-14379 ,-14368 ,-14355
                   ,-14353 ,-14345 ,-14170 ,-14159 ,-14151 ,-14149 ,-14145 ,-14140 ,-14137 ,-14135
                   ,-14125 ,-14123 ,-14122 ,-14112 ,-14109 ,-14099 ,-14097 ,-14094 ,-14092 ,-14090
                   ,-14087 ,-14083 ,-13917 ,-13914 ,-13910 ,-13907 ,-13906 ,-13905 ,-13896 ,-13894
                   ,-13878 ,-13870 ,-13859 ,-13847 ,-13831 ,-13658 ,-13611 ,-13601 ,-13406 ,-13404
                   ,-13400 ,-13398 ,-13395 ,-13391 ,-13387 ,-13383 ,-13367 ,-13359 ,-13356 ,-13343
                   ,-13340 ,-13329 ,-13326 ,-13318 ,-13147 ,-13138 ,-13120 ,-13107 ,-13096 ,-13095
                   ,-13091 ,-13076 ,-13068 ,-13063 ,-13060 ,-12888 ,-12875 ,-12871 ,-12860 ,-12858
                   ,-12852 ,-12849 ,-12838 ,-12831 ,-12829 ,-12812 ,-12802 ,-12607 ,-12597 ,-12594
                   ,-12585 ,-12556 ,-12359 ,-12346 ,-12320 ,-12300 ,-12120 ,-12099 ,-12089 ,-12074
                   ,-12067 ,-12058 ,-12039 ,-11867 ,-11861 ,-11847 ,-11831 ,-11798 ,-11781 ,-11604
                   ,-11589 ,-11536 ,-11358 ,-11340 ,-11339 ,-11324 ,-11303 ,-11097 ,-11077 ,-11067
                   ,-11055 ,-11052 ,-11045 ,-11041 ,-11038 ,-11024 ,-11020 ,-11019 ,-11018 ,-11014
                   ,-10838 ,-10832 ,-10815 ,-10800 ,-10790 ,-10780 ,-10764 ,-10587 ,-10544 ,-10533
                   ,-10519 ,-10331 ,-10329 ,-10328 ,-10322 ,-10315 ,-10309 ,-10307 ,-10296 ,-10281
                   ,-10274 ,-10270 ,-10262 ,-10260 ,-10256 ,-10254
                  };
            string[] sA = new string[]
          {
           "a","ai","an","ang","ao"

           ,"ba","bai","ban","bang","bao","bei","ben","beng","bi","bian","biao","bie","bin"
           ,"bing","bo","bu"

           ,"ca","cai","can","cang","cao","ce","ceng","cha","chai","chan","chang","chao","che"
           ,"chen","cheng","chi","chong","chou","chu","chuai","chuan","chuang","chui","chun"
           ,"chuo","ci","cong","cou","cu","cuan","cui","cun","cuo"

           ,"da","dai","dan","dang","dao","de","deng","di","dian","diao","die","ding","diu"
           ,"dong","dou","du","duan","dui","dun","duo"

           ,"e","en","er"

           ,"fa","fan","fang","fei","fen","feng","fo","fou","fu"

           ,"ga","gai","gan","gang","gao","ge","gei","gen","geng","gong","gou","gu","gua","guai"
           ,"guan","guang","gui","gun","guo"

           ,"ha","hai","han","hang","hao","he","hei","hen","heng","hong","hou","hu","hua","huai"
           ,"huan","huang","hui","hun","huo"

           ,"ji","jia","jian","jiang","jiao","jie","jin","jing","jiong","jiu","ju","juan","jue"
           ,"jun"

           ,"ka","kai","kan","kang","kao","ke","ken","keng","kong","kou","ku","kua","kuai","kuan"
           ,"kuang","kui","kun","kuo"

           ,"la","lai","lan","lang","lao","le","lei","leng","li","lia","lian","liang","liao","lie"
           ,"lin","ling","liu","long","lou","lu","lv","luan","lue","lun","luo"

           ,"ma","mai","man","mang","mao","me","mei","men","meng","mi","mian","miao","mie","min"
           ,"ming","miu","mo","mou","mu"

           ,"na","nai","nan","nang","nao","ne","nei","nen","neng","ni","nian","niang","niao","nie"
           ,"nin","ning","niu","nong","nu","nv","nuan","nue","nuo"

           ,"o","ou"

           ,"pa","pai","pan","pang","pao","pei","pen","peng","pi","pian","piao","pie","pin","ping"
           ,"po","pu"

           ,"qi","qia","qian","qiang","qiao","qie","qin","qing","qiong","qiu","qu","quan","que"
           ,"qun"

           ,"ran","rang","rao","re","ren","reng","ri","rong","rou","ru","ruan","rui","run","ruo"

           ,"sa","sai","san","sang","sao","se","sen","seng","sha","shai","shan","shang","shao","she"
           ,"shen","sheng","shi","shou","shu","shua","shuai","shuan","shuang","shui","shun","shuo","si"
           ,"song","sou","su","suan","sui","sun","suo"

           ,"ta","tai","tan","tang","tao","te","teng","ti","tian","tiao","tie","ting","tong","tou","tu"
           ,"tuan","tui","tun","tuo"

           ,"wa","wai","wan","wang","wei","wen","weng","wo","wu"

           ,"xi","xia","xian","xiang","xiao","xie","xin","xing","xiong","xiu","xu","xuan","xue","xun"

           ,"ya","yan","yang","yao","ye","yi","yin","ying","yo","yong","you","yu","yuan","yue","yun"

           ,"za","zai","zan","zang","zao","ze","zei","zen","zeng","zha","zhai","zhan","zhang","zhao"
           ,"zhe","zhen","zheng","zhi","zhong","zhou","zhu","zhua","zhuai","zhuan","zhuang","zhui"
           ,"zhun","zhuo","zi","zong","zou","zu","zuan","zui","zun","zuo"
          };
            byte[] B = new byte[2];
            string s = "";
            char[] c = x.ToCharArray();
            for (int j = 0; j < c.Length; j++)
            {
                B = System.Text.Encoding.Default.GetBytes(c[j].ToString());
                if ((int)(B[0]) <= 160 && (int)(B[0]) >= 0)
                {
                    s += c[j];
                }
                else
                {
                    for (int i = (iA.Length - 1); i >= 0; i--)
                    {
                        if (iA[i] <= (int)(B[0]) * 256 + (int)(B[1]) - 65536)
                        {
                            s += sA[i];
                            break;
                        }
                    }
                }
            }
            return s;
        }
    }


    #endregion
}
