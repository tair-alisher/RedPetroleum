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

        public TaskList CreateTask(string employeeId, string taskName, string taskDuration, DateTime taskDate)
        {
            TaskList task = new TaskList()
            {
                TaskListId = Guid.NewGuid(),
                EmployeeId = Guid.Parse(employeeId),
                DepartmentId = null,
                TaskName = taskName,
                TaskDuration = taskDuration,
                TaskDate = taskDate,
                CommentEmployees = "",
                Mark = null
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
            var employees = currentUser.EmployeeId.Split(',').Select(i => Guid.Parse(i));
            return db.TaskLists
                .Include(t => t.Employee)
                .Where(d => employees.Contains(d.Employee.EmployeeId))
                .Where(x => x.TaskName.Contains(search) || search == null)
                .OrderBy(x => x.TaskName).ToPagedList(pageNumber, pageSize);
        }
    }
}