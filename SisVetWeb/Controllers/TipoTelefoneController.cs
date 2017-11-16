using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Domain.Entidades;
using Domain.Entidades.Cadastro.Contato;
using PagedList;
using Repository;
using Repository.Context;
using Repository.Repositories;

namespace SisVetWeb.Controllers
{
    public class TipoTelefoneController : Controller
    {
        private readonly TipoTelefoneRepository repoTipoFone = new TipoTelefoneRepository();


        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, string typeSearch) {
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            var tipoFone = from m in repoTipoFone.GetAll().ToList()
                          select m;
            return View(tipoFone.ToPagedList(pageNumber, pageSize));
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoTelefone tipotelefone = repoTipoFone.Find(id);
            if (tipotelefone == null)
            {
                return HttpNotFound();
            }
            return View(tipotelefone);
        }

       
        public ActionResult Create()
        {
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Descricao")] TipoTelefone tipotelefone)
        {
            if (ModelState.IsValid)
            {
                repoTipoFone.Adicionar(tipotelefone);
                repoTipoFone.SalvarTodos();
                return RedirectToAction("Index");
            }

            return View(tipotelefone);          
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoTelefone tipotelefone = repoTipoFone.Find(id);
            if (tipotelefone == null)
            {
                return HttpNotFound();
            }
            return View(tipotelefone);
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Descricao")] TipoTelefone tipotelefone)
        {
            if (ModelState.IsValid)
            {
                repoTipoFone.Atualizar(tipotelefone);
                repoTipoFone.SalvarTodos();
                return RedirectToAction("Index");
            }
            return View(tipotelefone);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoTelefone tipotelefone = repoTipoFone.Find(id);
            if (tipotelefone == null)
            {
                return HttpNotFound();
            }
            return View(tipotelefone);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoTelefone tipotelefone = repoTipoFone.Find(id);
            repoTipoFone.Excluir(x => x==tipotelefone);
            repoTipoFone.SalvarTodos();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repoTipoFone.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
