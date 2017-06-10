using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Domain.Entidades;
using Domain.Entidades.Cadastro;
using Domain.Entidades.Operacao;
using Domain.Entidades.Operacao.Atendimento;
using Domain.Enum;
using PagedList;
using Repository;
using Repository.Context;
using Repository.Repositories;

namespace SisVetWeb.Controllers
{
    public class AtendimentoController : Controller
    {
        private readonly AtendimentoRepository repoAtendimento = new AtendimentoRepository();
        private readonly AnimalRepository repoAnimal = new AnimalRepository();
        private readonly ServicoRepository repoServico = new ServicoRepository();
        private readonly ClienteRepository repoCliente = new ClienteRepository();
        private readonly TelefoneRepository repoFone = new TelefoneRepository();
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page, string typeSearch) {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NomeParam = String.IsNullOrEmpty(sortOrder) ? "Nome_Animal" : "";

            if (searchString != null) {
                page = 1;
            } else {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var atendimentos = from m in repoAtendimento.GetAll().ToList().OrderBy(c => c.Id)
                          select m;

          if(!string.IsNullOrEmpty(typeSearch))
            switch (typeSearch) {
                case "NomeAnimal":
                    atendimentos = atendimentos.Where(s => s.Animal.Nome.ToUpper().Contains(searchString.ToUpper())).OrderBy(s => s.Id);
                    break;
                case "Cpf":
                    atendimentos = from atendimento in repoAtendimento.GetAll().ToList()
                        join animal in repoAnimal.GetAll().ToList() on atendimento.AnimalId equals animal.Id
                        join cliente in repoCliente.GetAll().ToList() on animal.ClienteId equals cliente.Id
                        where cliente.CpfCnpj.Contains(searchString)
                        select atendimento;                   
                   break;
                case "Fone":
                    atendimentos = from atendimento in repoAtendimento.GetAll().ToList()
                        join animal in repoAnimal.GetAll().ToList() on atendimento.AnimalId equals animal.Id
                        join cliente in repoCliente.GetAll().ToList() on animal.ClienteId equals cliente.Id
                        join fone in repoFone.GetAll().ToList() on cliente.Id equals fone.ClienteId
                        where fone.Numero.Contains(searchString)
                        select atendimento;
                    break;
                case "Atendimento":
                    atendimentos = atendimentos.Where(s => s.Id.ToString().Equals(searchString));
                    break;
                case "NomeCliente":
                    atendimentos = atendimentos.Where(s => s.Animal.Cliente.Nome.ToUpper().Contains(searchString.ToUpper())).OrderBy(s => s.Id);
                    break;
                default:
                    break;

            }

            atendimentos = atendimentos.OrderByDescending(x => x.Id);
            int pageSize = 15;
            int pageNumber = (page ?? 1);

            return View(atendimentos.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Atendimento atendimento = repoAtendimento.Find(id);
            if (atendimento == null)
            {
                return HttpNotFound();
            }
            ViewBag.AnimalID = new SelectList(repoAnimal.GetAll(), "ID", "ID", atendimento.AnimalId);
            return View(atendimento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details(Atendimento atendimento)
        {
            if(ModelState.IsValid)
            {
                atendimento.DataSaida = DateTime.Now.Date;
                atendimento.Situacao = SituacaoAtendimento.Fechado.ToString();
                repoAtendimento.Atualizar(atendimento);
                repoAtendimento.SalvarTodos();
                return RedirectToAction("Index");
            }
            return View(atendimento);
        }

        public ActionResult Create()
        {
            ViewBag.AnimalID = new SelectList(repoAnimal.GetAll(), "ID", "ID");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DataEntrada,DataSaida,HoraAtendimento,Situacao,ValorAtendimento,ValorDesconto,Observacao")] Atendimento atendimento, int animalId)
        {
            var Animal= new Animal() {Id = animalId};

            if (ModelState.IsValid)
            {
                using (var ctx = new BancoContexto())
                {
                    ctx.Entry(Animal).State = EntityState.Unchanged;
                    atendimento.Animal = Animal;
                    atendimento.DataEntrada = DateTime.Now.Date;
                    atendimento.HoraAtendimento = new TimeSpan(DateTime.Now.Hour,DateTime.Now.Minute,DateTime.Now.Second);
                    atendimento.DataSaida = null;
                    atendimento.ValorAtendimento = 0.00;
                    atendimento.Situacao = SituacaoAtendimento.Aberto.ToString();
                    ctx.Atendimentos.Add(atendimento);
                    ctx.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            return View(atendimento);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Atendimento atendimento = repoAtendimento.Find(id);
            if (atendimento == null)
            {
                return HttpNotFound();
            }
            ViewBag.AnimalID = new SelectList(repoAnimal.GetAll(), "ID", "ID",atendimento.AnimalId);
            return View(atendimento);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,DataEntrada,DataSaida,HoraAtendimento,Situacao,ValorAtendimento,ValorDesconto,Observacao")] Atendimento atendimento, int animalId)
        {
            List<Servico> totalVenda = repoServico.GetAll().Where(x => x.AtendimentoId == atendimento.Id).ToList();
            var totalAtendimento = totalVenda.Sum(soma => soma.TipoServico.Valor);
            if (ModelState.IsValid)
            {
                atendimento.AnimalId = animalId;
                atendimento.ValorAtendimento = totalAtendimento;
                repoAtendimento.Atualizar(atendimento);

                repoAtendimento.SalvarTodos();
                return RedirectToAction("Index");
            }
            return View(atendimento);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Atendimento atendimento = repoAtendimento.Find(id);
            if (atendimento == null)
            {
                return HttpNotFound();
            }
            return View(atendimento);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Atendimento atendimento = repoAtendimento.Find(id);
            repoAtendimento.Excluir(x => x == atendimento);
            repoAtendimento.SalvarTodos();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repoAtendimento.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}