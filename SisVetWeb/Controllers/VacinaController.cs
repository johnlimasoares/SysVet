using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Domain.Entidades.Cadastro;
using PagedList;
using Repository.Repositories;

namespace SisVetWeb.Controllers
{
    public class VacinaController : Controller
    {
        private readonly VacinaRepository repoVacina = new VacinaRepository();


        public ActionResult Index(string ordenacao, string pesquisa, string tipoPesquisa, int pagina = 1)
        {
            int totalRegistros = 20;
            ViewBag.IdParam = ordenacao == "Id" ? "Id_Desc" : "Id";
            ViewBag.NomeParam = ordenacao == "Nome" ? "Nome_Desc" : "Nome";

            ViewBag.ordenacaoCorrente = ordenacao;
            ViewBag.tipoPesquisa = tipoPesquisa;
            ViewBag.pesquisaCorrente = pesquisa;

            var vacinas = from m in repoVacina.GetAll().ToList()
                        select m;

            if (!String.IsNullOrEmpty(pesquisa)) {
                vacinas = vacinas.Where(s => s.Descricao.ToUpper().Contains(pesquisa.ToUpper()));
            }

            switch (ordenacao) {
                case "Nome_Desc":
                    vacinas = vacinas.OrderByDescending(x => x.Descricao);
                    break;
                case "Nome":
                    vacinas = vacinas.OrderBy(x => x.Descricao);
                    break;
                case "Id_Desc":
                    vacinas = vacinas.OrderByDescending(x => x.ID);
                    break;
                default:
                    vacinas = vacinas.OrderBy(x => x.ID);
                    break;

            }

            var quantidadeRegistros = vacinas.Count();
            if (!string.IsNullOrEmpty(pesquisa) && quantidadeRegistros > 0)
                totalRegistros = quantidadeRegistros;


            return View(vacinas.ToPagedList(pagina, totalRegistros));
        }
        public ActionResult Details(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacina vacina = repoVacina.Find(id);
            if (vacina == null) {
                return HttpNotFound();
            }
            return View(vacina);
        }

        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Descricao,IntervaloDias,Doses")] Vacina vacina)
        {
            if (ModelState.IsValid)
            {
                repoVacina.Adicionar(vacina);
               repoVacina.SalvarTodos();
                return RedirectToAction("Index");
            }

            return View(vacina);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacina vacina = repoVacina.Find(id);
            if (vacina == null)
            {
                return HttpNotFound();
            }
            return View(vacina);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Descricao,IntervaloDias,Doses")] Vacina vacina)
        {
            if (ModelState.IsValid)
            {
                repoVacina.Atualizar(vacina);
                repoVacina.SalvarTodos();
                return RedirectToAction("Index");
            }
            return View(vacina);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacina vacina = repoVacina.Find(id);
            if (vacina == null)
            {
                return HttpNotFound();
            }
            return View(vacina);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Vacina vacina = repoVacina.Find(id);
            repoVacina.Excluir(x => x.ID == vacina.ID);
            repoVacina.SalvarTodos();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repoVacina.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
