using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Domain.Entidades.Cadastro;
using PagedList;
using Repository.Context;
using Repository;
using Repository.Repositories;

namespace SisVetWeb.Controllers {
    public class AnimalController : Controller {
        private readonly AnimalRepository repoAnimal = new AnimalRepository();
        private RacaRepository repoRaca = new RacaRepository();
        private ClienteRepository repoCliente = new ClienteRepository();

        public ActionResult Index(string ordenacao, string pesquisa, string tipoPesquisa, int pagina = 1) {

            int totalRegistros = 20;
            ViewBag.IdParam = ordenacao == "Id" ? "Id_Desc" : "Id";
            ViewBag.NomeParam = ordenacao == "Nome" ? "Nome_Desc" : "Nome";

            ViewBag.ordenacaoCorrente = ordenacao;
            ViewBag.tipoPesquisa = tipoPesquisa;
            ViewBag.pesquisaCorrente = pesquisa;

            var animais = repoAnimal.GetAllAnimais(ordenacao, pesquisa, tipoPesquisa);

            var quantidadeRegistros = animais.Count();
            if (!string.IsNullOrEmpty(pesquisa) && quantidadeRegistros > 0)
                totalRegistros = quantidadeRegistros;

            return View(animais.ToPagedList(pagina, totalRegistros));
        }

        public ActionResult Details(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Animal animal = db.Animais.Find(id);
            Animal animal = repoAnimal.Find(id);
            if (animal == null) {
                return HttpNotFound();
            }
            return View(animal);
        }

        public ActionResult Create(int? clienteId) {
            ViewBag.RacaID = new SelectList(
                repoRaca.GetAll().OrderBy(x => x.Descricao),
                "ID",
                "Descricao"
                );

            ViewBag.ClienteID = new SelectList(
                repoCliente.GetAll().OrderBy(a => a.Nome),
                "ID",
                "Nome"
                );
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nome,Pelagem,Observacao,DataNascimento,Castrado,Obito,Sexo")] Animal animal, int racaId, int clienteId) {
            var racaID = new Raca() { Id = racaId };
            var clienteID = new Cliente() { Id = clienteId };
            if (ModelState.IsValid) {
                using (var ctx = new BancoContexto()) {
                    ctx.Entry(racaID).State = EntityState.Unchanged;
                    ctx.Entry(clienteID).State = EntityState.Unchanged;
                    animal.Raca = racaID;
                    animal.Cliente = clienteID;
                    ctx.Animais.Add(animal);
                    ctx.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            return View(animal);
        }

        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Animal animal = repoAnimal.Find(id);
            if (animal == null) {
                return HttpNotFound();
            }
            ViewBag.Racas = repoRaca.GetAll().OrderBy(x => x.Descricao).Select(g => new SelectListItem { Text = g.Descricao, Value = g.Id.ToString(), Selected = animal.RacaId == g.Id });
            ViewBag.Clientes = repoCliente.GetAll().OrderBy(x => x.Nome).Select(g => new SelectListItem { Text = g.Nome, Value = g.Id.ToString(), Selected = animal.ClienteId == g.Id });

            //ViewBag.RacaID = new SelectList(
            //        repoRaca.GetAll(),
            //        "ID",
            //        "Descricao",
            //        animal.RacaId
            //    );

            //ViewBag.ClienteId = new SelectList(
            //    repoCliente.GetAll(),
            //    "ID",
            //    "Nome",
            //    animal.ClienteId
            //    );

            //ViewBag.Genres = storeDB.Genres
            //.OrderBy(g => g.Name)
            //.AsEnumerable()
            //.Select(g => new SelectListItem {
            //Text = g.Name,
            //Value = g.GenreId.ToString(),
            //Selected = album.GenreId == g.GenreId
            //});
            return View(animal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Pelagem,Observacao,DataNascimento,Castrado,Obito,Sexo")] Animal animal, int racaId, int clienteId) {
            if (ModelState.IsValid) {

                using (var ctx = new BancoContexto()) {
                    animal.RacaId = racaId;
                    animal.ClienteId = clienteId;
                    ctx.Entry(animal).State = EntityState.Modified;
                    ctx.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            return View(animal);
        }

        [HttpPost]
        public JsonResult Delete(int id) {
            string mensagem = string.Empty;
            var animal = repoAnimal.Excluir(id);
            mensagem = string.Format("{0} excluido com sucesso", animal.Nome);
            TempData["success"] = "Mensagem de sucesso!!";
            return Json(mensagem, JsonRequestBehavior.AllowGet);
        }
        
        protected override void Dispose(bool disposing) {
            if (disposing) {
                repoAnimal.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
