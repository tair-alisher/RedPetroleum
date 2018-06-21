using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using X.PagedList;
using RedPetroleum.Models.Entities;
using RedPetroleum.Models.UnitOfWork;

namespace RedPetroleum.Controllers.CRUD
{
    public class TaskListsController : Controller
    {
        UnitOfWork unitOfWork;

        public TaskListsController() => this.unitOfWork = new UnitOfWork();

        public TaskListsController(UnitOfWork unit) => this.unitOfWork = unit;

        // GET: TaskLists
        public ActionResult Index(int? page, string searching)
        {
            
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var taskLists = unitOfWork.TaskLists.GetAllIndex(pageNumber, pageSize, searching);
            return View(taskLists.ToPagedList(pageNumber, pageSize));
        }

        // GET: TaskLists/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskList taskList = await unitOfWork.TaskLists.GetAsync(id);
            if (taskList == null)
            {
                return HttpNotFound();
            }
            return View(taskList);
        }

        // GET: TaskLists/Create
        public ActionResult Create()
        {
            ViewBag.EmployeeId = new SelectList(unitOfWork.Employees.GetAll(), "EmployeeId", "EFullName");
            return View();
        }

        // POST: TaskLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TaskListId,EmployeeId,TaskName,TaskDuration,CommentEmployer,CommentEmployees,Mark")] TaskList taskList)
        {
            if (ModelState.IsValid)
            {
                taskList.TaskListId = Guid.NewGuid();
                unitOfWork.TaskLists.Create(taskList);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }

            ViewBag.EmployeeId = new SelectList(unitOfWork.Employees.GetAll(), "EmployeeId", "EFullName", taskList.EmployeeId);
            return View(taskList);
        }

        // GET: TaskLists/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskList taskList = await unitOfWork.TaskLists.GetAsync(id);
            if (taskList == null)
            {
                return HttpNotFound();
            }
            ViewBag.EmployeeId = new SelectList(unitOfWork.Employees.GetAll(), "EmployeeId", "EFullName", taskList.EmployeeId);
            return View(taskList);
        }

        // POST: TaskLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TaskListId,EmployeeId,TaskName,TaskDuration,CommentEmployer,CommentEmployees,Mark")] TaskList taskList)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.TaskLists.Update(taskList);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(unitOfWork.Employees.GetAll(), "EmployeeId", "EFullName", taskList.EmployeeId);
            return View(taskList);
        }

        // GET: TaskLists/Delete/5
        public async Task<ActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskList taskList = await unitOfWork.TaskLists.GetAsync(id);
            if (taskList == null)
            {
                return HttpNotFound();
            }
            return View(taskList);
        }

        // POST: TaskLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(Guid id)
        {
            unitOfWork.TaskLists.Delete(id);
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
