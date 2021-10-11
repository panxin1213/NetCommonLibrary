using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web;
using Firm.Core.Admin.ManagePermissions;
using ChinaBM.Common;
using Firm.Core.Common.BLL.Pages;

namespace Firm.Core.Admin
{
    public abstract class ManagePage<T> : CommonBasePage<T> where T : class
    {
        protected Identity identity;

        protected Principal user;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void Page_Init()
        {
            var r = Manager.LoginValid();

            if (!r)
            {
                MessageError("未登陆，请登陆!");
                Response.Redirect("/manage/home/login.aspx?r_url=" + Request.Url);
                return;
            }
            r = Manager.RoleRightValid(this.GetType().BaseType.Namespace, this.GetType().BaseType.Name);

            identity = System.Web.HttpContext.Current.GetManage();

            user = System.Web.HttpContext.Current.User as Principal;

            if (!r)
            {
                MessageError("您没有该权限!");
                Response.Redirect("/manage/home/welecome.aspx");
                return;
            }

            ManagePageInit();
        }

        protected abstract void ManagePageInit();

        /// <summary>
        /// Get调用抽象方法
        /// </summary>
        protected override abstract void GetAction();

        /// <summary>
        /// Post调用抽象方法
        /// </summary>
        protected override abstract void PostAction();


        protected sealed override void OnPostRequest()
        {
            base.OnPostRequest();
        }

        protected sealed override void OnGetRequest()
        {
            base.OnGetRequest();
        }
    }

    public abstract class ManagePage : CommonBasePage
    {
        protected Identity identity;

        protected Principal user;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected abstract void PageInit();

        protected abstract void GetAction();

        protected abstract void PostAction();


        protected override void Page_Init()
        {
            var r = Manager.LoginValid();

            if (!r)
            {
                MessageError("未登陆，请登陆!");
                Response.Redirect("/manage/home/login.aspx?r_url=" + Request.Url);
                return;
            }
            var namespaces = this.GetType().BaseType.Namespace;
            var classname = this.GetType().BaseType.Name;

            if (!namespaces.StartsWith("Firm.Web", StringComparison.OrdinalIgnoreCase))
            {
                namespaces = this.GetType().Namespace;
                classname = this.GetType().Name;
            }

            r = Manager.RoleRightValid(namespaces, classname);

            if (!r)
            {
                MessageError("您没有该权限!");
                Response.Redirect("/manage/home/welecome.aspx");
                return;
            }
            identity = System.Web.HttpContext.Current.GetManage();

            user = System.Web.HttpContext.Current.User as Principal;
            PageInit();
        }

        protected sealed override void OnGetRequest()
        {
            GetAction();
        }

        protected sealed override void OnPostRequest()
        {
            PostAction();
        }
    }
}
