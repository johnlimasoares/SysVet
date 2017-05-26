using System.Linq;
using System.Net;
using System.Web.Mvc;
using Domain.Entidades.Cadastro.Financeiro;
using PagedList;
using Repository.Repositories;

namespace SisVetWeb.Controllers {
    public class FinanceiroCentroDeCustoGrupoController : Controller {

        private FinanceiroCentroDeCustoGrupoRepository repoCentroCusto = new FinanceiroCentroDeCustoGrupoRepository();
        public ActionResult Index(string pesquisa, int pagina = 1) {

            int totalRegistros = 20;

            ViewBag.pesquisaCorrente = pesquisa;

            var centroCustGrupos = repoCentroCusto.GetAll().OrderBy(x => x.Id);

            if (!string.IsNullOrEmpty(pesquisa)) {
                centroCustGrupos = (IOrderedQueryable<FinanceiroCentroDeCustoGrupo>)centroCustGrupos.Where(x => x.Descricao.ToLower().Contains(pesquisa.ToLower()));
            }

            var quantidadeRegistros = centroCustGrupos.Count();
            if (!string.IsNullOrEmpty(pesquisa) && quantidadeRegistros > 0)
                totalRegistros = quantidadeRegistros;

            return View(centroCustGrupos.ToPagedList(pagina, totalRegistros));
        }

        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,descricao")] FinanceiroCentroDeCustoGrupo financeiroCentroDeCustoGrupo) {
            if (ModelState.IsValid) {
                repoCentroCusto.Salvar(financeiroCentroDeCustoGrupo);
                return RedirectToAction("Index");
            }

            return View(financeiroCentroDeCustoGrupo);
        }

        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FinanceiroCentroDeCustoGrupo financeiroCentroDeCustoGrupo = repoCentroCusto.Find(id);
            if (financeiroCentroDeCustoGrupo == null) {
                return HttpNotFound();
            }
            return View(financeiroCentroDeCustoGrupo);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,descricao")] FinanceiroCentroDeCustoGrupo financeiroCentroDeCustoGrupo) {
            if (ModelState.IsValid) {
                repoCentroCusto.Editar(financeiroCentroDeCustoGrupo);
                return RedirectToAction("Index");
            }
            return View(financeiroCentroDeCustoGrupo);
        }
    }
}