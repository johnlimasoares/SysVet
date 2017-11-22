using System;
using System.Linq;
using System.Web.Mvc;
using Business.Financeiro.Movimentacao;
using Domain.Enum;
using Repository.Repositories;
using SisVetWeb.Models;


namespace SisVetWeb.Controllers
{
    public class FinanceiroMovimentacaoCaixaController : Controller
    {

        [HttpGet]
        public ActionResult Index(DateTime? dataInicial, DateTime? dataFinal, string tipoPesquisa, string tipoEntrada, string pesquisaTexto)
        {
            var repoMovimentacao = new FinanceiroMovimentacoesRepository();

            var movimentacoesList = repoMovimentacao.GetMovimentacoesDapper(dataInicial, dataFinal, tipoPesquisa, tipoEntrada, pesquisaTexto);
            var movimentacoesViewModel = new FinanceiroMovimentacoesInfoViewModel(movimentacoesList);

            return View("Index", movimentacoesViewModel);
        }

        [HttpGet]
        public ActionResult GerarMovimentacaoManual()
        {
            ViewBag.CentroCustoId = new SelectList(
             new FinanceiroCentroDeCustoRepository().GetAll().OrderBy(x => x.Descricao),
             "Id",
             "Descricao"
             );
            return View();
        }

        [HttpPost]
        public ActionResult GerarMovimentacaomanual(FinanceiroMovimentacaoViewModel financeiroMovimentacaoViewModel)
        {
            var movimentacao = new Domain.Entidades.Operacao.Financeiro.FinanceiroMovimentacoes();

            movimentacao.OrigemMovimentacao = OrigemMovimentacao.Manual;
            movimentacao.Observacao = financeiroMovimentacaoViewModel.Observacao;
            movimentacao.DataHora = financeiroMovimentacaoViewModel.DataLancamento;
            movimentacao.FinanceiroCentroDeCustoId = financeiroMovimentacaoViewModel.FinanceiroCentroDeCustoId;

            if (financeiroMovimentacaoViewModel.TipoMovimentacao == TipoMovimentacao.Credito)
            {
                movimentacao.Credito = financeiroMovimentacaoViewModel.Valor;
                movimentacao.TipoMovimentacao = TipoMovimentacao.Credito;
            }
            else
            {
                movimentacao.Debito = financeiroMovimentacaoViewModel.Valor;
                movimentacao.TipoMovimentacao = TipoMovimentacao.Debito;
            }
            MovimentacaoBusiness.GerarMovimentacaoManual(movimentacao);
            return RedirectToAction("Index");
        }
    }
}