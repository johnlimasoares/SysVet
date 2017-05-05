using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Domain.Entidades;
using Domain.Entidades.Cadastro;
using PagedList;
using Repository;
using Repository.Context;

namespace SisVetWeb.Controllers
{
    public class MedicamentoController : Controller
    {
        private readonly MedicamentoRepository repoMedicamento = new MedicamentoRepository();

        
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page) {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NomeParam = String.IsNullOrEmpty(sortOrder) ? "Nome_desc" : "";


            if (searchString != null) {
                page = 1;
            } else {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var medicamentos = from m in repoMedicamento.GetAll().ToList()
                        select m;
            if (!String.IsNullOrEmpty(searchString)) {
                medicamentos = medicamentos.Where(s => s.Nome.ToUpper().Contains(searchString.ToUpper()));
            }

            switch (sortOrder) {
                case "Nome_Desc":
                    medicamentos = medicamentos.OrderByDescending(x => x.Nome);
                    break;

                default:
                    medicamentos = medicamentos.OrderByDescending(x => x.Id);
                    break;

            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);

            return View(medicamentos.ToPagedList(pageNumber, pageSize));
        }

       
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicamento medicamento = repoMedicamento.Find(id);
            if (medicamento == null)
            {
                return HttpNotFound();
            }
            return View(medicamento);
        }

    
        public ActionResult Create()
        {
            return View();
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Nome,Marca,Categoria,Intervalo,Lote")] Medicamento medicamento)
        {
            if (ModelState.IsValid)
            {
                repoMedicamento.Adicionar(medicamento);
                repoMedicamento.SalvarTodos();
                return RedirectToAction("Index");
            }

            return View(medicamento);
        }

       
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicamento medicamento = repoMedicamento.Find(id);
            if (medicamento == null)
            {
                return HttpNotFound();
            }
            return View(medicamento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Nome,Marca,Categoria,Intervalo,Lote")] Medicamento medicamento)
        {
            if (ModelState.IsValid)
            {
                repoMedicamento.Atualizar(medicamento);
                repoMedicamento.SalvarTodos();
                return RedirectToAction("Index");
            }
            return View(medicamento);
        }

      
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Medicamento medicamento = repoMedicamento.Find(id);
            if (medicamento == null)
            {
                return HttpNotFound();
            }
            return View(medicamento);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Medicamento medicamento = repoMedicamento.Find(id);
            repoMedicamento.Excluir(e => e == medicamento);
            repoMedicamento.SalvarTodos();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repoMedicamento.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
