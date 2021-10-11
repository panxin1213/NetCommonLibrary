using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc.Html;

namespace System.Web.Mvc
{
    public static class DropDownListEx
    {

        public static MvcHtmlString DropDownListXMLPathFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string xmlpath)
        {
            return DropDownListXMLPathFor(html, expression, xmlpath, null);
        }

        public static MvcHtmlString DropDownListXMLPathFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string xmlpath, object htmlAttributes)
        {
            var SysFields = new List<string>();
            try
            {
                var filepath = xmlpath;

                var document = System.Xml.Linq.XElement.Load(filepath);

                SysFields = document.Descendants("item").Select(a => a.Value).ToList();
            }
            catch
            {
                throw;
            }
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            return html.DropDownListFor(expression, new SelectList(SysFields, metadata.Model), "-请选择-", htmlAttributes);
        }
    }
}
