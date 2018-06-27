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
    public class MarkRepository : IRepository<Mark>
    {
        private ApplicationDbContext db;

        public MarkRepository(ApplicationDbContext context) => db = context;

        public void Create(Mark item) => db.Marks.Add(item);

        public void Delete(Guid id)
        {
            Mark mark = db.Marks.Find(id);
            if (mark != null)
                db.Marks.Remove(mark);
        }

        public Mark Get(Guid id) => db.Marks.Find(id);

        public IEnumerable<Mark> GetAll() => db.Marks.OrderByDescending(x => x.Name);

        public IPagedList<Mark> GetAllIndex(int pageNumber, int pageSize, string search) => db.Marks.Where(x => x.Name.Contains(search) || search == null).OrderBy(x => x.Name).ToPagedList(pageNumber, pageSize);

        public async Task<Mark> GetAsync(Guid? id) => await db.Marks.FindAsync(id);

        public void Update(Mark item) => db.Entry(item).State = EntityState.Modified;
    }
}