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
    public class TipoServicoController : Controller
    {
        private BancoContexto db = new BancoContexto();
        private TipoServicoRepository repoTipoServico = new TipoServicoRepository();


        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            if (String.IsNullOrEmpty(sortOrder))
                sortOrder = "Descricao";

            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdParam = sortOrder == "Id" ? "Id_Desc" : "Id";
            ViewBag.ValorParam = sortOrder == "Valor" ? "Valor_Desc" : "Valor";
            ViewBag.NomeParam = sortOrder == "Descricao" ? "Descricao_Desc" : "Descricao";

            if (searchString != null) {
                page = 1;
            } else {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var tipoServico = from m in repoTipoServico.GetAll().ToList()
                           select m;
            if (!String.IsNullOrEmpty(searchString)) {
                tipoServico = tipoServico.Where(s => s.Descricao.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder) {
                case "Id":
                    tipoServico = tipoServico.OrderBy(x => x.ID);
                    break;
                case "Id_Desc":
                    tipoServico = tipoServico.OrderByDescending(x => x.ID);
                    break;
                case "Descricao":
                    tipoServico = tipoServico.OrderBy(x => x.Descricao);
                    break;
                case "Descricao_Desc":
                    tipoServico = tipoServico.OrderByDescending(x => x.Descricao);
                    break;
                case "Valor":
                    tipoServico = tipoServico.OrderBy(x => x.Valor);
                    break;
                case "Valor_Desc":
                    tipoServico = tipoServico.OrderByDescending(x => x.Valor);
                    break;    
                default:
                    tipoServico = tipoServico.OrderBy(x => x.Descricao);
                    break;

            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);

            return View(tipoServico.ToPagedList(pageNumber, pageSize));
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoServico tiposervico = repoTipoServico.Find(id);
            if (tiposervico == null)
            {
                return HttpNotFound();
            }
            return View(tiposervico);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Descricao,Valor")] TipoServico tiposervico)
        {
            if (ModelState.IsValid)
            {
                repoTipoServico.Adicionar(tiposervico);
                repoTipoServico.SalvarTodos();
                return RedirectToAction("Index");
            }

            return View(tiposervico);
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoServico tiposervico = repoTipoServico.Find(id);
            if (tiposervico == null)
            {
                return HttpNotFound();
            }
            return View(tiposervico);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Descricao,Valor")] TipoServico tiposervico)
        {
            if (ModelState.IsValid)
            {
                repoTipoServico.Atualizar(tiposervico);
                repoTipoServico.SalvarTodos();
                return RedirectToAction("Index");
            }
            return View(tiposervico);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoServico tiposervico = repoTipoServico.Find(id);
            if (tiposervico == null)
            {
                return HttpNotFound();
            }
            return View(tiposervico);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoServico tiposervico = db.TipoServicos.Find(id);
            repoTipoServico.Excluir(x => x == tiposervico);
            repoTipoServico.SalvarTodos();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repoTipoServico.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
