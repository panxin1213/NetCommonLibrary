using System.Globalization;
using System.Web.Helpers;
using System.Web.Mvc.Html;
using System.Web.Routing;


namespace System.Web.Mvc
{
    /// <summary>
    /// 控制器扩展
    /// </summary>
    public static class ControllerEx
    {
        #region RedirectLocal 重定向到本站地址
        /// <summary>
        /// 重定向到本站地址
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="redirectUrl">URL</param>
        /// <param name="invalidUrlBehavior">
        /// 参数用法 controller.RedirectLocal("aa", () =>new RedirectToAction("list"));
        /// 参数必须返回ActionResult
        /// </param>
        /// <returns></returns>
        public static ActionResult RedirectToLocal(this ControllerBase controller, string redirectUrl, Func<ActionResult> invalidUrlBehavior)
        {
            
            if (!string.IsNullOrWhiteSpace(redirectUrl) && DomainHelper.IsLocalDoamin(redirectUrl))
            {
                return new RedirectResult(redirectUrl);
            }
            return invalidUrlBehavior != null ? invalidUrlBehavior() : null;
        }
        /// <summary>
        /// 重定向到本站地址，如果不是本站地址 则定向到 /
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="redirectUrl"></param>
        /// <returns></returns>
        public static ActionResult RedirectToLocal(this ControllerBase controller, string redirectUrl)
        {
            return RedirectToLocal(controller, redirectUrl, (string)null);
        }
        /// <summary>
        /// 重定向到本站地址
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="redirectUrl">地址</param>
        /// <param name="defaultUrl">如果redirectUrl不是本站 重定向到defaultUrl</param>
        /// <returns></returns>
        public static ActionResult RedirectToLocal(this ControllerBase controller, string redirectUrl, string defaultUrl)
        {

            if (DomainHelper.IsLocalDoamin(redirectUrl))
            {
                return new RedirectResult(redirectUrl);
            }
            return new RedirectResult(defaultUrl ?? "/");
        }
        #endregion

        #region RedirectToLocalReferer 重定向到来路
        /// <summary>
        /// 重定向到本站的来路地址
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="defaultUrl">如果没有来路或不是本站，定向到defaultUrl</param>
        /// <returns></returns>
        public static ActionResult RedirectToLocalReferer(this ControllerBase controller, 
            string defaultUrl = null)
        {
            string url = controller.ControllerContext.HttpContext.Request.ServerVariables["HTTP_REFERER"];
            return RedirectToLocal(controller, url, defaultUrl);
        }
        /// <summary>
        /// 重定向到本站的来路地址
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="invalidUrlBehavior">
        /// 如果不是本站或没有来路
        /// 重定向到 该参数指定的地址
        /// 用法 controller.RedirectToLocalReferer(() =>new RedirectToAction("list"));
        /// 参数必须返回ActionResult
        /// </param>
        /// <returns></returns>
        public static ActionResult RedirectToLocalReferer(this ControllerBase controller, Func<ActionResult> invalidUrlBehavior)
        {
            string url = controller.ControllerContext.HttpContext.Request.ServerVariables["HTTP_REFERER"];
            return RedirectToLocal(controller, url, invalidUrlBehavior);
        }
        #endregion

        #region javascript重定向
        /// <summary>
        /// javascript重定向的目标 Enum
        /// </summary>
        public enum JSRedirectTarget {
            /// <summary>
            /// 当前页
            /// </summary>
            SELF,
            /// <summary>
            /// 顶层页
            /// </summary>
            TOP,
            /// <summary>
            /// 父级页
            /// </summary>
            PARENT
        }
        /// <summary>
        /// 用JS延迟跳转
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="url">跳转地址</param>
        /// <param name="timeout">时间</param>
        /// <param name="msg">提示</param>
        /// <param name="target">目标</param>
        /// <param name="viewName">视图名</param>
        /// <returns></returns>
        public static ActionResult JSRedirect(this ControllerBase controller, string url, int timeout, string msg = null, JSRedirectTarget target = JSRedirectTarget.SELF, string viewName = null)
        {
            var v = new ViewResult();

            controller.ViewBag.TimeOut = timeout;
            controller.ViewBag.Message = msg;
            controller.ViewBag.Url = url;
            controller.ViewBag.Target = target.ToString().ToLower();
            v.ViewData = controller.ViewData;
            v.TempData = controller.TempData;

            /*Views\Shared\JSRedirect\文件夹下*/
            v.ViewName = "JSRedirect/" + (viewName ?? "default");
            return v;
        }
        #endregion

        #region 重定向到"修改"前的地址
        /// <summary>
        /// 重定向到"修改"前的地址，结合Common.Library.Mvc.Html.FormBackButtonEx.FormBackButton方法
        /// 也就是页面上的form里必须有 @Html.FormBackButton(...)生成的按钮.它会自动维护当前修改前的地址，
        /// get 后取 UrlReferrer,POST后取hidden里的值
        /// </summary>
        /// <param name="c"></param>
        /// <param name="defaulturl"></param>
        /// <returns></returns>
        public static ActionResult RedirectToBackUrl(this ControllerBase c, string defaulturl = null)
        {
            var v = c.ControllerContext.HttpContext.Request[FormBackButtonEx.Hidden_Name];

            //defaulturl = defaulturl ?? UrlHelper.GenerateUrl(null, "index",null,c.ControllerContext.RouteData.DataTokens,RouteTable.Routes,c.ControllerContext.RequestContext,true);
            defaulturl = defaulturl ?? c.ControllerContext.HttpContext.Request.UrlReferrer.ToString();
            return new RedirectResult(string.IsNullOrEmpty(v)?defaulturl:v);
        }
        #endregion

        public static JavascriptContentResult JavascriptContent(this ControllerBase c, string js)
        {
            return new JavascriptContentResult(js);
        }

        public static ActionResult View(this ControllerBase c, object model, bool isRequireModel)
        {
            if (isRequireModel && model == null)
            {
                c.ControllerContext.HttpContext.Response.StatusCode = 404;
                return new ContentResult() { Content = "您访问的页面已删除或页面不存在" };
            }
            c.ViewData.Model = model;
            return new ViewResult()
            {
                ViewData = c.ViewData,
                TempData = c.TempData,

            };
        }

        /// <summary>
        /// 向HTTP响应头添加Last-Modified和ETag
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="lastModifiedTime">上次修改时间</param>
        /// <param name="actionResult">需要返回的ActionResult</param>
        /// <returns></returns>
        public static ActionResult HttpNotModified(this ControllerBase controller, DateTime lastModifiedTime, ActionResult actionResult)
        {
            var request = controller.ControllerContext.HttpContext.Request;
            var response = controller.ControllerContext.HttpContext.Response;
            // 根据上次修改时间生成整点时间
            var lastModifiedOnTheHourTime = new DateTime(lastModifiedTime.Year, lastModifiedTime.Month,
                lastModifiedTime.Day, lastModifiedTime.Hour, 0, 0);
            // 生成Last-Modified字符串
            var lastModified = lastModifiedOnTheHourTime.ToUniversalTime().ToString("R");
            // 计算ETag
            var etag =
                Math.Abs(string.Concat(request.GetFullUrl(), lastModifiedOnTheHourTime).GetHashCode())
                    .ToString(CultureInfo.InvariantCulture);

            var ifModifiedSince = request.Headers["If-Modified-Since"];
            var ifNoneMatch = request.Headers["If-None-Match"];

            if (lastModified.Equals(ifModifiedSince) || etag.Equals(ifNoneMatch))
            {
                return new HttpStatusCodeResult(304);
            }
            response.Headers.Add("Last-Modified", lastModified);
            response.Headers.Add("ETag", etag);

            return actionResult;
        }

        /// <summary>
        /// 向HTTP响应头添加Last-Modified和ETag
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="period">ETag的生成周期(小时)</param>
        /// <param name="actionResult">需要返回的ActionResult</param>
        /// <returns></returns>
        public static ActionResult HttpNotModified(this ControllerBase controller, int period, ActionResult actionResult)
        {
            return HttpNotModified(controller,
                DateTime.MinValue.AddHours(DateTime.Now.DateDiff(DateDiffType.Hour, DateTime.MinValue)/period*period), actionResult);
        }

        /// <summary>
        /// 以JSON格式返回操作成功的消息
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ActionResult JsonSuccess(this ControllerBase controller, string message, object data = null) {
            ActionResult jsonResult;
            if (data == null)
                jsonResult = new JsonResult {
                    Data = new {
                        code = CodeEx.成功.ToInt(),
                        msg = message
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            else
                jsonResult = new JsonResult {
                    Data = new {
                        code = CodeEx.成功.ToInt(),
                        msg = message,
                        data
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

            return jsonResult;
        }

        /// <summary>
        /// 以JSON格式返回操作失败的消息
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="message"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ActionResult JsonFailure(this ControllerBase controller, string message, object data = null) {
            ActionResult jsonResult;
            if (data == null)
                jsonResult = new JsonResult {
                    Data = new {
                        code = CodeEx.失败.ToInt(),
                        msg = message
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            else
                jsonResult = new JsonResult {
                    Data = new {
                        code = CodeEx.失败.ToInt(),
                        msg = message,
                        data
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

            return jsonResult;
        }
        /// <summary>
        /// 以JSON格式返回操作失败的消息
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="message"></param>
        /// <param name="codeEx"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static ActionResult JsonFailure(this ControllerBase controller, string message, CodeEx codeEx, object data = null)
        {
            ActionResult jsonResult;
            if (data == null)
                jsonResult = new JsonResult
                {
                    Data = new
                    {
                        code = codeEx.ToInt(),
                        msg = message
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            else
                jsonResult = new JsonResult
                {
                    Data = new
                    {
                        code = codeEx.ToInt(),
                        msg = message,
                        data
                    },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };

            return jsonResult;
        }
    }
}
