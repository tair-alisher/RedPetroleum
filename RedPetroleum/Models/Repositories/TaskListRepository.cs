using RedPetroleum.Models.Interfaces;
using RedPetroleum.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using X.PagedList;

namespace RedPetroleum.Models.Repositories
{
    public class TaskListRepository : IRepository<TaskList>
    {
        private ApplicationDbContext db;

        public TaskListRepository(ApplicationDbContext context) => db = context;

        public void Create(TaskList item) => db.TaskLists.Add(item);

        public void Delete(Guid id)
        {
            TaskList taskList = db.TaskLists.Find(id);
            if (taskList != null)
                db.TaskLists.Remove(taskList);
        }

        public TaskList Get(Guid id) => db.TaskLists.Find(id);

        public IEnumerable<TaskList> GetAll() => db.TaskLists.Include(t => t.Employee);

        public IPagedList<TaskList> GetAllIndex(int pageNumber, int pageSize, string search) => db.TaskLists.Where(x => x.TaskName.Contains(search) || search == null).Include(t => t.Employee).OrderBy(x => x.TaskName).ToPagedList(pageNumber, pageSize);

        public async Task<TaskList> GetAsync(Guid? id) => await db.TaskLists.FindAsync(id);

        public void Update(TaskList item) => db.Entry(item).State = EntityState.Modified;
    }
}