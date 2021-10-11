using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.WebPages;
using System.Diagnostics;
using System.IO;

namespace System.Web.Mvc
{
    public static class TempDataDictionaryEx
    {
        /// <summary>
        /// 记录一个待显示的成功消息
        /// </summary>
        /// <param name="tempdata"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="time"></param>
        public static void MessageSuccess(this TempDataDictionary tempdata, string message, string title = null, int time = 3)
        {
            tempdata.Message(message, TempMessageIcon.Success, title, time);
        }
        /// <summary>
        /// 记录一个待显示的提问消息
        /// </summary>
        /// <param name="tempdata"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="time"></param>
        public static void MessagePrompt(this TempDataDictionary tempdata, string message, string title = "提示", int time = 3)
        {
            tempdata.Message(message, TempMessageIcon.Prompt, title, time);
        }
        /// <summary>
        /// 记录一个待显示的错误消息
        /// </summary>
        /// <param name="tempdata"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="time"></param>
        public static void MessageError(this TempDataDictionary tempdata, string message, string title = "错误", int time = 3)
        {
            tempdata.Message(message, TempMessageIcon.Error, title, time);
        }
        /// <summary>
        /// 记录一个待显示的提示消息
        /// </summary>
        /// <param name="tempdata"></param>
        /// <param name="message"></param>
        /// <param name="title"></param>
        /// <param name="time"></param>
        public static void MessageAlert(this TempDataDictionary tempdata, string message, string title = "提示", int time = 3)
        {
            tempdata.Message(message, TempMessageIcon.Alert, title, time);
        }
        /// <summary>
        /// 记录一个待返回的消息
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="message">消息内容</param>
        /// <param name="icon">图标</param>
        /// <param name="title">消息标题</param>
        /// <param name="time">显示秒</param>
        public static void Message(this TempDataDictionary tempdata, string message, TempMessageIcon icon = TempMessageIcon.Success, string title = null, int time = 3)
        {
            tempdata[Guid.NewGuid().ToString("N")] = new TempMessage(icon, message, title, time);
        }
    }

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
}
namespace System.Web.Mvc
{
    public abstract class BMWebViewPage<T> : WebViewPage<T>
    {

        /// <summary>
        /// 记录是否最外部页面，没找到好办法
        /// </summary>
        bool x = false;
        static Type stringType = typeof(string);
        static Type messageType = typeof(TempMessage);
        protected override void ConfigurePage(WebPageBase parentPage)
        {
            x = true;
            base.ConfigurePage(parentPage);
        }

        public override void ExecutePageHierarchy()
        {
            base.ExecutePageHierarchy();

            if (x || ViewData.TemplateInfo.TemplateDepth == 0) //如果是最外部的页面。在最后添加tempdata提示
            {
                if (TempData.Count > 0)
                {

                    foreach (var v in TempData)
                    {
                        var type = v.Value.GetType();
                        if (v.Value != null && stringType.IsAssignableFrom(type))
                        {
                            WriteLiteral(String.Format("<script type=\"text/javascript\">$.dialog.tips('<h3>{0}</h3>',3,'success.gif')</script>", v.Value));
                        }
                        else if (messageType.IsAssignableFrom(type))
                        {
                            var m = v.Value as TempMessage;
                            if (String.IsNullOrEmpty(m.Title))
                            {
                                WriteLiteral(String.Format("<script type=\"text/javascript\">$.dialog.tips('<h3>{0}</h3>',{1},'{2}.gif')</script>", m.Message, m.Time, m.Icon.ToString()));
                            }
                            else
                            {
                                WriteLiteral(String.Format("<script type=\"text/javascript\">$.dialog.tips('<h3>{3}</h3><p>{0}<p>',{1},'{2}.gif')</script>", m.Message, m.Time, m.Icon.ToString(), m.Title));
                            }
                        }
                    }

                }
            }

        }
    }
    public abstract class BMWebViewPage : WebViewPage
    {
        /// <summary>
        /// 记录是否最外部页面，没找到好办法
        /// </summary>
        bool x = false;
        static Type stringType = typeof(string);
        static Type messageType = typeof(TempMessage);
        protected override void ConfigurePage(WebPageBase parentPage)
        {
            x = true;
            base.ConfigurePage(parentPage);
        }

        public override void ExecutePageHierarchy()
        {
            base.ExecutePageHierarchy();

            if (x || ViewData.TemplateInfo.TemplateDepth == 0) //如果是最外部的页面。在最后添加tempdata提示
            {
                if (TempData.Count > 0)
                {

                    foreach (var v in TempData)
                    {
                        var type = v.Value.GetType();
                        if (v.Value != null && stringType.IsAssignableFrom(type))
                        {
                            WriteLiteral(String.Format("<script type=\"text/javascript\">$.dialog.tips('<h3>{0}</h3>',3,'success.gif')</script>", v.Value));
                        }
                        else if (messageType.IsAssignableFrom(type))
                        {
                            var m = v.Value as TempMessage;
                            if (String.IsNullOrEmpty(m.Title))
                            {
                                WriteLiteral(String.Format("<script type=\"text/javascript\">$.dialog.tips('<h3>{0}</h3>',{1},'{2}.gif')</script>", m.Message, m.Time, m.Icon.ToString()));
                            }
                            else
                            {
                                WriteLiteral(String.Format("<script type=\"text/javascript\">$.dialog.tips('<h3>{3}</h3><p>{0}<p>',{1},'{2}.gif')</script>", m.Message, m.Time, m.Icon.ToString(), m.Title));
                            }
                        }
                    }

                }
            }

        }



    }
}
