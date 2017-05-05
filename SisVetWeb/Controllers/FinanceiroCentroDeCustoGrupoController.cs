using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repository.Repositories;

namespace SisVetWeb.Controllers
{
    public class FinanceiroCentroDeCustoGrupoController : Controller
    {
        private FinanceiroCentroDeCustoGrupoRepository repoCentroCusto = new FinanceiroCentroDeCustoGrupoRepository();
        public ActionResult Index(){
            var listCentroCusto = repoCentroCusto.GetAll();
            return View(listCentroCusto);
        }
	}
}