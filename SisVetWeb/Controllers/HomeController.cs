﻿using System.Web.Mvc;

namespace SisVetWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

   
        public ActionResult Login(string login,string senha)
        {
            if(login == "anjos" && senha == "patas")
            {
                return View("Index");
            }
            return View();
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}