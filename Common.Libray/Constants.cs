namespace Common.Library
{
    /// <summary>
    /// 定义常量
    /// </summary>
    public class Constants
    {
        #region 正则表达式
        /// <summary>
        /// Email正则表达式
        /// </summary>
        public const string REGEX_EMAIL = @"^((([a-zA-Z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-zA-Z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";

        /// <summary>
        /// 电话号码正则表达式
        /// </summary>
        public const string REGEX_TELEPHONE = @"^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}(-\d{1,4})?$|^400(-\d{3,4}){2}$|^400(-\d{3,4}){2}$";

        /// <summary>
        /// 手机号码正则表达式
        /// </summary>
        public const string REGEX_CELL_PHONE = "^(0|86|17951)?(13[0-9]|15[012356789]|18[0-9]|14[57]|17[0-9])[0-9]{8}$";

        /// <summary>
        /// 电话号码或者手机号码正则表达式
        /// </summary>
        public const string REGEX_TELEPHONE_OR_CELL_PHONE =
            @"^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}(-\d{1,4})?$|^(0|86|17951)?(13[0-9]|15[012356789]|18[0-9]|14[57]|17[0-9])[0-9]{8}$|^400(-\d{3,4}){2}$";

        //public const string REGEX_CELL_TELEORPHONE=@"(^(\d{3,4}-)?\d{7,8})$|(13[0-9]{9})";
        public const string REGEX_CELL_TELEORPHONE = @"^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}(-\d{1,4})?$|^(0|86|17951)?(13[0-9]|15[012356789]|18[0-9]|14[57]|17[0-9])[0-9]{8}$|^400(-\d{3,4}){2}$";

        /// <summary>
        /// 域名正则表达式
        /// </summary>
        public const string REGEX_DOMAIN = @"^[a-zA-Z][a-zA-Z\-\d]+[a-zA-Z\d]|(0|86|17951)?(13[0-9]|15[012356789]|18[0-9]|14[57]|17[0-9])[0-9]{8}$";

        /// <summary>
        /// URL地址正则表达式(包含http/https/ftp)
        /// </summary>
        public const string REGEX_URL_ADDRESS =
            @"^(https?|ftp):\/\/(((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$";

        /// <summary>
        /// URL地址正则表达式(不包含http/https/ftp)
        /// </summary>
        public const string REGEX_URL_ADDRESS_WITHOUT_SCHEME =
            @"^(((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-zA-Z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-zA-Z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-fA-F]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$";

        /// <summary>
        /// 邮政编码正则表达式
        /// </summary>
        public const string REGEX_POST_CODE = @"^[1-9]\d{5}$";

        /// <summary>
        /// 价格正则表达式
        /// </summary>
        public const string REGEX_PRICE = @"\d{1,10}(\.\d{1,2})?$";

        public const string REGEX_URL_MATCHS = @"(?<=>(\s+?)?)[a-zA-z]+://[^\s]*(?=</)";

        public const string REGEX_TEL_MATCHS = @"(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}(-\d{1,4})?";

        public const string REGEX_MOBLIE_MATCHS = @"(?<=>(\s+?)?)(0|86|17951)?(13[0-9]|15[012356789]|18[0-9]|14[57])[0-9]{8}(?=</)";

        /// <summary>
        /// 匹配除html标签外内容中的手机号或电话
        /// </summary>
        public const string REGEX_TELORMOBLIE_MATCHS = @"(?<=>(\s+?)?)(0|86|17951)?(13[0-9]|(15|17)[012356789]|18[0-9]|14[57])[0-9]{8}|(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}(-\d{1,4})?(?=</)";
        /// <summary>
        /// 分页正则表达式
        /// </summary>
        public const string REGEX_PAGEBREAK = @"_ueditor_page_break_tag_|<p(((?!>)(.))*)>(((?!(\[NT:PAGE=|</p>)).)*)\[NT:PAGE=(?<title>((?!(\$\]|</p>)).)*)?\$\](((?!</p>).)*)</p>|\[NT:PAGE=(?<title>((?!\$\]).)*)?\$\]";

        /// <summary>
        /// 在分页符中取出分页的标题
        /// </summary>
        public const string REGEX_PAGEBREAK_TITLE = @"_ueditor_page_break_tag_|<p(((?!>)(.))*)>(((?!(\[NT:PAGE=|</p>)).)*)\[NT:PAGE=(?<title>((?!(\$\]|</p>)).)*)?\$\](((?!</p>).)*)</p>|\[NT:PAGE=(?<title>((?!\$\]).)*)?\$\]";

        #endregion
    }
}
