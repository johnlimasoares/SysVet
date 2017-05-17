using System.Linq;
using System.Net;
using System.Web.Mvc;
using Domain.Entidades.Cadastro.Financeiro;
using PagedList;
using Repository.Repositories;

namespace SisVetWeb.Controllers {
    public class FinanceiroCentroDeCustoController : Controller {
        private FinanceiroCentroDeCustoRepository repoCentroCusto = new FinanceiroCentroDeCustoRepository();
        private FinanceiroCentroDeCustoGrupoRepository repoCentroCustoGrupo = new FinanceiroCentroDeCustoGrupoRepository();
        
        public ActionResult Index(string pesquisa, int pagina = 1) {

            int totalRegistros = 20;

            ViewBag.pesquisaCorrente = pesquisa;

            var centroCustos = repoCentroCusto.GetAll().OrderBy(x => x.Id);

            if (!string.IsNullOrEmpty(pesquisa)) {
                centroCustos = (IOrderedQueryable<FinanceiroCentroDeCusto>)centroCustos.Where(x => x.Descricao.ToLower().Contains(pesquisa.ToLower()));
            }

            var quantidadeRegistros = centroCustos.Count();
            if (!string.IsNullOrEmpty(pesquisa) && quantidadeRegistros > 0)
                totalRegistros = quantidadeRegistros;

            return View(centroCustos.ToPagedList(pagina, totalRegistros));
        }

        public ActionResult Create() {
            ViewBag.FinanceiroCentroDeCustoGrupos = new SelectList(repoCentroCustoGrupo.GetAll(), "Id", "Descricao");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,descricao")] FinanceiroCentroDeCusto financeiroCentroDeCusto, int financeiroCentroDeCustoGrupoId) {
            if (ModelState.IsValid) {
                repoCentroCusto.Salvar(financeiroCentroDeCusto, financeiroCentroDeCustoGrupoId);
                return RedirectToAction("Index");
            }

            return View(financeiroCentroDeCusto);
        }

        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var financeiroCentroDeCusto = repoCentroCusto.Find(id);

            if (financeiroCentroDeCusto == null) {
                return HttpNotFound();
            }

            ViewBag.FinanceiroCentroDeCustoGrupos =
                repoCentroCustoGrupo.GetAll()
                    .Select(
                        grupo =>
                            new SelectListItem() {
                                Text = grupo.Descricao,
                                Value = grupo.Id.ToString(),
                                Selected = financeiroCentroDeCusto.FinanceiroCentroDeCustoGrupoId == grupo.Id
                            });

            return View(financeiroCentroDeCusto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,descricao")] FinanceiroCentroDeCusto financeiroCentroDeCusto, int financeiroCentroDeCustoGrupoId) {
            if (ModelState.IsValid) {
                repoCentroCusto.Editar(financeiroCentroDeCusto, financeiroCentroDeCustoGrupoId);
                return RedirectToAction("Index");
            }
            return View(financeiroCentroDeCusto);
        }
    }
}