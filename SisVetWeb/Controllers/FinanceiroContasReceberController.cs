using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Business.Financeiro.ContasReceber;
using Domain.Entidades.Operacao.Financeiro;
using Repository;
using Repository.Repositories;
using SisVetWeb.Models;

namespace SisVetWeb.Controllers {
    public class FinanceiroContasReceberController : Controller {
        private ClienteRepository repoCliente = new ClienteRepository();
        private FinanceiroCentroDeCustoRepository centroCustoRepo = new FinanceiroCentroDeCustoRepository();
        private FinanceiroPlanoDePagamentoRepository planoPagamentoRepo = new FinanceiroPlanoDePagamentoRepository();
        
        public ActionResult Index() {
            return View();
        }

        public ActionResult GerarParcelasDuplicata() {
            ViewBag.CentroCustoId = new SelectList(
             centroCustoRepo.GetAll().OrderBy(x => x.Descricao),
             "Id",
             "Descricao"
             );

            ViewBag.FormaPagamentoId = new SelectList(
             planoPagamentoRepo.GetAll().OrderBy(x => x.Descricao),
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
            try {
                var demonstrativoParcelasVM = new FinanceiroDuplicataDemonstrativoDeParcelasViewModel();
                demonstrativoParcelasVM.FinanceiroContasReceberParcelasList = DuplicataParcelasBusiness.GerarDemostrativoParcelas(financeiroTipoRecebimento);
                demonstrativoParcelasVM.FinanceiroTipoRecebimento = financeiroTipoRecebimento;

                return View("DemonstrativoParcelas", demonstrativoParcelasVM);
            } catch {
                return View();
            }
        }

        [HttpPost]
        public ActionResult Confirmar() {
            try {
                var demonstrativoParcelasVM = (FinanceiroDuplicataDemonstrativoDeParcelasViewModel)TempData["FullModel"];
                DuplicataParcelasBusiness.ConcluirRegistroFinanceiro(demonstrativoParcelasVM.FinanceiroContasReceberParcelasList,demonstrativoParcelasVM.FinanceiroTipoRecebimento);
            } catch(Exception ex) {
                return View();
            }
            return View("Index");
        }

    }
}
