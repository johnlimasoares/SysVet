using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Domain.Entidades.Operacao;
using Domain.WidGet;
using Repository.Context;
using Utils;
using Repository.Repositories;

namespace SisVetWeb.Controllers {
    public class VacinacaoController : Controller {
        private VacinacaoRepository repoVacinacao = new VacinacaoRepository();
        private BancoContexto db = new BancoContexto();
        private AnimalRepository repoAnimal = new AnimalRepository();
        private VacinaRepository repoVacina = new VacinaRepository();

        [ChildActionOnly]
        public ActionResult Index(int id) {
            ViewBag.AnimalID = id;
            var vacinacao = repoVacinacao.GetAll().Where(a => a.AnimalId == id);
            return PartialView("_Index", vacinacao.ToList());
        }

        public ActionResult WidGetListVacinacao() {
            //const int dezDias = 10;
            //var hoje = DateTime.Today;
            //var decimoDia = hoje.AddDays(dezDias);
            //var proximasVacinas = repoVacinacao.GetAll().Where(a => a.DataPrevisao >= hoje && a.DataPrevisao <= decimoDia && a.DataVacinacao == null);
            //return PartialView("WidGet/_WidGetListVacinacao", proximasVacinas.OrderBy(a => a.DataPrevisao).ToList());
            var listVacinacoes = (List<VacinacaoAVencerWidGet>)repoVacinacao.GetAllVacinacoesAVencerDapper();
            return PartialView("WidGet/_WidGetListVacinacao", SetMascaraTelefone(listVacinacoes));
        }

        public ActionResult WidGetListVacinacaoVencidas() {
            var vacinasVencidas = (List<VacinacaoVencidasWidGet>)repoVacinacao.GetAllVacinacoesVencidasDapper();
            return PartialView("WidGet/_WidGetListVacinacaoVencidas", SetMascaraTelefone(vacinasVencidas));
        }

        public ActionResult Details(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacinacao vacinacao = repoVacinacao.Find(id);
            if (vacinacao == null) {
                return HttpNotFound();
            }
            return View(vacinacao);
        }

        public ActionResult Create(int AnimalID) {
            var vacinacao = new Vacinacao();
            vacinacao.AnimalId = AnimalID;
            ViewBag.VacinaId = new SelectList(repoVacina.GetAll().ToList(), "ID", "Descricao");
            ViewBag.AnimalId = new SelectList(repoAnimal.GetAll().ToList(), "ID", "Nome");
            return PartialView("_Create", vacinacao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DataPrevisao,DataVacinacao,VacinaId,AnimalId")] Vacinacao vacinacao) {
            if (ModelState.IsValid) {
                var vacina = repoVacina.Find(vacinacao.VacinaId);
                for (var dose = 1; dose <= vacina.Doses; dose++) {
                    if (dose == 1) {
                        vacinacao.DataPrevisao = DateTime.Now.Date;
                        vacinacao.DataVacinacao = DateTime.Now.Date;
                    } else {
                        vacinacao.DataPrevisao = DateTime.Now.Date.AddDays(vacina.IntervaloDias * (dose - 1));
                        vacinacao.DataVacinacao = null;

                    }
                    repoVacinacao.Adicionar(vacinacao);
                    repoVacinacao.SalvarTodos();
                }

                return Json(new { success = true });
            }


            return PartialView("_Create");
        }

        public async Task<ActionResult> Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacinacao vacinacao = await db.Vacinacoes.FindAsync(id);
            if (vacinacao == null) {
                return HttpNotFound();
            }
            ViewBag.AnimalId = new SelectList(repoAnimal.GetAll(), "ID", "Nome", vacinacao.AnimalId);
            ViewBag.VacinaId =
                repoVacina.GetAll()
                    .Select(
                        a =>
                            new SelectListItem() {
                                Text = a.Descricao,
                                Value = a.Id.ToString(),
                                Selected = a.Id == vacinacao.VacinaId
                            });
            return PartialView("_Edit", vacinacao);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,DataPrevisao,DataVacinacao,VacinaId,AnimalId")] Vacinacao vacinacao) {
            if (ModelState.IsValid) {
                using (var ctx = new BancoContexto()) {
                    ctx.Entry(vacinacao).State = EntityState.Modified;
                    await ctx.SaveChangesAsync();
                    return Json(new { success = true });
                }

            }
            return PartialView("_Edit", vacinacao);
        }

        public async Task<ActionResult> Delete(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vacinacao vacinacao = db.Vacinacoes.Find(id);
            if (vacinacao == null) {
                return HttpNotFound();
            }
            return PartialView("_Delete", vacinacao);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            Vacinacao vacinacao = repoVacinacao.Find(id);
            repoVacinacao.Excluir(x => x == vacinacao);
            repoVacinacao.SalvarTodos();
            return Json(new { success = true });
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                repoVacinacao.Dispose();
            }
            base.Dispose(disposing);
        }


        private List<VacinacaoVencidasWidGet> SetMascaraTelefone(List<VacinacaoVencidasWidGet> listVacinacaoVencidasWidGet) {
            try {
                for (var index = 0; index <= listVacinacaoVencidasWidGet.Count - 1; index++) {
                    listVacinacaoVencidasWidGet[index].NumeroTelefone =
                        listVacinacaoVencidasWidGet[index].NumeroTelefone.ApenasNumeros().FormatFone();
                }
            } catch (Exception) {
                throw;
            }
            return listVacinacaoVencidasWidGet;
        }

        private List<VacinacaoAVencerWidGet> SetMascaraTelefone(List<VacinacaoAVencerWidGet> listVacinacaoAVencerWidGet) {
            try {
                for (var index = 0; index <= listVacinacaoAVencerWidGet.Count - 1; index++) {
                    listVacinacaoAVencerWidGet[index].NumeroTelefone =
                        listVacinacaoAVencerWidGet[index].NumeroTelefone.ApenasNumeros().FormatFone();
                }
            } catch (Exception) {
                throw;
            }
            return listVacinacaoAVencerWidGet;
        }
    }
}
