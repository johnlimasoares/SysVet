using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Antlr.Runtime.Misc;
using SisVetWeb.Models;

namespace SisVetWeb.Helpers {
    public static class MyHelper {

        // As the text the: "<span class='glyphicon glyphicon-plus'></span>" can be entered
        public static MvcHtmlString NoEncodeActionLink(this HtmlHelper htmlHelper,
            string text, string title, string action,
            string controller,
            object routeValues = null,
            object htmlAttributes = null) {


            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            TagBuilder builder = new TagBuilder("a");
            builder.InnerHtml = text;
            builder.Attributes["title"] = title;
            builder.Attributes["href"] = urlHelper.Action(action, controller, routeValues);
            builder.MergeAttributes(new RouteValueDictionary(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));

            return MvcHtmlString.Create(builder.ToString());
        }

        public static MvcHtmlString PageLinks(this HtmlHelper html, Paginacao paginacao, Func<int, string> paginaUrl) {
            var builder = new StringBuilder();
            for (int pagina = 0; pagina <= paginacao.TotalPaginas; pagina++) {
                var tagBuilder = new TagBuilder("a");
                tagBuilder.MergeAttribute("href", paginaUrl(pagina));
                tagBuilder.InnerHtml = pagina.ToString();
                if (pagina == paginacao.PaginaAtual){
                    tagBuilder.AddCssClass("selected");
                    tagBuilder.AddCssClass("btn-primary");

                }
                tagBuilder.AddCssClass("btn btn-default");
                builder.Append(tagBuilder);
            }

            return MvcHtmlString.Create(builder.ToString());
        }
    }
}