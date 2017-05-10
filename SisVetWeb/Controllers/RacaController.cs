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

namespace SisVetWeb.Controllers {
    public class RacaController : Controller {
        private RacaRepository repoRaca = new RacaRepository();
        private EspecieRepository repoEspecie = new EspecieRepository();

        public ActionResult Index(string ordenacao, string pesquisa, string tipoPesquisa, int pagina = 1) {

            int totalRegistros = 20;
            ViewBag.IdParam = ordenacao == "Id_Desc" ? "Id" : "Id_Desc";
            ViewBag.NomeParam = ordenacao == "Nome" ? "Nome_Desc" : "Nome";

            ViewBag.ordenacaoCorrente = ordenacao;
            ViewBag.tipoPesquisa = tipoPesquisa;
            ViewBag.pesquisaCorrente = pesquisa;

            var racas = from m in repoRaca.GetAll().ToList()
                        select m;

            if (!String.IsNullOrEmpty(pesquisa)) {
                racas = racas.Where(s => s.Descricao.ToUpper().Contains(pesquisa.ToUpper()));
            }

            switch (ordenacao) {
                case "Nome_Desc":
                    racas = racas.OrderByDescending(x => x.Descricao);
                    break;
                case "Nome":
                    racas = racas.OrderBy(x => x.Descricao);
                    break;
                case "Id_Desc":
                    racas = racas.OrderByDescending(x => x.Id);
                    break;
                default:
                    racas = racas.OrderBy(x => x.Id);
                    break;

            }

            var quantidadeRegistros = racas.Count();
            if (!string.IsNullOrEmpty(pesquisa) && quantidadeRegistros > 0)
                totalRegistros = quantidadeRegistros;


            return View(racas.ToPagedList(pagina, totalRegistros));
        }

        public ActionResult Details(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Raca raca = repoRaca.Find(id);
            if (raca == null) {
                return HttpNotFound();
            }
            return View(raca);
        }

        public ActionResult Create() {

            ViewBag.EspecieId = new SelectList(repoEspecie.GetAll(), "Id", "Descricao");
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Descricao")] Raca raca, int especieId) {
            var especieID = new Especie() { Id = especieId };
            
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
        
        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Raca raca = repoRaca.Find(id);
            if (raca == null) {
                return HttpNotFound();
            }
            ViewBag.Especies =
                repoEspecie.GetAll()
                    .Select(
                        g =>
                            new SelectListItem {
                                Text = g.Descricao,
                                Value = g.Id.ToString(),
                                Selected = raca.EspecieId == g.Id
                            });
            return View(raca);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Descricao")] Raca raca, int especieId) {
            if (ModelState.IsValid) {
                using (var ctx = new BancoContexto()) {
                    raca.EspecieId = especieId;
                    ctx.Entry(raca).State = EntityState.Modified;
                    ctx.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return View(raca);
        }

        public ActionResult Delete(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Raca raca = repoRaca.Find(id);
            if (raca == null) {
                return HttpNotFound();
            }
            return View(raca);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            Raca raca = repoRaca.Find(id);
            repoRaca.Excluir(c => c == raca);
            repoRaca.SalvarTodos();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                repoRaca.Dispose();
            }
            base.Dispose(disposing);
        }



    }



}
