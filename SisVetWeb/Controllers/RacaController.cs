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
    public class RacaController : Controller
    {
        private RacaRepository repoRaca = new RacaRepository();
        private EspecieRepository repoEspecie = new EspecieRepository();

        

        // GET: /Raca/
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page) {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NomeParam = String.IsNullOrEmpty(sortOrder) ? "Nome_desc" : "";


            if (searchString != null) {
                page = 1;
            } else {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var racas = from m in repoRaca.GetAll().ToList()
                           select m;
            if (!String.IsNullOrEmpty(searchString)) {
                racas = racas.Where(s => s.Descricao.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder) {
                case "Nome_Desc":
                    racas = racas.OrderByDescending(x => x.Descricao);
                    break;

                default:
                    racas = racas.OrderByDescending(x => x.ID);
                    break;

            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);

            return View(racas.ToPagedList(pageNumber, pageSize));
        }

        // GET: /Raca/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Raca raca = repoRaca.Find(id);
            if (raca == null)
            {
                return HttpNotFound();
            }
            return View(raca);
        }

        // GET: /Raca/Create
        public ActionResult Create() {
           
           ViewBag.EspecieId = new SelectList(repoEspecie.GetAll(),"ID","Descricao");
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Descricao")] Raca raca,int especieId) {
            var especieID = new Especie() {ID = especieId};
            
            
            
            if (ModelState.IsValid) {

                using (var ctx = new BancoContexto()) {
                    ctx.Entry(especieID).State = EntityState.Unchanged;
                    raca.Especie = especieID;
                    ctx.Racas.Add(raca);
                    ctx.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(raca);
        }

      
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
          
            Raca raca = repoRaca.Find(id);
            if (raca == null)
            {                 
                return HttpNotFound();
            }
            ViewBag.Especies =
                repoEspecie.GetAll()
                    .Select(
                        g =>
                            new SelectListItem
                            {
                                Text = g.Descricao,
                                Value = g.ID.ToString(),
                                Selected = raca.EspecieId == g.ID
                            });
            return View(raca);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Descricao")] Raca raca, int especieId)
        {
            if (ModelState.IsValid)
            {                
                using (var ctx = new BancoContexto()) {                  
                    raca.EspecieId = especieId;
                    ctx.Entry(raca).State = EntityState.Modified;
                    ctx.SaveChanges();
            }                              
                return RedirectToAction("Index");
            }
            return View(raca);
        }

        // GET: /Raca/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Raca raca = repoRaca.Find(id);
            if (raca == null)
            {
                return HttpNotFound();
            }
            return View(raca);
        }

        // POST: /Raca/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Raca raca = repoRaca.Find(id);
            repoRaca.Excluir(c => c == raca);
            repoRaca.SalvarTodos();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repoRaca.Dispose();
            }
            base.Dispose(disposing);
        }

   
    
    }


   
}
