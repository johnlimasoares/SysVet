using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Domain.Entidades.Cadastro;
using PagedList;
using Repository;
using Repository.Repositories;
using Utils;

namespace SisVetWeb.Controllers {
    public class ClienteController : Controller {
        private readonly ClienteRepository repoCliente = new ClienteRepository();
        private readonly AnimalRepository repoAnimal = new AnimalRepository();
        private readonly TelefoneRepository repoFone = new TelefoneRepository();


        public ActionResult Index(string ordenacao, string pesquisa, string tipoPesquisa, int pagina = 1) {

            int totalRegistros = 20;
            ViewBag.IdParam = ordenacao == "Id" ? "Id_Desc" : "Id";
            ViewBag.NomeParam = ordenacao == "Nome" ? "Nome_Desc" : "Nome";

            ViewBag.ordenacaoCorrente = ordenacao;
            ViewBag.tipoPesquisa = tipoPesquisa;
            ViewBag.pesquisaCorrente = pesquisa;

            var clientes = repoCliente.GetAllClientes(ordenacao, pesquisa, tipoPesquisa);

            var quantidadeRegistros = clientes.Count();
            if (!string.IsNullOrEmpty(pesquisa) && quantidadeRegistros > 0)
                totalRegistros = quantidadeRegistros;

            return View(clientes.ToPagedList(pagina, totalRegistros));
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

            var animaisCliente = repoAnimal.GetAll().Where(x => x.Cliente.Id == id).ToList();

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
