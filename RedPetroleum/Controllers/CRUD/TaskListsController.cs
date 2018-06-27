﻿using System;
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
using Microsoft.AspNet.Identity;

namespace RedPetroleum.Controllers.CRUD
{
    public class TaskListsController : Controller
    {
        UnitOfWork unitOfWork;

        public TaskListsController() => this.unitOfWork = new UnitOfWork();

        public TaskListsController(UnitOfWork unit) => this.unitOfWork = unit;

        // GET: TaskLists
        [Authorize(Roles ="admin, manager")]
        public ActionResult Index(int? page, string searching)
        {
            var currentUser = unitOfWork.TaskLists.GetUser(User.Identity.GetUserId());
            
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            IPagedList<TaskList> taskLists;
            if (User.IsInRole("admin"))
            {
                taskLists = unitOfWork.TaskLists.GetEmployeesAdmin(pageNumber, pageSize, searching);
            }
            taskLists = unitOfWork.TaskLists.GetEmployeesById(pageNumber, pageSize, searching, currentUser.Id);
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
            ViewBag.EmployeeId = new SelectList(unitOfWork.Employees.GetAvailableEmployees(User.Identity.GetUserId()), "EmployeeId", "EFullName");
            ViewBag.Today = DateTime.Now.ToString("yyyy-MM");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTask(string employeeId, string taskName, string taskDuration, DateTime taskDate)
        {
            // TODO: сделать проверку,
            // обладает ли пользователь правами на выполнение операции
            TaskList task = unitOfWork
                .TaskLists
                .CreateTask(employeeId, taskName, taskDuration, taskDate);
            string taskEmployeeName = unitOfWork
                .Employees
                .GetEmployeeNameById(Guid.Parse(employeeId));

            ViewBag.Task = task;
            ViewBag.EmployeeName = taskEmployeeName;

            return PartialView(task);
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

            //ViewBag.EmployeeId = new SelectList(unitOfWork.Employees.GetAll(), "EmployeeId", "EFullName", taskList.EmployeeId);
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
            ViewBag.EmployeeId = new SelectList(unitOfWork.Employees.GetAvailableEmployees(User.Identity.GetUserId()), "EmployeeId", "EFullName", taskList.EmployeeId);
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
            //ViewBag.EmployeeId = new SelectList(unitOfWork.Employees.GetAvailableEmployees(User.Identity.GetUserId()), "EmployeeId", "EFullName", taskList.EmployeeId);
            return View(taskList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void DeleteTask(string taskId)
        {
            // TODO: сделать проверку,
            // обладает ли пользователь правами на выполнение операции
            Guid taskGuidId = Guid.Parse(taskId);
            unitOfWork
                .TaskLists
                .Delete(taskGuidId);
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
