using System;
using System.Web.Mvc;

using RedPetroleum.Models.UnitOfWork;
using RedPetroleum.Models.Entities;


namespace RedPetroleum.Controllers
{
    public class HomeController : Controller
    {
        UnitOfWork unit;
        public HomeController(UnitOfWork unit)
        {
            this.unit = unit;
        }
        public HomeController()
        {
            this.unit = new UnitOfWork();
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}