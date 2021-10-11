namespace System.Web.Mvc
{
    public static class UrlHelperEx
    {
        public static string AbsoluteUrl(this UrlHelper urlHelper, string relativeUrl, string area = "www")
        {
            return UrlUtility.ToAbsolute(relativeUrl, new {area});
        }
    }
}
