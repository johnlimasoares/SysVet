using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Business.Financeiro.ContasReceber;
using Domain.Entidades.Cadastro;
using Domain.Entidades.Operacao.Financeiro;
using Repository;
using Repository.Repositories;
using SisVetWeb.Models;
using Utils;

namespace SisVetWeb.Controllers {
    public class FinanceiroContasReceberController : Controller {
        private ClienteRepository repoCliente = new ClienteRepository();
        private FinanceiroCentroDeCustoRepository repoCentroCusto = new FinanceiroCentroDeCustoRepository();
        private FinanceiroPlanoDePagamentoRepository repoPlanoPagamento = new FinanceiroPlanoDePagamentoRepository();
        private FinanceiroContasReceberParcelasRepository repoContasReceber = new FinanceiroContasReceberParcelasRepository();

        public ActionResult Index() {
            var parcelasEtotalizadores = new FinanceiroParcelasETotalizadoresViewModel();
            parcelasEtotalizadores.FinanceiroContasReceberParcelasDapperList = repoContasReceber.GetAllContasReceberDapper().ToList();
            return View(parcelasEtotalizadores);
        }

        public ActionResult GerarParcelasDuplicata() {
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
        public ActionResult GerarParcelasDuplicata(FinanceiroTipoRecebimento financeiroTipoRecebimento) {
            var demonstrativoParcelasVm = new FinanceiroDemonstrativoDeParcelasViewModel();
            demonstrativoParcelasVm.FinanceiroContasReceberParcelasList = DuplicataParcelasBusiness.GerarDemostrativoParcelas(financeiroTipoRecebimento);
            demonstrativoParcelasVm.FinanceiroTipoRecebimento = financeiroTipoRecebimento;
            demonstrativoParcelasVm.NomeCliente = repoCliente.GetNomeCliente(financeiroTipoRecebimento.ClienteId);
            demonstrativoParcelasVm.DescricaoPlanoPagamento = repoPlanoPagamento.GetDescricaoPlano(financeiroTipoRecebimento.FinanceiroPlanoDePagamentoId);
            return View("DemonstrativoParcelas", demonstrativoParcelasVm);
        }

        [HttpPost]
        public JsonResult ValidarQuantidadeMaximaParcelasPlano(string planoPagamentoId) {
            try {
                var maximoParcelas = repoPlanoPagamento.GetPlanoPagamento(planoPagamentoId.ToInteger()).QuantidadeParcelas;
                return Json(maximoParcelas, JsonRequestBehavior.AllowGet);
            } catch (Exception ex) {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Confirmar() {
            try {
                var demonstrativoParcelasVM = (FinanceiroDemonstrativoDeParcelasViewModel)TempData["FullModel"];
                //validar quantidade parcelas do plano
                DuplicataParcelasBusiness.SalvarRegistroFinanceiro(demonstrativoParcelasVM.FinanceiroContasReceberParcelasList, demonstrativoParcelasVM.FinanceiroTipoRecebimento);
            } catch (Exception ex) {
                return null;
            }
            return View("Index");
        }

    }
}
