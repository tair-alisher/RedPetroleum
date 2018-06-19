using RedPetroleum.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RedPetroleum.Controllers
{
    public class HomeController : Controller
    {
        PetroleumContext db = new PetroleumContext();
        public ActionResult Index()
        {
            Position p = new Position();
            p.PositionId = Guid.NewGuid();
            p.Name = "gg";
            db.Positions.Add(p);
            db.SaveChanges();
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