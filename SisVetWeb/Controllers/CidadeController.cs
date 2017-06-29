using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Domain.Entidades;
using Domain.Entidades.Cadastro.Localidade;
using PagedList;
using Repository;
using Repository.Context;
using Repository.Repositories;

namespace SisVetWeb.Controllers
{
    public class CidadeController : Controller
    {
        private CidadeRepository repoCidade = new CidadeRepository();

        // GET: /Cidade/
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, string typeSearch)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            var cidades = from m in repoCidade.GetAll().ToList()
                          select m;
            return View(cidades.ToPagedList(pageNumber, pageSize));
        }

        // GET: /Cidade/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cidade cidade = repoCidade.Find(id);
            if (cidade == null)
            {
                return HttpNotFound();
            }
            return View(cidade);
        }

        // GET: /Cidade/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Cidade/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Descricao,SiglaUnidadeFederativa")] Cidade cidade)
        {
            if (ModelState.IsValid)
            {
                repoCidade.Adicionar(cidade);
                repoCidade.SalvarTodos();
                return RedirectToAction("Index");
            }

            return View(cidade);
        }

        // GET: /Cidade/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cidade cidade = repoCidade.Find(id);
            if (cidade == null)
            {
                return HttpNotFound();
            }
            return View(cidade);
        }

        // POST: /Cidade/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Descricao,SiglaUnidadeFederativa")] Cidade cidade)
        {
            if (ModelState.IsValid)
            {
              repoCidade.Atualizar(cidade);
                repoCidade.SalvarTodos();
                return RedirectToAction("Index");
            }
            return View(cidade);
        }

        // GET: /Cidade/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cidade cidade = repoCidade.Find(id);
            if (cidade == null)
            {
                return HttpNotFound();
            }
            return View(cidade);
        }

        // POST: /Cidade/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cidade cidade = repoCidade.Find(id);
            repoCidade.Excluir(x => x == cidade);
            repoCidade.SalvarTodos();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repoCidade.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
