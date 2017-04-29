using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Domain.Entidades;
using Domain.Entidades.Cadastro;
using PagedList;
using Repository;
using Repository.Context;
using Repository.Repositories;
using Utils;

namespace SisVetWeb.Controllers {
    public class ClienteController : Controller {
        private readonly ClienteRepository repoCliente = new ClienteRepository();
        private readonly AnimalRepository repoAnimal = new AnimalRepository();
        private readonly TelefoneRepository repoFone = new TelefoneRepository();


        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, string typeSearch) {

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

            var clientes = from m in repoCliente.GetAll().ToList()
                           select m;

            switch (typeSearch) {
                case "Nome":
                    clientes = clientes.Where(s => s.Nome.ToUpper().Contains(searchString.ToUpper()));
                    break;
                case "Cpf":
                    clientes = clientes.Where(s => s.CpfCnpj.Contains(searchString));
                    break;
                case "Fone":
                    clientes = from cliente in repoCliente.GetAll().ToList()
                               join fone in repoFone.GetAll().ToList() on cliente.ID equals fone.ClienteID
                               where fone.Numero.Contains(searchString)
                               select cliente;
                    break;
            }

            switch (sortOrder) {
                case "Id":
                    clientes = clientes.OrderBy(x => x.ID);
                    break;
                case "Id_Desc":
                    clientes = clientes.OrderByDescending(x => x.ID);
                    break;
                case "Nome":
                    clientes = clientes.OrderBy(x => x.Nome);
                    break;
                case "Nome_Desc":
                    clientes = clientes.OrderByDescending(x => x.Nome);
                    break;
                default:
                    clientes = clientes.OrderBy(x => x.ID);
                    break;

            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);

            return View(clientes.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult Details(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = repoCliente.Find(id);
            if (cliente == null) {
                return HttpNotFound();
            }
            return View(cliente);
        }

        public ActionResult DetailsOwnerAnimal(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cliente cliente = repoCliente.Find(id);
            if (cliente == null) {
                return HttpNotFound();
            }
            return View("_DetailsOwnerAnimal", cliente);
        }


        public ActionResult Create() {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nome,CpfCnpj,RgIe,TipoPessoa,DataNascimento,DataCadastro,Email,Sexo")] Cliente cliente) {
            if (ModelState.IsValid) {
                var cpf = cliente.CpfCnpj.ApenasNumeros();
                cliente.CpfCnpj = cpf;
                repoCliente.Adicionar(cliente);
                repoCliente.SalvarTodos();
                return RedirectToAction("Index");
            }
            return View(cliente);
        }


        public ActionResult Edit(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = repoCliente.Find(id);

            if (cliente == null) {
                return HttpNotFound();
            }
            var cpf = cliente.CpfCnpj.ApenasNumeros();
            cliente.CpfCnpj = cpf;
            return View(cliente);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,CpfCnpj,RgIe,TipoPessoa,DataNascimento,DataCadastro,Email,Sexo")] Cliente cliente) {
            if (ModelState.IsValid) {
                var cpf = cliente.CpfCnpj.ApenasNumeros();
                cliente.CpfCnpj = cpf;
                repoCliente.Atualizar(cliente);
                repoCliente.SalvarTodos();
                return RedirectToAction("Index");
            }
            return View(cliente);
        }


        public ActionResult Delete(int? id) {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = repoCliente.Find(id);
            if (cliente == null) {
                return HttpNotFound();
            }
            return View(cliente);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id) {
            Cliente cliente = repoCliente.Find(id);
            repoCliente.Excluir(c => c == cliente);
            repoCliente.SalvarTodos();
            return RedirectToAction("Index");
        }


        public ActionResult ListaAnimaisPorCliente(int id, int? page, string currentFilter, string searchString) {

            var animaisCliente = repoAnimal.GetAll().Where(x => x.Cliente.ID == id).ToList();

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(animaisCliente.ToPagedList(pageNumber, pageSize));
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                repoCliente.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
