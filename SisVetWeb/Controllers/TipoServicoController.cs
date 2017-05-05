using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Domain.Entidades.Cadastro;
using PagedList;
using Repository.Context;
using Repository.Repositories;

namespace SisVetWeb.Controllers
{
    public class TipoServicoController : Controller
    {
        private BancoContexto db = new BancoContexto();
        private TipoServicoRepository repoTipoServico = new TipoServicoRepository();


        public ActionResult Index(string ordenacao, string pesquisa, string tipoPesquisa, int pagina = 1)
        {
           int totalRegistros = 20;
            ViewBag.IdParam = ordenacao == "Id" ? "Id_Desc" : "Id";
            ViewBag.NomeParam = ordenacao == "Descricao" ? "Descricao_Desc" : "Descricao";
            ViewBag.ValorParam = ordenacao == "Valor" ? "Valor_Desc" : "Valor";

            ViewBag.ordenacaoCorrente = ordenacao;
            ViewBag.tipoPesquisa = tipoPesquisa;
            ViewBag.pesquisaCorrente = pesquisa;

            var tipoServico = from m in repoTipoServico.GetAll().ToList()
                        select m;

            if (!String.IsNullOrEmpty(pesquisa)) {
                tipoServico = tipoServico.Where(s => s.Descricao.ToUpper().Contains(pesquisa.ToUpper()));
            }


            switch (ordenacao) {
                case "Id":
                    tipoServico = tipoServico.OrderBy(x => x.Id);
                    break;
                case "Id_Desc":
                    tipoServico = tipoServico.OrderByDescending(x => x.Id);
                    break;
                case "Descricao":
                    tipoServico = tipoServico.OrderBy(x => x.Descricao);
                    break;
                case "Descricao_Desc":
                    tipoServico = tipoServico.OrderByDescending(x => x.Descricao);
                    break;
                case "Valor":
                    tipoServico = tipoServico.OrderBy(x => x.Valor);
                    break;
                case "Valor_Desc":
                    tipoServico = tipoServico.OrderByDescending(x => x.Valor);
                    break;    
                default:
                    tipoServico = tipoServico.OrderBy(x => x.Id);
                    break;

            }

            var quantidadeRegistros = tipoServico.Count();
            if (!string.IsNullOrEmpty(pesquisa) && quantidadeRegistros > 0)
                totalRegistros = quantidadeRegistros;


            return View(tipoServico.ToPagedList(pagina, totalRegistros));
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoServico tiposervico = repoTipoServico.Find(id);
            if (tiposervico == null)
            {
                return HttpNotFound();
            }
            return View(tiposervico);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Descricao,Valor")] TipoServico tiposervico)
        {
            if (ModelState.IsValid)
            {
                repoTipoServico.Adicionar(tiposervico);
                repoTipoServico.SalvarTodos();
                return RedirectToAction("Index");
            }

            return View(tiposervico);
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoServico tiposervico = repoTipoServico.Find(id);
            if (tiposervico == null)
            {
                return HttpNotFound();
            }
            return View(tiposervico);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Descricao,Valor")] TipoServico tiposervico)
        {
            if (ModelState.IsValid)
            {
                repoTipoServico.Atualizar(tiposervico);
                repoTipoServico.SalvarTodos();
                return RedirectToAction("Index");
            }
            return View(tiposervico);
        }

        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipoServico tiposervico = repoTipoServico.Find(id);
            if (tiposervico == null)
            {
                return HttpNotFound();
            }
            return View(tiposervico);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoServico tiposervico = db.TipoServicos.Find(id);
            repoTipoServico.Excluir(x => x == tiposervico);
            repoTipoServico.SalvarTodos();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                repoTipoServico.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
