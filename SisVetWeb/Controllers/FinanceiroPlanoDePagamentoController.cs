using System.Linq;
using System.Net;
using System.Web.Mvc;
using Domain.Entidades.Cadastro.Financeiro;
using PagedList;
using Repository.Repositories;

namespace SisVetWeb.Controllers {
    public class FinanceiroPlanoDePagamentoController : Controller {
        private FinanceiroPlanoDePagamentoRepository repoPlano = new FinanceiroPlanoDePagamentoRepository();

        public ActionResult Index(string pesquisa, int pagina = 1) {

            int totalRegistros = 20;

            ViewBag.pesquisaCorrente = pesquisa;

            var planosDePagamentos = repoPlano.GetAll().OrderBy(x => x.Id);

            if (!string.IsNullOrEmpty(pesquisa)) {
                planosDePagamentos = (IOrderedQueryable<FinanceiroPlanoDePagamento>)planosDePagamentos.Where(x => x.Descricao.ToLower().Contains(pesquisa.ToLower()));
            }

            var quantidadeRegistros = planosDePagamentos.Count();
            if (!string.IsNullOrEmpty(pesquisa) && quantidadeRegistros > 0)
                totalRegistros = quantidadeRegistros;

            return View(planosDePagamentos.ToPagedList(pagina, totalRegistros));
        }

        public ActionResult Details(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var financeiroplanodepagamento = repoPlano.Find(id);
            if (financeiroplanodepagamento == null) {
                return HttpNotFound();
            }
            return View(financeiroplanodepagamento);
        }

        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descricao,QuantidadeParcelas,IntervaloDias")] FinanceiroPlanoDePagamento financeiroPlanoDePagamento) {
            if (ModelState.IsValid) {
                repoPlano.Salvar(financeiroPlanoDePagamento);
                return RedirectToAction("Index");
            }

            return View(financeiroPlanoDePagamento);
        }

        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var financeiroplanodepagamento = repoPlano.Find(id);
            if (financeiroplanodepagamento == null) {
                return HttpNotFound();
            }
            return View(financeiroplanodepagamento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descricao,QuantidadeParcelas,IntervaloDias")] FinanceiroPlanoDePagamento financeiroplanodepagamento) {
            if (ModelState.IsValid) {
                repoPlano.Editar(financeiroplanodepagamento);
                return RedirectToAction("Index");
            }
            return View(financeiroplanodepagamento);
        }

        [HttpPost]
        public JsonResult Delete(int id) {
            string mensagem = string.Empty;
            var plano = repoPlano.Excluir(id);
            mensagem = string.Format("{0} excluido com sucesso", plano.Descricao);
            return Json(mensagem, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                repoPlano.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
