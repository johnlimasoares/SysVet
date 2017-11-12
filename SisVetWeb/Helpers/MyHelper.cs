using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using Domain.EntidadesLeitura.Operacao.Financeiro;
using Domain.Enum;
using SisVetWeb.Models;

namespace SisVetWeb.Helpers
{
    public static class MyHelper
    {

        // As the text the: "<span class='glyphicon glyphicon-plus'></span>" can be entered
        public static MvcHtmlString NoEncodeActionLink(this HtmlHelper htmlHelper,
            string text, string title, string action,
            string controller,
            object routeValues = null,
            object htmlAttributes = null)
        {


            UrlHelper urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            TagBuilder builder = new TagBuilder("a");
            builder.InnerHtml = text.SetTituloBotaoNovoCadastro();
            builder.Attributes["title"] = title;
            builder.Attributes["href"] = urlHelper.Action(action, controller, routeValues);
            builder.MergeAttributes(new RouteValueDictionary(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes)));

            return MvcHtmlString.Create(builder.ToString());
        }

        private static string SetTituloBotaoNovoCadastro(this string value)
        {
            if (value.Contains("glyphicon-plus"))
            {
                return value.Replace("><", "> Adicionar<");
            }
            return value;
        }

        public static MvcHtmlString PageLinks(this HtmlHelper html, Paginacao paginacao, Antlr.Runtime.Misc.Func<int, string> paginaUrl)
        {
            var builder = new StringBuilder();
            for (int pagina = 0; pagina <= paginacao.TotalPaginas; pagina++)
            {
                var tagBuilder = new TagBuilder("a");
                tagBuilder.MergeAttribute("href", paginaUrl(pagina));
                tagBuilder.InnerHtml = pagina.ToString();
                if (pagina == paginacao.PaginaAtual)
                {
                    tagBuilder.AddCssClass("selected");
                    tagBuilder.AddCssClass("btn-primary");

                }
                tagBuilder.AddCssClass("btn btn-default");
                builder.Append(tagBuilder);
            }

            return MvcHtmlString.Create(builder.ToString());
        }

        public static string GetColorColumnDataVencimento(FinanceiroContasReceberParcelasDapper parcela)
        {
            var isVencida = parcela.DataVencimento < DateTime.Now && parcela.SituacaoParcelaFinanceira == SituacaoParcelaFinanceira.Aberto;
            return isVencida ? "Color:red" : "";
        }

        public static string GetColorColumnDataRecebimento(FinanceiroContasReceberParcelasDapper parcela)
        {
            return !string.IsNullOrEmpty(GetDataFormatada(parcela.DataRecebimento)) ? "Color:green" : "";
        }

        public static string GetDataFormatada(DateTime data)
        {
            return data.ToString() == "01/01/0001 00:00:00" ? string.Empty : data.ToString("d");
        }

        public static string GetStatusBotaoBaixarParcela(SituacaoParcelaFinanceira situacaoParcelaFinanceira)
        {
            if (situacaoParcelaFinanceira == SituacaoParcelaFinanceira.Liquidado || situacaoParcelaFinanceira == SituacaoParcelaFinanceira.Cancelado)
                return "btn btn-xs btn-success disabled";

            return "btn btn-xs btn-success";

        }

        public static string GetStatusBotaoCancelarBaixa(SituacaoParcelaFinanceira situacaoParcelaFinanceira)
        {
            if (situacaoParcelaFinanceira == SituacaoParcelaFinanceira.Liquidado)
                return "btn btn-xs btn-warning";

            return "btn btn-xs btn-warning disabled";

        }

        public static string GetStatusBotaoCancelarParcela(SituacaoParcelaFinanceira situacaoParcelaFinanceira)
        {
            return situacaoParcelaFinanceira == SituacaoParcelaFinanceira.Cancelado
                ? "btn btn-xs btn-danger disabled"
                : "btn btn-xs btn-danger";
        }

        public static string GetColorDescricaoDebitoCredito(TipoMovimentacao tipo)
        {
            return tipo == TipoMovimentacao.Debito ? "red" : "green";
        }

        public static string GetStringLimitada(this string value, int limite)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (value.Length > limite)
                return value.Substring(0, limite);
            return value;
        }

    }
}