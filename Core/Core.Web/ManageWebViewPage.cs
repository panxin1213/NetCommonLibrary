using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc.Html;

namespace System.Web.Mvc
{
    public class ManageWebViewPage : BaseWebViewPage
    {
        public void RenderPartialSearch(List<Tuple<string, SelectList, string>> selectlist, List<Tuple<string, string>> timelist, bool isadd = true, bool isall = true, bool isdel = true, bool iskey = true)
        {
            ViewBag.SelectList = selectlist;
            ViewBag.TimeList = timelist;
            ViewBag.IsAdd = isadd;
            ViewBag.IsAll = isall;
            ViewBag.IsDel = isdel;
            ViewBag.IsKey = iskey;

            Html.RenderPartial("_Search");
        }

        public override void Execute()
        {

        }
    }

    public class ManageWebViewPage<T> : BaseWebViewPage<T>
    {
        public void RenderPartialSearch(List<Tuple<string, SelectList, string>> selectlist, List<Tuple<string, string>> timelist, bool isadd = true, bool isall = true, bool isdel = true, bool iskey = true)
        {
            ViewBag.SelectList = selectlist;
            ViewBag.TimeList = timelist;
            ViewBag.IsAdd = isadd;
            ViewBag.IsAll = isall;
            ViewBag.IsDel = isdel;
            ViewBag.IsKey = iskey;

            Html.RenderPartial("_Search");
        }

        public override void Execute()
        {
            
        }
    }
}
