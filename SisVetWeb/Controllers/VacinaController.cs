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
using Repository.Repositories;

namespace SisVetWeb.Controllers
{
    public class VacinaController : Controller
    {
        private readonly VacinaRepository repoVacina = new VacinaRepository();

  
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var vacina= from m in repoVacina.GetAll().ToList()
                select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                vacina = vacina.Where(s => s.Descricao.ToUpper().Contains(searchString.ToUpper()));
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);

            return View(vacina.ToPagedList(pageNumber, pageSize));
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
