using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Domain.Entidades;
using Domain.Entidades.Cadastro;
using PagedList;
using Repository.Context;
using Repository;


namespace SisVetWeb.Controllers
{
    public class EspecieController : Controller
    {
        private readonly EspecieRepository repoEspecie = new EspecieRepository();

        // GET: /Especie/
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

        // GET: /Especie/Details/5
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

        // GET: /Especie/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Especie/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: /Especie/Edit/5
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

        // POST: /Especie/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: /Especie/Delete/5
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

        // POST: /Especie/Delete/5
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
