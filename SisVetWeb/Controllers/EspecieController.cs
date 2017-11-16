using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Domain.Entidades.Cadastro;
using PagedList;
using Repository;
using Repository.Repositories;


namespace SisVetWeb.Controllers
{
    public class EspecieController : Controller
    {
        private readonly EspecieRepository repoEspecie = new EspecieRepository();

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page) {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NomeParam = String.IsNullOrEmpty(sortOrder) ? "Nome_desc" : "";

            if (searchString != null) {
                page = 1;
            } else {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var especies = from m in repoEspecie.GetAll().ToList()
                           select m;
            if (!String.IsNullOrEmpty(searchString)) {
                especies = especies.Where(s => s.Descricao.ToUpper().Contains(searchString.ToUpper()));
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);

            return View(especies.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Especie especie = repoEspecie.Find(id);
            if (especie == null)
            {
                return HttpNotFound();
            }
            return View(especie);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,descricao")] Especie especie)
        {
            if (ModelState.IsValid)
            {
                repoEspecie.Adicionar(especie);
                repoEspecie.SalvarTodos();
                return RedirectToAction("Index");
            }

            return View(especie);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Especie especie = repoEspecie.Find(id);
            if (especie == null)
            {
                return HttpNotFound();
            }
            return View(especie);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,descricao")] Especie especie)
        {
            if (ModelState.IsValid)
            {
                repoEspecie.Atualizar(especie);
                repoEspecie.SalvarTodos();
                return RedirectToAction("Index");
            }
            return View(especie);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Especie especie = repoEspecie.Find(id);
            if (especie == null)
            {
                return HttpNotFound();
            }
            return View(especie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Especie especie = repoEspecie.Find(id);
            repoEspecie.Excluir(e => e == especie);
            repoEspecie.SalvarTodos();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repoEspecie.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
