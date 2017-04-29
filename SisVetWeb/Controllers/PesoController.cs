using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Domain.Entidades.Cadastro;
using Repository;
using Repository.Context;
using Repository.Repositories;

namespace SisVetWeb.Controllers
{
    public class PesoController : Controller
    {
        private readonly PesoRepository repoPeso = new PesoRepository();
        private readonly AnimalRepository repoAnimal = new AnimalRepository();

         [ChildActionOnly]
        public ActionResult Index(int id) {
            ViewBag.AnimalID = id;
             var peso = repoPeso.GetAll().Where(a => a.AnimalID == id);

            return PartialView("_Index", peso.ToList());
        }


         public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
             Peso peso = repoPeso.Find(id);
            if (peso == null)
            {
                return HttpNotFound();
            }
            return View(peso);
        }

      
        public ActionResult Create(int animalId) {
            var peso = new Peso {AnimalID = animalId};
            return PartialView("_Create", peso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,PesoAnimal,DataCadastro,Observacao,AnimalID")] Peso peso)
        {
            if (ModelState.IsValid) {
                repoPeso.Adicionar(peso);
                repoPeso.SalvarTodos();
                return Json(new { success = true });

            }

            return PartialView("_Create");
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Peso peso = repoPeso.Find(id);
            if (peso == null)
            {
                return HttpNotFound();
            }
            ViewBag.AnimalID = new SelectList(repoAnimal.GetAll(),"ID","Nome",peso.AnimalID);
            return PartialView("_Edit",peso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,PesoAnimal,DataCadastro,Observacao,AnimalID")] Peso peso)
        {
            if (ModelState.IsValid)
            {
                using (var ctx = new BancoContexto())
                {
                     ctx.Entry(peso).State = EntityState.Modified;
                    await ctx.SaveChangesAsync();
                    return Json(new {success = true});
                }
                
            }
            return PartialView("_Edit",peso);
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Peso peso = repoPeso.Find(id);
            if (peso == null)
            {
                return HttpNotFound();
            }
            return PartialView("_Delete",peso);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Peso peso = repoPeso.Find(id);
            repoPeso.Excluir(x => x == peso);
            repoPeso.SalvarTodos();
            return Json(new {success = true});
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
               repoPeso.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
