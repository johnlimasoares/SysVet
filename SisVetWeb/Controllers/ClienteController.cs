using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;
using Business.Cliente;
using Domain.Entidades.Cadastro;
using Domain.Entidades.Cadastro.Contato;
using Domain.Entidades.Cadastro.Localidade;
using Microsoft.Ajax.Utilities;
using PagedList;
using Repository.Repositories;
using SisVetWeb.Models;
using Utils;

namespace SisVetWeb.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index(string ordenacao, string pesquisa, string tipoPesquisa, int pagina = 1)
        {

            int totalRegistros = 20;
            ViewBag.IdParam = ordenacao == "Id" ? "Id_Desc" : "Id";
            ViewBag.NomeParam = ordenacao == "Nome" ? "Nome_Desc" : "Nome";

            ViewBag.ordenacaoCorrente = ordenacao;
            ViewBag.tipoPesquisa = tipoPesquisa;
            ViewBag.pesquisaCorrente = pesquisa;

            var clientes = new ClienteRepository().GetAllClientes(ordenacao, pesquisa, tipoPesquisa);

            var quantidadeRegistros = clientes.Count();
            if (!string.IsNullOrEmpty(pesquisa) && quantidadeRegistros > 0)
                totalRegistros = quantidadeRegistros;

            return View(clientes.ToPagedList(pagina, totalRegistros));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = new ClienteRepository().Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        public ActionResult DetailsOwnerAnimal(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Cliente cliente = new ClienteRepository().Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View("_DetailsOwnerAnimal", cliente);
        }

        public ActionResult Create()
        {
            ViewBag.TipoTelefoneID = new SelectList(new TipoTelefoneRepository().GetAll(), "ID", "Descricao");
            ViewBag.CidadeID = new SelectList(new CidadeRepository().GetAll().ToList().OrderBy(x => x.Descricao), "ID", "Descricao");
            return View();
        }


        [HttpPost]
        public ActionResult Create(ClienteViewModel clienteViewModel)
        {
            Endereco endereco = null;
            Telefone telefone = null;

            var cliente = new Cliente();
            cliente.Nome = clienteViewModel.Nome;
            cliente.CpfCnpj = clienteViewModel.CpfCnpj.ApenasNumeros();
            cliente.RgIe = clienteViewModel.RgIe;
            cliente.Sexo = clienteViewModel.Sexo;
            cliente.DataNascimento = clienteViewModel.DataNascimento;
            cliente.DataCadastro = DateTime.Now;
            cliente.Email = clienteViewModel.Email;
            cliente.TipoPessoa = clienteViewModel.TipoPessoa;

            if (!clienteViewModel.Rua.IsNullOrWhiteSpace() &&
                !clienteViewModel.CidadeId.ToString().IsNullOrWhiteSpace())
            {
                endereco = new Endereco();
                endereco.Cep = clienteViewModel.Cep;
                endereco.Logradouro = clienteViewModel.Rua;
                endereco.CidadeId = clienteViewModel.CidadeId;
                endereco.Numero = clienteViewModel.Numero;
                endereco.Complemento = clienteViewModel.Complemento;
            }

            if (!clienteViewModel.NumeroTelefone.IsNullOrWhiteSpace() &&
                !clienteViewModel.TipoTelefoneId.ToString().IsNullOrWhiteSpace())
            {
                telefone = new Telefone();
                telefone.Numero = clienteViewModel.NumeroTelefone.ApenasNumeros();
                telefone.TipoTelefoneId = clienteViewModel.TipoTelefoneId;
            }

            var clienteId = ClienteBusiness.Save(cliente, endereco, telefone);
            var routeValues = new RouteValueDictionary();
            routeValues.Add("clienteId", clienteId);
            return RedirectToAction("Create", "Animal", routeValues);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var cliente = new ClienteRepository().Find(id);

            if (cliente == null)
            {
                return HttpNotFound();
            }
            var cpf = cliente.CpfCnpj.ApenasNumeros();
            cliente.CpfCnpj = cpf;
            return View(cliente);
        }

        [HttpPost]
        public ActionResult Edit(Cliente cliente)
        {
            var cpf = cliente.CpfCnpj.ApenasNumeros();
            cliente.CpfCnpj = cpf;
            var repoCliente = new ClienteRepository();
            repoCliente.Atualizar(cliente);
            repoCliente.SalvarTodos();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult Delete(int id)
        {
            if (ClienteBusiness.HaRegistrosFinanceiroDoCliente(id)){
                Response.StatusCode = (int)HttpStatusCode.Conflict;
                return Json("Cliente possui registros financeiros efetuados. Não permitido sua exclusão.", JsonRequestBehavior.AllowGet);
            }
            
            string mensagem;
            var cliente = new ClienteRepository().Excluir(id);
            mensagem = string.Format("{0} excluído com sucesso.", cliente.Nome);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(mensagem, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ListaAnimaisPorCliente(int id, int? page, string currentFilter, string searchString)
        {
            var animaisCliente = new AnimalRepository().GetAll().Where(x => x.Cliente.Id == id).ToList();
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(animaisCliente.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Error(Exception ex){
            return View("Error");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                new ClienteRepository().Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
