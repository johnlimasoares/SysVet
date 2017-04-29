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
using Microsoft.Ajax.Utilities;
using PagedList;
using Repository.Context;
using Repository;
using Repository.Repositories;

namespace SisVetWeb.Controllers
{
    public class AnimalController : Controller
    {
        private readonly AnimalRepository repoAnimal = new AnimalRepository();
        private RacaRepository repoRaca = new RacaRepository();
        private ClienteRepository repoCliente = new ClienteRepository();
        private TelefoneRepository repoFone = new TelefoneRepository();

        // GET: /Animal/
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, string typeSearch)
        {

            if (String.IsNullOrEmpty(sortOrder))
                sortOrder = "Id";

            ViewBag.CurrentSort = sortOrder;
            ViewBag.IdParam = sortOrder == "Id" ? "Id_Desc" : "Id";
            ViewBag.NomeParam = sortOrder == "Nome" ? "Nome_Desc" : "Nome";

            if (searchString != null) {
                page = 1;
            } else {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var animais = from m in repoAnimal.GetAll().ToList()
                           select m;

            switch (typeSearch)
            {
                case "Id" :
                    animais = animais.Where(s => s.ID.ToString().ToUpper().Contains(searchString.ToString().ToUpper()));
                    break;
                case "Nome":
                    animais = animais.Where(s => s.Nome.ToUpper().Contains(searchString.ToUpper()));
                    break;
                case "Cpf":
                    animais = animais.Where(s => s.Cliente.CpfCnpj.Contains(searchString));
                    break;
                case "Fone":
                    animais = from animal in repoAnimal.GetAll().ToList()
                        join prop in repoCliente.GetAll().ToList() on animal.ClienteId equals prop.ID
                        join fone in repoFone.GetAll().ToList() on prop.ID equals fone.ClienteID
                        where fone.Numero.Contains(searchString)
                        select animal;
                    
                    break;
                default:
                   // animais = animais.Where(s => s.Nome.ToUpper().Contains(searchString.ToUpper()));
                    break;

            }
           

            switch (sortOrder) {
                case "Id":
                     animais = animais.OrderBy(x => x.ID);
                    break;
                case "Id_Desc":
                    animais = animais.OrderByDescending(x => x.ID);
                    break;
                case "Nome":
                    animais = animais.OrderBy(x => x.Nome);
                    break;
                case "Nome_Desc":
                    animais = animais.OrderByDescending(x => x.Nome);
                    break;
                default:
                    animais = animais.OrderBy(x => x.ID);
                    break;

            }
            
            int pageSize = 20;
            int pageNumber = (page ?? 1);

            return View(animais.ToPagedList(pageNumber, pageSize));
        }

        // GET: /Animal/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Animal animal = db.Animais.Find(id);
            Animal animal = repoAnimal.Find(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            return View(animal);
        }

        // GET: /Animal/Create
        public ActionResult Create()
        {
            ViewBag.RacaID = new SelectList(
                repoRaca.GetAll(),
                "ID",
                "Descricao"
                );

            ViewBag.ClienteID = new SelectList(
                repoCliente.GetAll().OrderByDescending(a => a.ID),
                "ID",
                "Nome"
                );
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nome,Pelagem,Observacao,DataNascimento,Castrado,Obito,Sexo")] Animal animal,int racaId,int clienteId)
        {
            var racaID = new Raca() {ID = racaId};
            var clienteID = new Cliente() {ID = clienteId};
            if (ModelState.IsValid){
                using (var ctx = new BancoContexto()){
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

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Animal animal = repoAnimal.Find(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            ViewBag.Racas = repoRaca.GetAll().Select(g => new SelectListItem { Text = g.Descricao, Value = g.ID.ToString(), Selected = animal.RacaId == g.ID });
            ViewBag.Clientes = repoCliente.GetAll().Select(g => new SelectListItem { Text = g.Nome, Value = g.ID.ToString(), Selected = animal.ClienteId == g.ID });

            //ViewBag.RacaID = new SelectList(
            //        repoRaca.GetAll(),
            //        "ID",
            //        "Descricao",
            //        animal.RacaId
            //    );
          
            //ViewBag.ClienteID = new SelectList(
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
        public ActionResult Edit([Bind(Include = "ID,Nome,Pelagem,Observacao,DataNascimento,Castrado,Obito,Sexo")] Animal animal, int racaId, int clienteId)
        {
            if (ModelState.IsValid)
            {

                using (var ctx = new BancoContexto())
                {
                    animal.RacaId = racaId;
                    animal.ClienteId = clienteId;
                    ctx.Entry(animal).State = EntityState.Modified;
                    ctx.SaveChanges();
                }
                
                return RedirectToAction("Index");
            }
            return View(animal);
        }

        // GET: /Animal/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Animal animal = db.Animais.Find(id);
            Animal animal = repoAnimal.Find(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            return View(animal);
        }

        // POST: /Animal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Animal animal = db.Animais.Find(id);
            Animal animal = repoAnimal.Find(id);
            //db.Animais.Remove(animal);
            repoAnimal.Excluir(c => c == animal);
            //db.SaveChanges();
            repoAnimal.SalvarTodos();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repoAnimal.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
