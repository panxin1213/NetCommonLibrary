using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.SessionState;
using System.Text.RegularExpressions;
using System.IO;
using ChinaBM.Common;

namespace System.Web
{
    public class WebBase : Page, IRequiresSessionState
    {
        /// <summary>
        /// 静态化最后更新时间，和IsList,Suffix字段一起使用
        /// </summary>
        protected DateTime? StaticHtmlLastTime = null;

        /// <summary>
        /// 静态化是否是列表页，和StaticHtmlLastTime,Suffix字段一起使用
        /// </summary>
        protected bool? IsList = null;

        /// <summary>
        /// 静态化文件后缀，和IsList,StaticHtmlLastTime字段一起使用
        /// </summary>
        protected string Suffix = ".htm";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //排除ajax请求
            if (!HttpKit.IsAjax && HttpKit.ishtml && StaticHtmlLastTime != null && IsList != null)
            {
                HttpKit.MakeHtml(IsList.Value, Suffix, StaticHtmlLastTime);
            }

        }

        protected string EndAppendString = null;

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);

            if (!HttpKit.IsAjax)
            {
                TempMessage m = null;
                if (HttpContext.Current.Response.StatusCode != 200)
                {
                    return;
                }
                try
                {
                    m = TempData;
                }
                catch
                {
                    return;
                }
                var s = "";
                if (m != null)
                {
                    m.Message = m.Message.EnterToBr();
                    if (String.IsNullOrEmpty(m.Title))
                    {
                        s = String.Format("<script type=\"text/javascript\">$.dialog.tips('<h3>{0}</h3>',{1},'{2}.gif')</script>", m.Message, m.Time, m.Icon.ToString());
                    }
                    else
                    {
                        s = (String.Format("<script type=\"text/javascript\">$.dialog.tips('<h3>{3}</h3><p>{0}<p>',{1},'{2}.gif')</script>", m.Message, m.Time, m.Icon.ToString(), m.Title));
                    }
                }
                else
                {
                    return;
                }

                writer.Write(s);
                writer.Write(EndAppendString);
            }
        }

        public void MessageSuccess(string message, string title = null, int time = 3)
        {
            TempData = new TempMessage(TempMessageIcon.Success, message, title, time);
        }

        public void MessageError(string message, string title = "错误", int time = 3)
        {
            TempData = new TempMessage(TempMessageIcon.Error, message, title, time);
        }

        public void MessagePrompt(string message, string title = "提示", int time = 3)
        {
            TempData = new TempMessage(TempMessageIcon.Prompt, message, title, time);
        }

        public void MessageAlert(string message, string title = "提示", int time = 3)
        {
            TempData = new TempMessage(TempMessageIcon.Alert, message, title, time);
        }

        protected TempMessage TempData
        {
            get
            {
                var m = Session["M_TempData"] as TempMessage;
                Session.Remove("M_TempData");
                return m;
            }
            set
            {
                Session.Remove("M_TempData");
                Session.Add("M_TempData", value);

            }
        }

        #region Response.Redirect

        protected void RedirectUrlReferrer(string defaulturl = "/")
        {
            var url = "";

            try
            {
                url = Request.UrlReferrer.ToString();
            }
            catch
            {
                Response.Redirect(defaulturl, true);
            }

            if (String.IsNullOrEmpty(url))
            {
                Response.Redirect(defaulturl, true);
            }

            Response.Redirect(url, true);
        }

        protected void RedirectToBack(string defaulturl = "/")
        {
            var url = Request[HtmlTool.Hidden_Name];

            if (String.IsNullOrEmpty(url))
            {
                Response.Redirect(defaulturl, true);
            }

            Response.Redirect(url, true);
        }

        #endregion

        protected void Resonse404()
        {
            Response.StatusCode = 404;
            Response.Clear();
            Response.Write(new StreamReader(HttpKit.GetMapPath("/error/404.html")).ReadToEnd());
            Response.End();
            return;
        }

        /// <summary>
        /// 输出JS
        /// </summary>
        /// <param name="script">JS代码</param>
        protected void ResponseJavascript(string script)
        {
            Response.Write("<script type='text/javascript'>");
            Response.Write(script);
            Response.Write("</script>");
            Response.End();
        }
    }

    #region TempMessage 提示信息输出

    /// <summary>
    /// 提示消息类型
    /// </summary>
    public enum TempMessageIcon
    {
        Success,
        Alert,
        Error,
        Prompt
    }
    /// <summary>
    /// 消息模型
    /// </summary>
    [Serializable]
    public class TempMessage
    {
        public TempMessageIcon Icon { get; set; }
        public string Message { get; set; }
        public string Title { get; set; }
        private int _time = 3;
        public int Time
        {
            get
            {
                return _time;
            }
            set
            {
                _time = value;
            }
        }
        public TempMessage(TempMessageIcon icon, string message, string title, int time = 3)
        {
            Icon = icon;
            Message = message;
            Title = title;
            Time = 3;
        }
    }

    #endregion
}
