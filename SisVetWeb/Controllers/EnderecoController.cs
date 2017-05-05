using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Domain.Entidades;
using Domain.Entidades.Cadastro.Localidade;
using Repository;
using Repository.Context;
using Utils;

namespace SisVetWeb.Controllers
{
    public class EnderecoController : Controller
    {
        private BancoContexto db = new BancoContexto();
        CidadeRepository repoCidade = new CidadeRepository();

       
        [ChildActionOnly]
        public ActionResult Index(int id)
        {
            ViewBag.ClienteID = id;
            var endereco = db.Enderecos.Where(a => a.ClienteId == id);

            return PartialView("_Index", endereco.ToList());
        }
  
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Endereco endereco = await db.Enderecos.FindAsync(id);
            if (endereco == null)
            {
                return HttpNotFound();
            }
            return View(endereco);
        }

       
        public ActionResult Create(int clienteId)
        {
            var endereco = new Endereco() {Cep = "86990000",ClienteId = clienteId};
            ViewBag.CidadeID = new SelectList(repoCidade.GetAll().ToList().OrderBy(x => x.Descricao), "ID", "Descricao");
            return PartialView("_Create",endereco);
        }

  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public  ActionResult Create([Bind(Include="ID,Logradouro,Cep,Complemento,Numero,ClienteID,CidadeId")] Endereco endereco)
        {
            if (ModelState.IsValid)
            {
                var cep = endereco.Cep.ApenasNumeros();
                endereco.Cep= cep;
                db.Enderecos.Add(endereco);
                db.SaveChanges();
                return Json(new { success = true });
                
            }

            return PartialView("_Create");
        }

        
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Endereco endereco = await db.Enderecos.FindAsync(id);
            if (endereco == null)
            {
                return HttpNotFound();
            }
            ViewBag.Cidades = repoCidade.GetAll().Select(x => new SelectListItem{Text = x.Descricao,Value = x.Id.ToString(),Selected = x.Id == endereco.CidadeId});
            ViewBag.ClienteID = new SelectList(db.Clientes, "ID", "Nome", endereco.ClienteId);
            return PartialView("_Edit",endereco);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="ID,Logradouro,Cep,Complemento,Numero,ClienteID,CidadeId")] Endereco endereco)
        {
            if (ModelState.IsValid)
            {
                var cep = endereco.Cep.ApenasNumeros();
                endereco.Cep = cep;
                db.Entry(endereco).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return Json(new { success = true });
            }
            ViewBag.ClienteID = new SelectList(db.Clientes, "ID", "Nome", endereco.ClienteId);
            return PartialView("_Edit",endereco);
        }

     
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Endereco endereco = await db.Enderecos.FindAsync(id);
            if (endereco == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Delete",endereco);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Endereco endereco = await db.Enderecos.FindAsync(id);
            db.Enderecos.Remove(endereco);
            await db.SaveChangesAsync();
            return Json(new { success = true });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
