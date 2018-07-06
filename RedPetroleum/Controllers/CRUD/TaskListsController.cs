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
using Microsoft.AspNet.Identity;

namespace RedPetroleum.Controllers.CRUD
{
    public class TaskListsController : Controller
    {
        UnitOfWork unitOfWork;

        public TaskListsController() => this.unitOfWork = new UnitOfWork();

        public TaskListsController(UnitOfWork unit) => this.unitOfWork = unit;

        // GET: TaskLists
        [Authorize(Roles ="admin, manager, employee")]
        public ActionResult Index(int? page, string searching)
        {
            var currentUser = unitOfWork.TaskLists.GetUser(User.Identity.GetUserId());
            
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            IPagedList<TaskList> taskLists;
            if (User.IsInRole("admin"))
            {
                taskLists = unitOfWork
                    .TaskLists
                    .GetEmployeesAdmin(pageNumber, pageSize, searching);

                
                ViewBag.EmployeeId = CreateEmployeeSelectList(
                    unitOfWork
                        .Employees
                        .GetAllWithoutRelations());

                ViewBag.DepartmentId = CreateDepartmentSelectList();
            }
            else
            {
                taskLists = unitOfWork
                .TaskLists
                .GetEmployeeTasksByDepartmentId(pageNumber, pageSize, searching, currentUser.DepartmentId);

                ViewBag.EmployeeId = CreateEmployeeSelectList(
                    unitOfWork
                        .Employees
                        .GetAvailableEmployees(User.Identity.GetUserId()));

                ViewBag.Department = unitOfWork
                    .Departments
                    .GetDepartmentByUserId(User.Identity.GetUserId());
            }


            ViewBag.Today = DateTime.Now.ToString("yyyy-MM");

            return View(taskLists.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public void CommentTask(string taskId, string comment)
        {
            unitOfWork.TaskLists.CommentTask(taskId, comment);
        }

        // GET: TaskLists/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskList taskList = unitOfWork.TaskLists.GetTaskWithEmployeeById((Guid)id);
            if (taskList == null)
            {
                return HttpNotFound();
            }
            return View(taskList);
        }

        // GET: TaskLists/Create
        public ActionResult Create()
        {
            if (User.IsInRole("admin"))
            {
                ViewBag.EmployeeId = new SelectList(unitOfWork.Employees.GetAll(), "EmployeeId", "EFullName");
            } else
            {
                ViewBag.EmployeeId = new SelectList(unitOfWork.Employees.GetAvailableEmployees(User.Identity.GetUserId()), "EmployeeId", "EFullName");
            }
            
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
                .CreateTask(taskName, taskDuration, taskDate, employeeId, null);
            string taskEmployeeName = unitOfWork
                .Employees
                .GetEmployeeNameById(Guid.Parse(employeeId));

            ViewBag.Task = task;
            ViewBag.EmployeeName = taskEmployeeName;

            return PartialView(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetFilteredTaskList(int? page, DateTime? TaskDate, string DepartmentId, string EmployeeId)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            IEnumerable<TaskList> taskList = unitOfWork
                .TaskLists
                .GetFilteredTaskList(DepartmentId, EmployeeId, TaskDate);

            if (User.IsInRole("admin"))
            {
                ViewBag.DepartmentId = CreateDepartmentSelectList(DepartmentId);

                ViewBag.EmployeeId = CreateEmployeeSelectList(
                    unitOfWork
                        .Employees
                        .GetAllWithoutRelations(),
                    EmployeeId);
            }
            else
            {
                ViewBag.Department = unitOfWork
                    .Departments
                    .GetDepartmentByUserId(User.Identity.GetUserId());

                ViewBag.EmployeeId = CreateEmployeeSelectList(
                    unitOfWork
                        .Employees
                        .GetAvailableEmployees(User.Identity.GetUserId()),
                    EmployeeId);
            }

            if (TaskDate != null)
                ViewBag.Month = ((DateTime)TaskDate).ToString("yyyy-MM");

            return View(taskList.ToPagedList(pageNumber, pageSize));
        }

        // POST: TaskLists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TaskListId,EmployeeId,TaskName,TaskDuration,CommentEmployer,CommentEmployees")] TaskList taskList)
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
            ViewBag.TaskDate = ((DateTime)taskList.TaskDate).ToString("yyyy-MM");
            return View(taskList);
        }

        // POST: TaskLists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TaskListId,EmployeeId,TaskName,TaskDuration,TaskDate,CommentEmployees")] TaskList taskList)
        {
            if (taskList.TaskDate == null)
                taskList.TaskDate = DateTime.Today;

            if (ModelState.IsValid)
            {
                unitOfWork.TaskLists.Update(taskList);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            ViewBag.EmployeeId = new SelectList(unitOfWork.Employees.GetAvailableEmployees(User.Identity.GetUserId()), "EmployeeId", "EFullName", taskList.EmployeeId);
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
            return Redirect(Request.UrlReferrer.ToString());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }

        public double RateTask(string taskId, string skillMark, string effectivenessMark, string disciplineMark, string timelinessMark)
        {
            double skill = ConvertMarkToDouble(skillMark);
            double effectiveness = ConvertMarkToDouble(effectivenessMark);
            double discipline = ConvertMarkToDouble(disciplineMark);
            double timeliness = ConvertMarkToDouble(timelinessMark);

            double average = (skill + effectiveness + discipline + timeliness) / 4;

            unitOfWork
                .TaskLists
                .RateTask(
                    taskId,
                    skill,
                    effectiveness,
                    discipline,
                    timeliness,
                    average
                );

            return average;
        }

        private double ConvertMarkToDouble(string mark)
        {
            return double.Parse(mark.Replace(".", ","));
        }

        /************ Department Tasks ************/

        [Authorize(Roles = "admin, manager, employee")]
        public ActionResult DepartmentTasks(int? page, string searching)
        {
            var currentUser = unitOfWork.TaskLists.GetUser(User.Identity.GetUserId());

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            IPagedList<TaskList> taskLists;
            if (User.IsInRole("admin"))
            {
                taskLists = unitOfWork
                    .TaskLists
                    .GetAllDepartmentsTasks(pageNumber, pageSize, searching);

                ViewBag.DepartmentId = CreateDepartmentSelectList();
            }
            else
            {
                taskLists = unitOfWork
                .TaskLists
                .GetDepartmentTasksByDepartmentId(pageNumber, pageSize, searching, currentUser.DepartmentId);

                ViewBag.Department = unitOfWork
                    .Departments
                    .GetDepartmentByUserId(User.Identity.GetUserId());
            }

            ViewBag.Today = DateTime.Now.ToString("yyyy-MM");

            return View(taskLists.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetFilteredDepartmentTaskList(int? page, DateTime? TaskDate, string DepartmentId)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            IEnumerable<TaskList> taskList = unitOfWork
                .TaskLists
                .GetFilteredDepartmentTaskList(DepartmentId, TaskDate);

            if (User.IsInRole("admin"))
                ViewBag.DepartmentId = CreateDepartmentSelectList(DepartmentId);
            else
                ViewBag.Department = unitOfWork
                    .Departments
                    .GetDepartmentByUserId(User.Identity.GetUserId());

            if (TaskDate != null)
                ViewBag.Month = ((DateTime)TaskDate).ToString("yyyy-MM");

            return View(taskList.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult CreateDepartmentTask()
        {
            if (User.IsInRole("admin"))
                ViewBag.DepartmentId = CreateDepartmentSelectList();
            else
                ViewBag.Department = unitOfWork
                    .Departments
                    .GetDepartmentByUserId(User.Identity.GetUserId());

            ViewBag.Today = DateTime.Now.ToString("yyyy-MM");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDepartmentTaskPost(string departmentId, string taskName, string taskDuration, DateTime taskDate)
        {
            // TODO: сделать проверку,
            // обладает ли пользователь правами на выполнение операции
            TaskList task = unitOfWork
                .TaskLists
                .CreateTask(taskName, taskDuration, taskDate, null, departmentId);
            string taskDepartmentName = unitOfWork
                .Departments
                .GetDepartmentNameById(Guid.Parse(departmentId));

            ViewBag.Task = task;
            ViewBag.DepartmentName = taskDepartmentName;

            return PartialView(task);
        }

        public ActionResult DepartmentTaskDetails(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            TaskList taskList = unitOfWork.TaskLists.GetTaskWithDepartmentById((Guid)id);
            if (taskList == null)
                return HttpNotFound();

            return View(taskList);
        }

        public async Task<ActionResult> EditDepartmentTask(Guid? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            TaskList taskList = await unitOfWork.TaskLists.GetAsync(id);
            if (taskList == null)
                return HttpNotFound();

            if (User.IsInRole("admin"))
                ViewBag.DepartmentId = CreateDepartmentSelectList(taskList.DepartmentId.ToString());
            else
                ViewBag.Department = unitOfWork
                    .Departments
                    .GetDepartmentByUserId(User.Identity.GetUserId());

            ViewBag.TaskDate = ((DateTime)taskList.TaskDate).ToString("yyyy-MM");

            return View(taskList);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditDepartmentTask([Bind(Include = "TaskListId,DepartmentId,TaskName,TaskDuration,TaskDate,CommentEmployees")] TaskList taskList)
        {
            if (taskList.TaskDate == null)
                taskList.TaskDate = DateTime.Today;

            if (ModelState.IsValid)
            {
                unitOfWork.TaskLists.Update(taskList);
                await unitOfWork.SaveAsync();
                return RedirectToAction("DepartmentTasks");
            }

            if (User.IsInRole("admin"))
                ViewBag.DepartmentId = CreateDepartmentSelectList(taskList.DepartmentId.ToString());
            else
                ViewBag.Department = unitOfWork
                    .Departments
                    .GetDepartmentByUserId(User.Identity.GetUserId());

            return View(taskList);
        }

        private SelectList CreateDepartmentSelectList(string selected = null)
        {
            if (selected == null)
                return new SelectList(
                    unitOfWork
                        .Departments
                        .GetAll(),
                    "DepartmentId",
                    "Name");
            else
                return new SelectList(
                    unitOfWork
                        .Departments
                        .GetAll(),
                    "DepartmentId",
                    "Name",
                    selected);
        }

        private SelectList CreateEmployeeSelectList(IEnumerable<Employee> employees, string selected = null)
        {
            if (selected == null)
                return new SelectList(
                    employees,
                    "EmployeeId",
                    "EFullName"
                    );
            else
                return new SelectList(
                    employees,
                    "EmployeeId",
                    "EFullName",
                    selected
                    );
        }
    }
}
