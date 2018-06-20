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

namespace RedPetroleum.Controllers.CRUD
{
    public class EmployeesController : Controller
    {
        private UnitOfWork unitOfWork;

        public EmployeesController() => this.unitOfWork = new UnitOfWork();

        public EmployeesController(UnitOfWork unit) => this.unitOfWork = unit;

        // GET: Employees
        public async Task<ActionResult> Index()
        {
            var employees = await unitOfWork.Employees.GetAllAsync();
            return View(employees);
        }

        // GET: Employees/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await unitOfWork.Employees.GetAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(unitOfWork.Departments.GetAll(), "DepartmentId", "Name");
            ViewBag.PositionId = new SelectList(unitOfWork.Positions.GetAll(), "PositionId", "Name");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EmployeeId,EFullName,DepartmentId,PositionId,DateBorn,Dismissed")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.EmployeeId = Guid.NewGuid();
                unitOfWork.Employees.Create(employee);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(unitOfWork.Departments.GetAll(), "DepartmentId", "Name", employee.DepartmentId);
            ViewBag.PositionId = new SelectList(unitOfWork.Positions.GetAll(), "PositionId", "Name", employee.PositionId);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await unitOfWork.Employees.GetAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(unitOfWork.Departments.GetAll(), "DepartmentId", "Name", employee.DepartmentId);
            ViewBag.PositionId = new SelectList(unitOfWork.Positions.GetAll(), "PositionId", "Name", employee.PositionId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EmployeeId,EFullName,DepartmentId,PositionId,DateBorn,Dismissed")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Employees.Update(employee);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(unitOfWork.Departments.GetAll(), "DepartmentId", "Name", employee.DepartmentId);
            ViewBag.PositionId = new SelectList(unitOfWork.Positions.GetAll(), "PositionId", "Name", employee.PositionId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await unitOfWork.Employees.GetAsync(id);
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            unitOfWork.Employees.Delete(id);
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
