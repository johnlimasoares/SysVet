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

        private VacinacoesReport GetRelatorio(DateTime? data, string status, string pesquisaTexto)
        {
            var rpt = new VacinacoesReport(data, status, pesquisaTexto);
            rpt.BasePath = Server.MapPath("/");

            rpt.Titulo = "Relatório de Vacinações";
            rpt.ImprimirCabecalhoPadrao = true;
            rpt.ImprimirRodapePadrao = false;

            return rpt;
        }

        public ActionResult Preview(DateTime? data, string status, string pesquisaTexto)
        {
            var rpt = GetRelatorio(data, status, pesquisaTexto);
            return File(rpt.GetOutput().GetBuffer(), "application/pdf");
        }

        public FileResult BaixarPDF(DateTime? data, string status, string pesquisaTexto)
        {
            var rpt = GetRelatorio(data, status, pesquisaTexto);
            return File(rpt.GetOutput().GetBuffer(), "application/pdf", "Documento.pdf");
        }
    }
}