using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RedPetroleum.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using X.PagedList;

namespace RedPetroleum.Controllers.CRUD
{
    public class RolesController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Roles
        public ActionResult Index(int? page, string searching)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var roles = db.Roles.Where(x => x.Name.Contains(searching) || searching == null).OrderBy(x => x.Name);
            return View(roles.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var role = db.Roles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IdentityRole role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(role).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(role);
            }
            catch (DbEntityValidationException ex)
            {
                if (ex.TargetSite.Name == "SaveChanges")
                {
                    ViewBag.Message = "Такая запись уже существует!";
                    return View(role);
                }
                else
                {
                    throw;
                }
            }
        }

        //Edit
        public ActionResult Create()
        {
            return View();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IdentityRole role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Roles.Add(role);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(role);
            }
            catch (DbEntityValidationException ex)
            {
                if (ex.TargetSite.Name == "SaveChanges")
                {
                    ViewBag.Message = "Такая запись уже существует!";
                    return View(role);
                }
                else
                {
                    throw;
                }
            }
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.Roles.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View("Index");
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var role = db.Roles.Find(id);
            if (role != null)
            {
                db.Roles.Remove(role);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        //public async Task<ActionResult> Details(Guid? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Department department = await unitOfWork.Departments.GetAsync(id);
        //    if (department == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(department);
        //}
    }
}