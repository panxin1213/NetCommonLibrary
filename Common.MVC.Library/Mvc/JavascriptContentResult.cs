using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Web.Mvc
{
    /// <summary>
    /// 
    /// </summary>
    public class JavascriptContentResult :ActionResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string Content { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public JavascriptContentResult()
        { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        public JavascriptContentResult(string content)
        {
            Content = content;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            var response =context.HttpContext.Response;
            response.Write("<script type=\"text/javascript\">");
            response.Write(Content);
            response.Write("</script>");
        }
    }
}
