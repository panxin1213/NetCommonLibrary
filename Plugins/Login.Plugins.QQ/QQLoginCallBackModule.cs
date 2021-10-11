using ChinaBM.Common;
using Login.Plugins.QQ.Section;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Login.Plugins.QQ
{
    /// <summary>
    /// 授权module
    /// </summary>
    public class QQLoginCallBackModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.AcquireRequestState += (a, b) =>
            {
                var Response = context.Response;
                var Request = context.Request;
                var query = Request.Url.Query;

                if (!String.IsNullOrEmpty(query) && Request.Url.ToSafeString().Replace(Request.Url.Query, "").Equals(QQConfigSection.Current.AuthUrl.Value, StringComparison.OrdinalIgnoreCase))
                {
                    var code = HttpKit.GetUrlParam("code", true).ToSafeString();
                    var state = HttpKit.GetUrlParam("state", true).ToSafeString();

                    if (QQConfigSection.Current.Server.Value.Equals(state))
                    {
                        var accessToken = QQLoginHelper.GetAccessToken(code);
                        var openid = QQLoginHelper.GetOpenId(accessToken.AccessToken);

                        if (String.IsNullOrEmpty(openid))
                        {
                            QQLoginHelper.configoperation.ReturnErrorPage("授权失败,请联系客服");
                        }
                        else
                        {
                            QQLoginHelper.configoperation.AuthSuccessCallBack(openid, accessToken);
                        }
                    }
                }
            };
        }
    }
}
