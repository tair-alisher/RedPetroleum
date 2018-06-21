using RedPetroleum.Models.Interfaces;
using RedPetroleum.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using PagedList;

namespace RedPetroleum.Models.Repositories
{
    public class PositionRepository : IRepository<Position>
    {
        private ApplicationDbContext db;

        public PositionRepository(ApplicationDbContext context) => db = context;

        public void Create(Position item) => db.Positions.Add(item);

        public void Delete(Guid id)
        {
            Position pos = db.Positions.Find(id);
            if (pos != null)
                db.Positions.Remove(pos);
        }

        public Position Get(Guid id) => db.Positions.Find(id);

        public IEnumerable<Position> GetAll() => db.Positions.OrderByDescending(x => x.Name);

        public IPagedList<Position> GetAllIndex(int pageNumber, int pageSize, string search) => db.Positions.Where(x => x.Name.Contains(search) || search == null).OrderBy(x => x.Name).ToPagedList(pageNumber, pageSize);

        public async Task<Position> GetAsync(Guid? id) => await db.Positions.FindAsync(id);

        public void Update(Position item) => db.Entry(item).State = EntityState.Modified;
    }
}