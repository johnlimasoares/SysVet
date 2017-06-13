using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
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

        public ActionResult Details(int id) {
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
                var financeiroContasReceberParcelasList = new List<FinanceiroContasReceberParcelas>();

                for (var parcela = 1; parcela <= financeiroTipoRecebimento.QuantidadeParcelas; parcela++) {

                    var financeiroContasReceberParcela = new FinanceiroContasReceberParcelas();
                    financeiroContasReceberParcela.DataEmissao = financeiroTipoRecebimento.DataEmissao;
                    financeiroContasReceberParcela.Parcela = parcela;
                    financeiroContasReceberParcela.ValorTotalBruto = financeiroTipoRecebimento.ValorTotal / financeiroTipoRecebimento.QuantidadeParcelas;
                    financeiroContasReceberParcela.NumeroDocumento = string.Format("{0}{1}", parcela, financeiroTipoRecebimento.ClienteId);
                    financeiroContasReceberParcelasList.Add(financeiroContasReceberParcela);

                }

                var demonstrativoParcelasVM = new FinanceiroDuplicataDemonstrativoDeParcelasViewModel();
                demonstrativoParcelasVM.FinanceiroContasReceberParcelasList = financeiroContasReceberParcelasList;
                demonstrativoParcelasVM.FinanceiroTipoRecebimento = financeiroTipoRecebimento;

                return View("DemonstrativoParcelas", demonstrativoParcelasVM);
            } catch {
                return View();
            }
        }

        public ActionResult Edit(int id) {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection) {
            try {
                return RedirectToAction("Index");
            } catch {
                return View();
            }
        }

        public ActionResult Delete(int id) {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection) {
            try {
                return RedirectToAction("Index");
            } catch {
                return View();
            }
        }
    }
}
