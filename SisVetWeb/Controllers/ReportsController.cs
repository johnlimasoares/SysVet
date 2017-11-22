using System;
using System.Web.Mvc;
using Reports;

namespace SisVetWeb.Controllers
{
    public class ReportsController : Controller
    {
        public ActionResult Vacinacoes()
        {
            return View();
        }

        private VacinacoesReport GetRelatorio(DateTime? data, DateTime? datafinal, string status, string pesquisaTexto)
        {
            var rpt = new VacinacoesReport(data, datafinal, status, pesquisaTexto);
            rpt.BasePath = Server.MapPath("/");

            rpt.Titulo = "Relatório de Vacinações";
            rpt.ImprimirCabecalhoPadrao = true;
            rpt.ImprimirRodapePadrao = false;

            return rpt;
        }

        public ActionResult Preview(DateTime? data, DateTime? datafinal, string status, string pesquisaTexto)
        {
            var rpt = GetRelatorio(data, datafinal, status, pesquisaTexto);
            return File(rpt.GetOutput().GetBuffer(), "application/pdf");
        }

        public FileResult BaixarPDF(DateTime? data, DateTime? datafinal, string status, string pesquisaTexto)
        {
            var rpt = GetRelatorio(data, datafinal, status, pesquisaTexto);
            return File(rpt.GetOutput().GetBuffer(), "application/pdf", "Documento.pdf");
        }
    }
}