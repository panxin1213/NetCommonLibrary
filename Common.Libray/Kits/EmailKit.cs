using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.IO;
using System.Xml;

namespace ChinaBM.Common
{
    public class EmailKit
    {
        /// <summary>
        /// 发邮件
        /// </summary>
        /// <param name="mailConfig">邮件信息</param>
        public static void SendEmail(MailEntity mailConfig)
        {
            try
            {
                //编码
                Encoding encoding = Encoding.GetEncoding(mailConfig.EnCode);
                MailMessage Message = new MailMessage();
                //发信人
                Message.From = new MailAddress(mailConfig.EmailName, mailConfig.DisplayName);
                //收信人
                foreach (string str in mailConfig.ToEmailName)
                {
                    Message.To.Add(new MailAddress(str));
                }
                //编码
                Message.SubjectEncoding = encoding;
                //标题
                Message.Subject = mailConfig.Title;
                //编码
                Message.BodyEncoding = encoding;
                //是否html
                Message.IsBodyHtml = mailConfig.IsBodyHtml;
                //主体
                Message.Body = mailConfig.Content;
                //添加附件
                foreach (string str in mailConfig.AttachmentStrs)
                {
                    Message.Attachments.Add(new Attachment(str));
                }
                //抄送
                foreach (string str in mailConfig.CCs)
                {
                    Message.CC.Add(str);
                }
                SmtpClient smtpClient = new SmtpClient(mailConfig.SmtpClient);//信箱服务器
                smtpClient.Credentials = new NetworkCredential(mailConfig.EmailName, mailConfig.EmailPwd);//信箱的用户名和密码
                smtpClient.Send(Message);
                Message.Dispose();
            }
            catch (Exception ex)
            {
                string path = HttpKit.GetMapPath("/error.txt");
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.GetEncoding("GB2312"));//通过指定字符编码方式可以实现对汉字的支持，否则在用记事本打开查看会出现乱码 
                sw.Flush();
                sw.BaseStream.Seek(0, SeekOrigin.End);   //从哪里开始写入.
                sw.WriteLine(ex.Message);
                sw.Flush();
                sw.Close();
                fs.Close();

            }
        }

    }

    #region MailEntity
    /// <summary>
    /// MailEntity
    /// </summary>
    public class MailEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public MailEntity(string[] toEmailName)
        {
            XmlDocumentExtender xmldocument = new XmlDocumentExtender();
            xmldocument.Load(HttpKit.GetMapPath("/Manage/param/param.xml"));
            XmlNode root = xmldocument.GetElementsByTagName("root")[0];

            this.Content = root.SelectSingleNode("MailContent").InnerXml;
            this.DisplayName = string.Empty;
            this.EmailName = root.SelectSingleNode("EmailName").InnerXml;
            this.EmailPwd = root.SelectSingleNode("EmailPwd").InnerXml;
            this.EnCode = "gbk";
            this.IsBodyHtml = true;
            this.SmtpClient = root.SelectSingleNode("SmtpClient").InnerXml;
            this.Title = root.SelectSingleNode("MailTitle").InnerXml;
            this.ToEmailName = toEmailName;
        }


        private string emailName = string.Empty;
        private string emailPwd = string.Empty;
        private string enCode = string.Empty;
        private string displayName = string.Empty;
        private string smtpClient = string.Empty;
        private bool isBodyHtml = false;
        private string[] toEmailName = { };
        private string[] ccs = { };
        private string[] attachmentStrs = { };
        private string title = string.Empty;
        private string content = string.Empty;

        /// <summary>
        /// 正文
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }


        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// 附件
        /// </summary>
        public string[] AttachmentStrs
        {
            get { return attachmentStrs; }
            set { attachmentStrs = value; }
        }

        /// <summary>
        /// 抄送
        /// </summary>
        public string[] CCs
        {
            get { return ccs; }
            set { ccs = value; }
        }

        /// <summary>
        /// 收件人
        /// </summary>
        public string[] ToEmailName
        {
            get { return toEmailName; }
            set { toEmailName = value; }
        }

        /// <summary>
        /// 是否为html编码
        /// </summary>
        public bool IsBodyHtml
        {
            get { return isBodyHtml; }
            set { isBodyHtml = value; }
        }
        /// <summary>
        /// 邮箱用户名
        /// </summary>
        public string EmailName
        {
            get { return emailName; }
            set { emailName = value; }
        }
        /// <summary>
        /// 邮箱密码
        /// </summary>
        public string EmailPwd
        {
            get { return emailPwd; }
            set { emailPwd = value; }
        }
        /// <summary>
        /// 字符编码
        /// </summary>
        public string EnCode
        {
            get { return enCode; }
            set { enCode = value; }
        }
        /// <summary>
        /// 别名
        /// </summary>
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }
        /// <summary>
        /// 信箱服务器
        /// </summary>
        public string SmtpClient
        {
            get { return smtpClient; }
            set { smtpClient = value; }
        }
    }
    #endregion
}
