using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Domain.Entidades;
using Domain.Entidades.Cadastro;
using Repository;
using Repository.Context;
using Repository.Repositories;

namespace SisVetWeb.Controllers
{
    public class ServicoController : Controller
    {
        private ServicoRepository repoServico = new ServicoRepository();
        private TipoServicoRepository repoTipoServico = new TipoServicoRepository();
        private AtendimentoRepository repoAtendimento = new AtendimentoRepository();

        [ChildActionOnly]
        public ActionResult Index(int id)
        {
            ViewBag.AtendimentoID = id;
            var atendimentos = repoServico.GetAll().Where(a => a.AtendimentoId == id);
            return PartialView("_Index",atendimentos.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servico servico = repoServico.Find(id);
            if (servico == null)
            {
                return HttpNotFound();
            }
            return View(servico);
        }

        public ActionResult Create(int atendimentoID)
        {
            Servico servico = new Servico();
            servico.AtendimentoId = atendimentoID;
            ViewBag.TipoServicoID = new SelectList(repoTipoServico.GetAll(), "ID", "Descricao");
            return PartialView("_Create", servico);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,TipoServicoID,Valor,Observacao,AtendimentoID")] Servico servico,int tipoServicoId)
        {
            var tipoServico = new TipoServico();

            if (ModelState.IsValid)
            {
                using (var ctx = new BancoContexto())
                {
                    ctx.Entry(tipoServico).State = EntityState.Unchanged;
                    //servico.TipoServico = tipoServico;
                    ctx.Servicos.Add(servico);
                    ctx.SaveChanges();
                    return Json(new {success = true});
                }
                
            }
            
            return PartialView("_Create");
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servico servico = repoServico.Find(id);
            if (servico == null)
            {
                return HttpNotFound();
            }
            ViewBag.AtendimentoID = new SelectList(repoAtendimento.GetAll(), "ID", "ID",servico.AtendimentoId);
            ViewBag.TipoServicoID = new SelectList(repoTipoServico.GetAll(), "ID", "Descricao", servico.TipoServicoId);
            return PartialView("_Edit",servico);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="ID,TipoServicoID,Valor,Observacao,AtendimentoID")] Servico servico)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new BancoContexto())
                {
                    
                    ctx.Entry(servico).State = EntityState.Modified;
                    await ctx.SaveChangesAsync();
                    return Json(new {success = true});
                }
            }
            ViewBag.AtendimentoID = new SelectList(repoAtendimento.GetAll(), "ID", "ID", servico.AtendimentoId);
            return PartialView("_Edit",servico);

        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servico servico = repoServico.Find(id);
            if (servico == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Delete",servico);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Servico servico = repoServico.Find(id);
            repoServico.Excluir(x => x == servico);
            repoServico.SalvarTodos();
            return Json(new {success = true});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repoServico.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
