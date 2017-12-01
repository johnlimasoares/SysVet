using System;
using System.Web.Mvc;
using Reports;

namespace SisVetWeb.Controllers
{
    public class ReportsController : Controller
    {
        public ActionResult VacinacoesDetalhado()
        {
            return View();
        }

        public ActionResult AnaliseVacinasMensal()
        {
            return View();
        }

        public ActionResult FinanceiroContasReceber()
        {
            return View();
        }

        public ActionResult GetRelatorioVacinacoesDetalhado(DateTime? data, DateTime? datafinal, string status, string pesquisaTexto)
        {
            var rpt = new VacinacoesDetalhadoReport(data, datafinal, status, pesquisaTexto);
            rpt.BasePath = Server.MapPath("/");
            rpt.Titulo = "Relatório de vacinações";
            rpt.ImprimirCabecalhoPadrao = true;
            rpt.ImprimirRodapePadrao = false;
            return File(rpt.GetOutput().GetBuffer(), "application/pdf");
        }

        public ActionResult GetRelatorioAnaliseMensalDeVacinas(DateTime? dataInicial, DateTime? datafinal, string descricaoVacina)
        {
            var rpt = new VacinasAnaliseMensalReport(dataInicial, datafinal, descricaoVacina);
            rpt.BasePath = Server.MapPath("/");
            rpt.Titulo = "Análise de Vacinas Mensal";
            rpt.ImprimirCabecalhoPadrao = true;
            rpt.ImprimirRodapePadrao = false;
            return File(rpt.GetOutput().GetBuffer(), "application/pdf");
        }

        public ActionResult GetRelatorioContasReceber(DateTime? datainicial, DateTime? datafinal, string status, string pesquisaTexto)
        {
            var rpt = new FinanceiroContasReceberReport(datainicial, datafinal, status, pesquisaTexto);
            rpt.BasePath = Server.MapPath("/");
            rpt.Titulo = "Relatório de Contas a Receber";
            rpt.ImprimirCabecalhoPadrao = true;
            rpt.ImprimirRodapePadrao = false;
            return File(rpt.GetOutput().GetBuffer(), "application/pdf");
        }

        //public FileResult BaixarPDF(DateTime? data, DateTime? datafinal, string status, string pesquisaTexto)
        //{
        //    var rpt = GetRelatorio(data, datafinal, status, pesquisaTexto);
        //    return File(rpt.GetOutput().GetBuffer(), "application/pdf", "Documento.pdf");
        //}
    }
}