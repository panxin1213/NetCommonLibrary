using Base.Model;
using Common.Library.SiteMap;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Dapper;
using Dapper.Contrib.Extensions;
using Core.Base;
using System.Web.Mvc;
using Common.Library;
using System.Web.Routing;
using ChinaBM.Common;
using Common.Library.Log;
using System.Web;
using Core.Fenda.Service;

namespace Core.Web.Service
{
    /// <summary>
    /// 整站公共数据处理Service
    /// </summary>
    public class SiteMapService
    {

        private IDbConnection db = null;

        private UrlHelper Url = null;

        public SiteMapService(IDbConnection _db)
        {
            db = _db;
            Url = new UrlHelper(System.Web.HttpContext.Current != null ? System.Web.HttpContext.Current.Request.RequestContext : new RequestContext() { HttpContext = new HttpContextWrapper(new HttpContext(new HttpRequest("", "http://" + BaseConfig.Current.Routes["www"] + "/", ""), new HttpResponse(null))), RouteData = new RouteData() });
        }

        /// <summary>
        /// www域名SiteMap
        /// </summary>
        public void SaveMain()
        {
            try
            {
                var l = new List<URL>();

                DateTime? time = null;

                #region 首页及关于我们之类

                //增加首页
                l = new List<URL> { new URL { Loc = Url.AbsoluteAction("index", "home", new { area = "www" }), Lastmod = time.ToDateTime(DateTime.Now), Priority = "1.0", ChangeFreq = "always" } };

                var _asv = new Anchor_Service(db);
                l.AddRange(_asv.GetSiteMapList(0, 20000));

                #endregion

                SiteMapHelper.SaveSiteMap(l, HttpKit.GetMapPath("/SiteMap/www.xml"));
            }
            catch (Exception e)
            {
                Logger.Error(this, e.Message, e);
            }

        }

        /// <summary>
        /// 移动端
        /// </summary>
        public void SaveMobie()
        {
            try
            {
                var l = new List<URL>();

                DateTime? time = null;


                #region 资讯SiteMap

                var nlist = new News_Service(db).GetSiteMapList(0, 10000, true);

                var nl = nlist.Where(a => a.Loc.StartsWith("http://" + BaseConfig.Current.Routes["mobile"] + "/news/", StringComparison.OrdinalIgnoreCase)).ToList();

                var cdpagecount = nl.GroupBy(a => a.Additional).Where(a => News_Class_Service.All.Any(b => b.F_N_Class_Id == a.Key.ToInt())).ToDictionary(a => News_Class_Service.All.SingleOrDefault(b => a.Key.ToInt() == b.F_N_Class_Id), a =>
                {
                    var count = a.Count();
                    var pagecount = count % 20 > 0 ? (count / 20 + 1) : count / 20;
                    return pagecount > 0 ? pagecount : 1;
                });

                //获取最新一条信息
                var first = nl.FirstOrDefault();

                if (first != null)
                {
                    time = first.Lastmod.Value;
                }

                var pcount = nl.Count % 20 > 0 ? nl.Count / 20 + 1 : nl.Count / 20;

                //增加新闻首页
                for (var i = 1; i <= pcount; i++)
                {
                    l.Add(new URL { Loc = Url.AbsoluteAction("index", "news", new { area = "mobile", page = i }), Lastmod = time ?? DateTime.Now, Priority = "0.8", ChangeFreq = "always", IsMobile = true });
                }

                //增加新闻分类列表
                foreach (var item in cdpagecount)
                {
                    for (var i = 1; i <= item.Value; i++)
                    {
                        l.Add(new URL { Loc = Url.AbsoluteAction("list", "news", new { area = "mobile", id = item.Key.F_N_Class_Action, page = i }), Lastmod = first != null ? first.Lastmod : DateTime.Now, Priority = "0.8", ChangeFreq = "always", IsMobile = true });
                    }
                }

                //增加新闻详细
                l.AddRange(nl);

                #endregion

                #region 品牌SiteMap

                var _csv = new Company_Service(db);

                var cl = _csv.GetSiteMapList(0, 10000, true);

                //获取最新一条信息
                first = cl.FirstOrDefault();

                if (first != null && (time == null || first.Lastmod.ToDateTime(DateTime.Now) > time))
                {
                    time = first.Lastmod.Value;
                }

                #region 品牌列表

                var types = typeof(CompanyType).ToList().Select(a => a.Key);

                //品牌栏目列表
                foreach (var item in types)
                {
                    var count = _csv.GetTypeCount(item);
                    count = count == 0 ? 1 : count;
                    pcount = count % 20 > 0 ? count / 20 + 1 : count / 20;

                    for (var i = 1; i <= pcount; i++)
                    {
                        l.Add(new URL { Priority = "0.8", ChangeFreq = "always", Lastmod = time != null ? time.Value : DateTime.Now.Date, Loc = Url.AbsoluteAction("index", "pinpai", new { area = "mobile", id = item, page = i }), IsMobile = true });
                    }
                }

                #endregion

                #region 站内站页面

                var col = cl.Select(a =>
                {
                    var d = new List<URL>();

                    d.Add(a);

                    var m = a.CopyNewObject<URL>();
                    m.Loc = Url.AbsoluteAction("news", "co", new { area = "mobile", companyname = a.Additional.ToSafeString() });

                    d.Add(m);

                    m = a.CopyNewObject<URL>();
                    m.Loc = Url.AbsoluteAction("about", "co", new { area = "mobile", companyname = a.Additional.ToSafeString() });

                    d.Add(m);

                    m = a.CopyNewObject<URL>();
                    m.Loc = Url.AbsoluteAction("product", "co", new { area = "mobile", companyname = a.Additional.ToSafeString() });

                    d.Add(m);

                    m = a.CopyNewObject<URL>();
                    m.Loc = Url.AbsoluteAction("shop", "co", new { area = "mobile", companyname = a.Additional.ToSafeString() });

                    d.Add(m);

                    return d;
                }).Aggregate((a, b) => a.Concat(b).ToList());

                l.AddRange(col);

                #endregion

                #region 产品及商铺详细

                var pl = new Product_Service(db).GetSiteMapList(0, 10000, true);

                //获取最新一条信息
                first = pl.FirstOrDefault();

                if (first != null && (time == null || first.Lastmod.ToDateTime(DateTime.Now) > time))
                {
                    time = first.Lastmod.Value;
                }

                //增加产品首页
                l.Add(new URL { Loc = Url.AbsoluteAction("index", "product", new { area = "mobile" }), Lastmod = first != null ? first.Lastmod.ToDateTime(DateTime.Now) : DateTime.Now, Priority = "0.8", ChangeFreq = "always", IsMobile = true });

                //产品列表
                var plpagecount = pl.GroupBy(a => a.Additional).Where(a => Product_Class_Service.All.Any(b => b.F_P_Class_Id == a.Key.ToInt())).ToDictionary(a => Product_Class_Service.All.SingleOrDefault(b => a.Key.ToInt() == b.F_P_Class_Id), a =>
                {
                    var count = a.Count();
                    var pagecount = count % 20 > 0 ? (count / 20 + 1) : count / 20;
                    return pagecount > 0 ? pagecount : 1;
                });

                foreach (var item in plpagecount)
                {
                    for (var i = 1; i <= item.Value; i++)
                    {
                        l.Add(new URL { Loc = Url.AbsoluteAction("list", "product", new { area = "mobile", id = item.Key.F_P_Class_Action, page = 1 }), Lastmod = first != null ? first.Lastmod : DateTime.Now, Priority = "0.8", ChangeFreq = "always", IsMobile = true });
                    }
                }

                l.AddRange(pl);

                var sl = new Company_Shop_Service(db).GetSiteMapList(0, 10000, true);

                //获取最新一条信息
                first = sl.FirstOrDefault();

                if (first != null && (time == null || first.Lastmod.ToDateTime(DateTime.Now) > time))
                {
                    time = first.Lastmod.Value;
                }

                var spagecount = sl.Count % 50 > 0 ? sl.Count / 50 + 1 : sl.Count / 50;

                for (var i = 1; i <= (spagecount == 0 ? 1 : spagecount); i++)
                {
                    l.Add(new URL { Loc = Url.AbsoluteAction("index", "shop", new { area = "mobile", page = i }), Lastmod = first != null ? first.Lastmod.ToDateTime(DateTime.Now) : DateTime.Now, Priority = "0.8", ChangeFreq = "always", IsMobile = true });
                }

                l.AddRange(sl);

                #endregion

                //增加首页
                l = new List<URL> { new URL { Loc = Url.AbsoluteAction("index", "pinpai", new { area = "mobile" }), Lastmod = time.ToDateTime(DateTime.Now), Priority = "1.0", ChangeFreq = "always", IsMobile = true } }.Concat(l).ToList();


                #endregion

                #region 招商SiteMap


                #region 品牌招商列表及详情

                pcount = cl.Count % 20 > 0 ? cl.Count / 20 + 1 : cl.Count / 20;

                for (var i = 1; i <= pcount; i++)
                {
                    l.Add(new URL { Priority = "0.8", ChangeFreq = "always", Lastmod = time != null ? time.Value : DateTime.Now.Date, Loc = Url.AbsoluteAction("pinpai", "jiameng", new { area = "mobile", page = i }), IsMobile = true });
                }

                l.AddRange(cl.Select(a =>
                {
                    var m = a.CopyNewObject<URL>();
                    m.Loc = Url.AbsoluteAction("pinpaidetail", "jiameng", new { area = "mobile", companyname = m.Additional });
                    return m;
                }));

                #endregion

                #region 招商资讯列表及详细


                var anl = nlist.Where(a => a.Loc.StartsWith("http://" + BaseConfig.Current.Routes["mobile"] + "/jiameng/", StringComparison.OrdinalIgnoreCase)).ToList();

                cdpagecount = anl.GroupBy(a => a.Additional).Where(a => News_Class_Service.All.Any(b => b.F_N_Class_Id == a.Key.ToInt())).ToDictionary(a => News_Class_Service.All.SingleOrDefault(b => a.Key.ToInt() == b.F_N_Class_Id), a =>
                {
                    var count = a.Count();
                    var pagecount = count % 20 > 0 ? (count / 20 + 1) : count / 20;
                    return pagecount > 0 ? pagecount : 1;
                });

                //获取最新一条信息
                first = anl.FirstOrDefault();

                if (first != null && (time == null || first.Lastmod.ToDateTime(DateTime.Now) > time))
                {
                    time = first.Lastmod.Value;
                }

                //增加新闻分类列表
                foreach (var item in cdpagecount)
                {
                    for (var i = 1; i <= item.Value; i++)
                    {
                        l.Add(new URL { Loc = Url.AbsoluteAction(item.Key.F_N_Class_Controller, "jiameng", new { area = "mobile", page = i }), Lastmod = first != null ? first.Lastmod : DateTime.Now, Priority = "0.8", ChangeFreq = "always", IsMobile = true });
                    }
                }

                //增加新闻详细
                l.AddRange(anl);



                #endregion

                #region 代理信息列表及详细

                var al = new Agent_Service(db).GetSiteMapList(0, 10000, true);

                //获取最新一条信息
                first = al.FirstOrDefault();

                if (first != null && (time == null || first.Lastmod.ToDateTime(DateTime.Now) > time))
                {
                    time = first.Lastmod.Value;
                }

                pcount = al.Count % 20 > 0 ? al.Count / 20 + 1 : al.Count / 20;

                for (var i = 1; i <= pcount; i++)
                {
                    l.Add(new URL { Priority = "0.8", ChangeFreq = "always", Lastmod = time != null ? time.Value : DateTime.Now.Date, Loc = Url.AbsoluteAction("daili", "jiameng", new { area = "mobile", page = i }), IsMobile = true });
                }

                l.AddRange(al);

                l.Add(new URL { ChangeFreq = "Monthly", Lastmod = DateTime.Now.FirstDayOfMonth().Date, Priority = "0.6", Loc = Url.AbsoluteAction("zhaoshang", "jiameng", new { area = "mobile" }), IsMobile = true });


                //增加首页
                l = new List<URL> { new URL { Loc = Url.AbsoluteAction("index", "jiameng", new { area = "mobile" }), Lastmod = time.ToDateTime(DateTime.Now), Priority = "1.0", ChangeFreq = "always", IsMobile = true } }.Concat(l).ToList();


                #endregion

                #endregion

                //增加首页
                l = new List<URL> { new URL { Loc = Url.AbsoluteAction("index", "home", new { area = "mobile" }), Lastmod = time.ToDateTime(DateTime.Now), Priority = "1.0", ChangeFreq = "always", IsMobile = true } }.Concat(l).ToList();

                SiteMapHelper.SaveSiteMap(l, HttpKit.GetMapPath("/SiteMap/m.xml"));
            }
            catch (Exception e)
            {
                Logger.Error(this, e.Message, e);
            }
        }

    }
}
