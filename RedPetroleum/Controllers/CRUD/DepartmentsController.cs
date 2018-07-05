using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RedPetroleum.Models;
using RedPetroleum.Models.Entities;
using RedPetroleum.Models.UnitOfWork;
using X.PagedList;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace RedPetroleum.Controllers.CRUD
{
    public class DepartmentsController : Controller
    {
        UnitOfWork unitOfWork;

        public DepartmentsController() => this.unitOfWork = new UnitOfWork();

        public DepartmentsController(UnitOfWork unit) => this.unitOfWork = unit;

        // GET: Departments
        public ActionResult Index(int? page, string searching)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var departments = unitOfWork.Departments.GetAllIndex(pageNumber, pageSize, searching);
            return View(departments);
        }

        // GET: Departments/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = await unitOfWork.Departments.GetAsync(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: Departments/Create
        public ActionResult Create()
        {
            ViewBag.ParentId = new SelectList(unitOfWork.Departments.GetAll(), "DepartmentId", "Name");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "DepartmentId,Name,ParentId")] Department department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    department.DepartmentId = Guid.NewGuid();
                    unitOfWork.Departments.Create(department);
                    await unitOfWork.SaveAsync();
                    return RedirectToAction("Index");
                }

                ViewBag.ParentId = new SelectList(unitOfWork.Departments.GetAll(), "DepartmentId", "Name", department.ParentId);
                return View(department);
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;
                if (sqlException != null)
                {
                    if (sqlException.Errors.Count > 0)
                    {
                        switch (sqlException.Errors[0].Number)
                        {
                            case 2601:
                                ViewBag.ParentId = new SelectList(unitOfWork.Departments.GetAll(), "DepartmentId", "Name", department.ParentId);
                                ViewBag.Message = "Такая запись уже существует!";
                                return View(department);
                            default:
                                return View(department);
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

        }

        // GET: Departments/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = await unitOfWork.Departments.GetAsync(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            ViewBag.ParentId = new SelectList(unitOfWork.Departments.GetAll(), "DepartmentId", "Name", department.ParentId);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "DepartmentId,Name,ParentId")] Department department)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.Departments.Update(department);
                    await unitOfWork.SaveAsync();
                    return RedirectToAction("Index");
                }
                ViewBag.ParentId = new SelectList(unitOfWork.Departments.GetAll(), "DepartmentId", "Name", department.ParentId);
                return View(department);
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;
                if (sqlException != null)
                {
                    if (sqlException.Errors.Count > 0)
                    {
                        switch (sqlException.Errors[0].Number)
                        {
                            case 2601:
                                ViewBag.ParentId = new SelectList(unitOfWork.Departments.GetAll(), "DepartmentId", "Name", department.ParentId);
                                ViewBag.Message = "Такая запись уже существует!";
                                return View(department);
                            default:
                                return View(department);
                        }
                    }
                    else
                    {
                        throw;
                    }
                }
                else
                {
                    throw;
                }
            }

        }

        // GET: Departments/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = await unitOfWork.Departments.GetAsync(id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            unitOfWork.Departments.Delete(id);
            await unitOfWork.SaveAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
