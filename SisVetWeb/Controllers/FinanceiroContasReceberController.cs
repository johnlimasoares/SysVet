using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Business.Financeiro.ContasReceber;
using Domain.Entidades.Cadastro;
using Domain.Entidades.Operacao.Financeiro;
using Repository;
using Repository.Context;
using Repository.Repositories;
using SisVetWeb.Models;
using Utils;

namespace SisVetWeb.Controllers
{
    public class FinanceiroContasReceberController : Controller
    {
        private ClienteRepository repoCliente = new ClienteRepository();
        private FinanceiroCentroDeCustoRepository repoCentroCusto = new FinanceiroCentroDeCustoRepository();
        private FinanceiroPlanoDePagamentoRepository repoPlanoPagamento = new FinanceiroPlanoDePagamentoRepository();
        private FinanceiroContasReceberParcelasRepository repoContasReceber = new FinanceiroContasReceberParcelasRepository();

        public ActionResult Index(string tipoPesquisa, DateTime? dataInicial, DateTime? dataFinal, string pesquisaCliente, string tipoPesquisaCliente)
        {
            var parcelasEtotalizadores = new FinanceiroParcelasETotalizadoresViewModel();
            parcelasEtotalizadores.FinanceiroContasReceberParcelasDapperList = repoContasReceber.GetAllContasReceberDapper(tipoPesquisa, dataInicial, dataFinal, pesquisaCliente,tipoPesquisaCliente).ToList();
            return View(parcelasEtotalizadores.PreencherTotalizadores());
        }

        public ActionResult GerarParcelasDuplicata()
        {
            ViewBag.CentroCustoId = new SelectList(
             repoCentroCusto.GetAll().OrderBy(x => x.Descricao),
             "Id",
             "Descricao"
             );

            ViewBag.FormaPagamentoId = new SelectList(
             repoPlanoPagamento.GetAll().OrderBy(x => x.Descricao),
             "Id",
             "Descricao"
             );

            ViewBag.ClienteId = new SelectList(
            repoCliente.GetAll().OrderBy(a => a.Nome),
            "ID",
            "Nome"
            );
            return View();
        }

        [HttpPost]
        public ActionResult GerarParcelasDuplicata(FinanceiroTipoRecebimento financeiroTipoRecebimento)
        {
            var demonstrativoParcelasVm = new FinanceiroDemonstrativoDeParcelasViewModel();
            demonstrativoParcelasVm.DemonstrativoParcelasList = ParcelasBusiness.GerarDemonstrativoParcelas(financeiroTipoRecebimento);
            demonstrativoParcelasVm.FinanceiroTipoRecebimento = financeiroTipoRecebimento;
            demonstrativoParcelasVm.NomeCliente = repoCliente.GetNomeCliente(financeiroTipoRecebimento.ClienteId);
            demonstrativoParcelasVm.DescricaoPlanoPagamento = repoPlanoPagamento.GetDescricaoPlano(financeiroTipoRecebimento.FinanceiroPlanoDePagamentoId);
            return View("DemonstrativoParcelas", demonstrativoParcelasVm);
        }

        [HttpPost]
        public JsonResult ValidarQuantidadeMaximaParcelasPlano(string planoPagamentoId)
        {
            try
            {
                var maximoParcelas = repoPlanoPagamento.GetPlanoPagamento(planoPagamentoId.ToInteger()).QuantidadeParcelas;
                return Json(maximoParcelas, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Confirmar()
        {
            var demonstrativoParcelasVM = (FinanceiroDemonstrativoDeParcelasViewModel)TempData["FullModel"];
            //validar quantidade parcelas do plano
            ParcelasBusiness.SalvarParcelasGeradas(demonstrativoParcelasVM.DemonstrativoParcelasList, demonstrativoParcelasVM.FinanceiroTipoRecebimento);
            return RedirectToAction("Index");
        }

        public ActionResult InformacaoParcelaBaixa(int id)
        {
            var informacaoDeParcela = InformacaoDeParcelaViewModel(id);
            return View("BaixaParcela", informacaoDeParcela);
        }

        public ActionResult InformacaoParcelaCancelamentoBaixa(int id)
        {
            var informacaoDeParcela = InformacaoDeParcelaViewModel(id);
            return View("CancelarBaixa", informacaoDeParcela);
        }

        public ActionResult InformacaoParcelaCancelamento(int id)
        {
            var informacaoDeParcela = InformacaoDeParcelaViewModel(id);
            return View("CancelarParcela", informacaoDeParcela);
        }

        [HttpPost]
        public ActionResult BaixarParcela(InformacaoDeParcelaViewModel baixaDeParcelaViewModel)
        {
            var financeiroParcelaRecebida = new FinanceiroContasReceberParcelas();
            financeiroParcelaRecebida.Id = baixaDeParcelaViewModel.ParcelaId;
            financeiroParcelaRecebida.DataRecebimento = baixaDeParcelaViewModel.DataRecebimento;
            financeiroParcelaRecebida.HoraRecebimento = DateTime.Now.TimeOfDay;
            financeiroParcelaRecebida.Observacoes = baixaDeParcelaViewModel.Observacoes;
            ParcelasBusiness.BaixarParcela(financeiroParcelaRecebida);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CancelarBaixa(InformacaoDeParcelaViewModel baixaDeParcelaViewModel)
        {
            ParcelasBusiness.CancelarBaixa(baixaDeParcelaViewModel.ParcelaId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult CancelarParcela(InformacaoDeParcelaViewModel baixaDeParcelaViewModel)
        {
            ParcelasBusiness.CancelarParcela(baixaDeParcelaViewModel.ParcelaId);
            return RedirectToAction("Index");
        }

        private static InformacaoDeParcelaViewModel InformacaoDeParcelaViewModel(int id)
        {
            var informacaoDeParcela = new InformacaoDeParcelaViewModel();
            using (var ctx = new BancoContexto())
            {
                var contaReceberParcela = ctx.FinanceiroContasReceberParcelas
                    .Join(ctx.FinanceiroTipoRecebimentos, parcela => parcela.FinanceiroTipoRecebimentoId,
                        tipoRecebimento => tipoRecebimento.Id, (parcela, tipoRecebimento) => new { parcela, tipoRecebimento })
                    .Join(ctx.Clientes, tipoRecebimento => tipoRecebimento.tipoRecebimento.ClienteId, cliente => cliente.Id,
                        (tipoRecebimento, cliente) => new { tipoRecebimento, cliente })
                    .Select(x => new
                    {
                        x.tipoRecebimento.parcela.NumeroDocumento,
                        x.tipoRecebimento.parcela.DataVencimento,
                        x.tipoRecebimento.parcela.DataRecebimento,
                        x.tipoRecebimento.parcela.ValorTotalLiquido,
                        x.tipoRecebimento.parcela.Observacoes,
                        x.tipoRecebimento.parcela.Id,
                        x.cliente.Nome
                    }).AsNoTracking().Single(x => x.Id == id);

                informacaoDeParcela.ParcelaId = (int)contaReceberParcela.Id;
                informacaoDeParcela.DataRecebimento = contaReceberParcela.DataRecebimento ?? DateTime.Now;
                informacaoDeParcela.DataVencimento = contaReceberParcela.DataVencimento;
                informacaoDeParcela.NomeCliente = contaReceberParcela.Nome;
                informacaoDeParcela.NumeroDocumento = contaReceberParcela.NumeroDocumento;
                informacaoDeParcela.ValorTotalLiquido = contaReceberParcela.ValorTotalLiquido;
                informacaoDeParcela.Observacoes = contaReceberParcela.Observacoes;
            }
            return informacaoDeParcela;
        }

    }
}
