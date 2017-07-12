using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Domain.Entidades.Cadastro.Contato;
using Repository;
using Repository.Context;
using Utils;
using WebGrease.Css.Extensions;

namespace SisVetWeb.Controllers {
    public class TelefoneController : Controller {
        private readonly TelefoneRepository repoFone = new TelefoneRepository();
        private TipoTelefoneRepository repoTipoFone = new TipoTelefoneRepository();
        private ClienteRepository repoCliente = new ClienteRepository();

        [ChildActionOnly]
        public ActionResult Index(int id) {
            ViewBag.ClienteID = id;
            var listTelefone = repoFone.GetAll().Where(a => a.ClienteId == id).ToList();
            for (var index = 0; index <= listTelefone.Count - 1; index++) {
                listTelefone[index].Numero = listTelefone[index].Numero.FormatFone();
            }
            return PartialView("_Index", listTelefone);
        }


        public async Task<ActionResult> Details(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Telefone telefone = repoFone.Find(id);
            if (telefone == null) {
                return HttpNotFound();
            }
            return View(telefone);
        }


        public ActionResult Create(int clienteID) {
            ViewBag.TipoTelefoneID = new SelectList
                (
                repoTipoFone.GetAll(),
                "ID",
                "Descricao"
                );
            Telefone telefone = new Telefone();
            telefone.ClienteId = clienteID;
            return PartialView("_Create", telefone);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Numero,ClienteId")] Telefone telefone, int tipoTelefoneId) {
            var tipoTelefoneID = new TipoTelefone() { Id = tipoTelefoneId };
            if (ModelState.IsValid) {
                using (var ctx = new BancoContexto()) {
                    telefone.Numero = telefone.Numero.ApenasNumeros(); ;
                    ctx.Entry(tipoTelefoneID).State = EntityState.Unchanged;
                    telefone.TipoTelefone = tipoTelefoneID;
                    ctx.Telefones.Add(telefone);
                    ctx.SaveChanges();
                    return Json(new { success = true });
                }

            }
            return PartialView("_Create");
        }

        public async Task<ActionResult> Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Telefone telefone = repoFone.Find(id);
            if (telefone == null) {
                return HttpNotFound();
            }
            var fone = telefone.Numero.ApenasNumeros();
            if (fone.Length < 10)
                fone = fone.PadLeft(10, '4');

            telefone.Numero = fone;
            ViewBag.ClienteID = new SelectList(repoCliente.GetAll(), "ID", "Nome", telefone.ClienteId);
            ViewBag.TipoTelefones =
                repoTipoFone.GetAll()
                    .Select(
                        x =>
                            new SelectListItem {
                                Text = x.Descricao,
                                Value = x.Id.ToString(),
                                Selected = telefone.TipoTelefoneId == x.Id
                            });
            return PartialView("_Edit", telefone);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Numero,ClienteId,Cliente,TipoTelefone")] Telefone telefone, int tipoTelefoneId) {
            if (ModelState.IsValid) {
                using (var ctx = new BancoContexto()) {
                    telefone.Numero = telefone.Numero.ApenasNumeros(); ;
                    telefone.TipoTelefoneId = tipoTelefoneId;
                    ctx.Entry(telefone).State = EntityState.Modified;
                    await ctx.SaveChangesAsync();
                    return Json(new { success = true });
                }

            }
            ViewBag.ClienteID = new SelectList(repoFone.GetAll(), "ID", "Numero", telefone.ClienteId);
            return PartialView("_Edit", telefone);

        }


        public async Task<ActionResult> Delete(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Telefone telefone = repoFone.Find(id);
            if (telefone == null) {
                return HttpNotFound();
            }
            return PartialView("_Delete", telefone);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id) {
            Telefone telefone = repoFone.Find(id);
            repoFone.Excluir(x => x == telefone);
            repoFone.SalvarTodos();
            return Json(new { success = true });
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                repoFone.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
