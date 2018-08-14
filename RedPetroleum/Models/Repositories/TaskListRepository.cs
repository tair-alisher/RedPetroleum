using RedPetroleum.Models.Interfaces;
using RedPetroleum.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using X.PagedList;
using System.Web.Security;
using Microsoft.AspNet.Identity;

namespace RedPetroleum.Models.Repositories
{
    public class TaskListRepository : IRepository<TaskList>
    {
        private ApplicationDbContext db;

        public TaskListRepository(ApplicationDbContext context) => db = context;

        public void Create(TaskList item) => db.TaskLists.Add(item);

        public TaskList CreateTask(string taskName, string taskDuration, DateTime taskDate, string employeeId = null, string departmentId = null)
        {
            TaskList task = new TaskList()
            {
                TaskListId = Guid.NewGuid(),
                EmployeeId = employeeId != null ? Guid.Parse(employeeId) : (Guid?)null,
                DepartmentId = departmentId != null ? Guid.Parse(departmentId) : (Guid?)null,
                TaskName = taskName,
                TaskDuration = taskDuration,
                TaskDate = taskDate,
                CommentEmployees = ""
            };

            db.TaskLists.Add(task);
            db.SaveChanges();

            return task;
        }

        public void Delete(Guid id)
        {
            TaskList taskList = db.TaskLists.Find(id);
            if (taskList != null)
                db.TaskLists.Remove(taskList);
            db.SaveChanges();
        }

        public TaskList Get(Guid id) => db.TaskLists.Find(id);

        public IEnumerable<TaskList> GetAll() => db.TaskLists.Include(t => t.Employee);

        public IPagedList<TaskList> GetAllIndex(int pageNumber, int pageSize, string search) => db.TaskLists.Where(x => x.TaskName.Contains(search) || search == null).Include(t => t.Employee).OrderBy(x => x.TaskName).ToPagedList(pageNumber, pageSize);

        public async Task<TaskList> GetAsync(Guid? id) => await db.TaskLists.FindAsync(id);

        public void Update(TaskList item) => db.Entry(item).State = EntityState.Modified;

        public ApplicationUser GetUser(string id)
        {
            return db.Users.Find(id);
        }

        public IPagedList<TaskList> GetEmployeesById(int pageNumber, int pageSize, string search, string id)
        {
            var currentUser = db.Users.Find(id);
            var employees = currentUser.EmployeeId == null ? null : currentUser.EmployeeId.Split(',').Select(i => Guid.Parse(i));

            if (employees != null)
                return db.TaskLists
                .Include(t => t.Employee)
                .Where(d => employees.Contains(d.Employee.EmployeeId))
                .Where(x => x.TaskName.Contains(search) || search == null)
                .OrderBy(x => x.TaskName).ToPagedList(pageNumber, pageSize);
            else
                return new PagedList<TaskList>(null, 1, 1);
        }

        public IPagedList<TaskList> GetEmployeeTasksByDepartmentId(int pageNumber, int pageSize, string search, string departmentId)
        {
            IEnumerable<TaskList> taskList = db.TaskLists
                .Include(e => e.Employee)
                .Where(t =>
                    t.Employee.DepartmentId.ToString() == departmentId &&
                    (t.TaskName.Contains(search) || search == null)
                )
                .OrderBy(t => t.TaskName);

            if (!(taskList.Count() <= 0))
                return taskList.ToPagedList(pageNumber, pageSize);
            else
                return new PagedList<TaskList>(null, 1, 1);
        }

        public IPagedList<TaskList> GetEmployeesAdmin(int pageNumber, int pageSize, string search)
        {
            return
                db.TaskLists
                .Include(t => t.Employee)
                .Where(t => t.EmployeeId != null)
                .Where(x => x.TaskName.Contains(search) || search == null)
                .OrderBy(x => x.TaskName).ToPagedList(pageNumber, pageSize);
        }

        public IEnumerable<TaskList> GetFilteredTaskList(string departmentId, string employeeId, DateTime? month)
        {
            IEnumerable<TaskList> taskList = db.TaskLists
                .Include(e => e.Employee)
                .Where(t => t.EmployeeId != null);

            if (!String.IsNullOrEmpty(departmentId))
                taskList = taskList
                    .Where(t => t.Employee.DepartmentId.ToString() == departmentId);
            if (!String.IsNullOrEmpty(employeeId))
                taskList = taskList
                    .Where(t => t.EmployeeId.ToString() == employeeId);
            if (month != null)
                taskList = FilterTaskListByMonth(taskList, month);

            return taskList
                .OrderBy(t => t.TaskName);
        }

        public TaskList GetTaskWithEmployeeById(Guid id)
        {
            return db.TaskLists
                 .Include(e => e.Employee)
                 .Where(t => t.TaskListId == id).FirstOrDefault();
        }

        public TaskList GetTaskWithDepartmentById(Guid id)
        {
            return db.TaskLists
                .Include(d => d.Department)
                .Where(t => t.TaskListId == id).FirstOrDefault();
        }

        public void RateTask(
            string taskId, double skill, double effectiveness,
            double discipline, double timeliness, double average
            )
        {
            TaskList task = db.TaskLists.Find(Guid.Parse(taskId));
            task.SkillMark = skill;
            task.EffectivenessMark = effectiveness;
            task.DisciplineMark = discipline;
            task.TimelinessMark = timeliness;
            task.AverageMark = average;

            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void RateDepartmentTask(string taskId, double average)
        {
            TaskList task = db.TaskLists.Find(Guid.Parse(taskId));
            task.AverageMark = average;

            db.Entry(task).State = EntityState.Modified;
            db.SaveChanges();
        }

        public IPagedList<TaskList> GetAllDepartmentsTasks(int pageNumber, int pageSize, string search)
        {
            return db.TaskLists
                .Include(d => d.Department)
                .Where(t => t.DepartmentId != null)
                .Where(t => t.TaskName.Contains(search) || search == null)
                .OrderBy(t => t.TaskName).ToPagedList(pageNumber, pageSize);
        }

        public IPagedList<TaskList> GetDepartmentTasksByDepartmentId(int pageNumber, int pageSize, string search, string departmentId)
        {
            return db.TaskLists
                .Include(t => t.Department)
                .Where(t => t.DepartmentId.ToString() == departmentId)
                 .Where(t => t.TaskName.Contains(search) || search == null)
                .OrderBy(t => t.TaskName)
                .ToPagedList(pageNumber, pageSize);
        }

        public IEnumerable<TaskList> GetFilteredDepartmentTaskList(string departmentId, DateTime? month)
        {
            IEnumerable<TaskList> taskList = db.TaskLists
                .Include(d => d.Department)
                .Where(t => t.DepartmentId != null);

            if (!String.IsNullOrEmpty(departmentId))
                taskList = taskList
                    .Where(t => t.DepartmentId.ToString() == departmentId);
            if (month != null)
                taskList = FilterTaskListByMonth(taskList, month);

            return taskList
                .OrderBy(t => t.TaskName);
        }

        public void CommentTask(string taskId, string comment)
        {
            TaskList taskToUpdate = db.TaskLists.Find(Guid.Parse(taskId));
            taskToUpdate.CommentEmployees = comment;
            db.Entry(taskToUpdate).State = EntityState.Modified;
            db.SaveChanges();
        }

        private IEnumerable<TaskList> FilterTaskListByMonth(IEnumerable<TaskList> taskList, DateTime? month)
        {
            return taskList
                .Where(t =>
                    ((DateTime)t.TaskDate).Year == ((DateTime)month).Year &&
                    ((DateTime)t.TaskDate).Month == ((DateTime)month).Month
                );
        }
        public IEnumerable<TaskList> GetTaskListsWithRelations()
        {
            return db.TaskLists
                .Include(d => d.Department);
        }

        public IEnumerable<TaskList> GetTaskListsByDepartmentId(Guid id, DateTime? taskDate)
        {
            return db.TaskLists
                .Include(d => d.Department)
                .Where(e => e.DepartmentId == id).Where(e =>
                    ((DateTime)e.TaskDate) == taskDate
                );
        }
    }
}