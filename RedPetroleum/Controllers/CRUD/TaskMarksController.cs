using RedPetroleum.Models.Entities;
using RedPetroleum.Models.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace RedPetroleum.Controllers.CRUD
{
    public class TaskMarksController : Controller
    {
        private UnitOfWork unitOfWork;

        public TaskMarksController() => this.unitOfWork = new UnitOfWork();

        public TaskMarksController(UnitOfWork unit) => this.unitOfWork = unit;

        // GET: TaskMarks
        public ActionResult Index()
        {
            ViewBag.EmployeeId = new SelectList(unitOfWork.Employees.GetAll(), "EmployeeId", "EFullName");

            return View(unitOfWork.TaskMarks.GetAll());
        }

        // GET: TaskMarks/Details/5
        public ActionResult Details(Guid id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskMark tmark = unitOfWork.TaskMarks.Get(id);
            if (tmark == null)
            {
                return HttpNotFound();
            }
            return View(tmark);
        }

        // GET: TaskMarks/Create
        public ActionResult Create()
        {
            

            return View();
        }

        // POST: TaskMarks/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TaskMarks/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: TaskMarks/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: TaskMarks/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: TaskMarks/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
