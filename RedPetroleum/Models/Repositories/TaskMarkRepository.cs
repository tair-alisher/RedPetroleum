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
    public class TaskMarkRepository : IRepository<TaskMark>
    {
        private ApplicationDbContext db;

        public TaskMarkRepository(ApplicationDbContext context) => db = context;

        public void Create(TaskMark item) => db.TaskMarks.Add(item);

        public void Delete(Guid id)
        {
            TaskMark taskMark = db.TaskMarks.Find(id);
            if (taskMark != null)
                db.TaskMarks.Remove(taskMark);
        }

        public TaskMark Get(Guid id) => db.TaskMarks.Find(id);

        public IEnumerable<TaskMark> GetAll() => db.TaskMarks.OrderByDescending(x=>x.Mark);

        public IPagedList<TaskMark> GetAllIndex(int pageNumber, int pageSize, string search)
        {
            throw new NotImplementedException();
        }

        public async Task<TaskMark> GetAsync(Guid? id) => await db.TaskMarks.FindAsync(id);

        public void Update(TaskMark item) => db.Entry(item).State = EntityState.Modified;
    }
}