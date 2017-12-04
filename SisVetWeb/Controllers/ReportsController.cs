using System;
using System.Web.Mvc;
using Domain.EntidadesLeitura.ReportsModel;
using Reports;
using Utils;
using FinanceiroContasReceberReport = Reports.FinanceiroContasReceberReport;

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

        public ActionResult FinanceiroMovimentacoes()
        {
            return View();
        }

        public ActionResult Clientes()
        {
            return View();
        }

        public ActionResult GetRelatorioVacinacoesDetalhado(DateTime? data, DateTime? datafinal, string status, string pesquisaTexto)
        {
            var rpt = new VacinacoesDetalhadoReport(data, datafinal, status, pesquisaTexto);
            rpt.BasePath = GetServerPath();
            rpt.Titulo = "Relatório de vacinações";
            rpt.ListaDeFiltros.Add("Data Inicial", data.IsValidDate());
            rpt.ListaDeFiltros.Add("Data Final", datafinal.IsValidDate());
            rpt.ListaDeFiltros.Add("Status", status);
            rpt.ListaDeFiltros.Add("Descrição", pesquisaTexto);
            rpt.ImprimirCabecalhoPadrao = true;
            rpt.ImprimirRodapePadrao = false;
            return File(rpt.GetOutput().GetBuffer(), "application/pdf");
        }

        public ActionResult GetRelatorioAnaliseMensalDeVacinas(DateTime? dataInicial, DateTime? datafinal, string descricaoVacina)
        {
            var rpt = new VacinasAnaliseMensalReport(dataInicial, datafinal, descricaoVacina);
            rpt.Titulo = "Análise de Vacinas Mensal";
            rpt.BasePath = GetServerPath();
            rpt.ListaDeFiltros.Add("Data Inicial", dataInicial.IsValidDate());
            rpt.ListaDeFiltros.Add("Data Final", datafinal.IsValidDate());
            rpt.ListaDeFiltros.Add("Descrição", descricaoVacina);
            rpt.ImprimirCabecalhoPadrao = true;
            rpt.ImprimirRodapePadrao = false;
            return File(rpt.GetOutput().GetBuffer(), "application/pdf");
        }

        public ActionResult GetRelatorioContasReceber(DateTime? datainicial, DateTime? datafinal, string status, string pesquisaTexto)
        {
            var rpt = new FinanceiroContasReceberReport(datainicial, datafinal, status, pesquisaTexto);
            rpt.ListaDeFiltros.Add("Status", status);
            rpt.ListaDeFiltros.Add("Data Inicial", datainicial.IsValidDate());
            rpt.ListaDeFiltros.Add("Data Final", datafinal.IsValidDate());
            rpt.ListaDeFiltros.Add("pesquisa", pesquisaTexto);
            rpt.BasePath = GetServerPath();
            rpt.Titulo = "Relatório de Contas a Receber";
            rpt.ImprimirCabecalhoPadrao = true;
            rpt.ImprimirRodapePadrao = false;
            return File(rpt.GetOutput().GetBuffer(), "application/pdf");
        }

        public ActionResult GetRelatorioFinanceiroMovimentacoes(DateTime? datainicial, DateTime? datafinal, string tipo, string pesquisaTexto)
        {
            var rpt = new FinanceiroMovimentacoesReport(datainicial, datafinal, tipo, pesquisaTexto);
            rpt.ListaDeFiltros.Add("Tipo", tipo);
            rpt.ListaDeFiltros.Add("Data Inicial", datainicial.IsValidDate());
            rpt.ListaDeFiltros.Add("Data Final", datafinal.IsValidDate());
            rpt.ListaDeFiltros.Add("pesquisa", pesquisaTexto);
            rpt.BasePath = GetServerPath();
            rpt.Titulo = "Relatório de Movimentações";
            rpt.ImprimirCabecalhoPadrao = true;
            rpt.ImprimirRodapePadrao = false;
            return File(rpt.GetOutput().GetBuffer(), "application/pdf");
        }

        public ActionResult GetRelatorioClientes(DateTime? datainicial, DateTime? datafinal, string pesquisaTexto)
        {
            var rpt = new ClientesReport(datainicial, datafinal, pesquisaTexto);
            rpt.ListaDeFiltros.Add("Data Inicial", datainicial.IsValidDate());
            rpt.ListaDeFiltros.Add("Data Final", datafinal.IsValidDate());
            rpt.ListaDeFiltros.Add("Pesquisa", pesquisaTexto);
            rpt.BasePath = GetServerPath();
            rpt.Titulo = "Relatório de Clientes";
            rpt.ImprimirCabecalhoPadrao = true;
            rpt.ImprimirRodapePadrao = false;
            return File(rpt.GetOutput().GetBuffer(), "application/pdf");
        }

        private string GetServerPath()
        {
            return Server.MapPath("~");
        }

        //public FileResult BaixarPDF(DateTime? data, DateTime? datafinal, string status, string pesquisaTexto)
        //{
        //    var rpt = GetRelatorio(data, datafinal, status, pesquisaTexto);
        //    return File(rpt.GetOutput().GetBuffer(), "application/pdf", "Documento.pdf");
        //}
    }
}