using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using X.PagedList;

using RedPetroleum.Models.Interfaces;
using RedPetroleum.Models.Entities;

namespace RedPetroleum.Models.Repositories
{
    public class EmployeeRepository : IRepository<Employee>
    {
        private ApplicationDbContext db;

        public EmployeeRepository(ApplicationDbContext context) => db = context;

        public void Create(Employee item) => db.Employees.Add(item);

        public void Delete(Guid id)
        {
            Employee emp = db.Employees.Find(id);
            if (emp != null)
                db.Employees.Remove(emp);
        }

        public Employee Get(Guid id) => db.Employees.Find(id);

        public IEnumerable<Employee> GetAll() => db.Employees.Include(e => e.Department).Include(e => e.Position);

        public IEnumerable<Employee> GetAllWithoutRelations()
        {
            return db.Employees;
        }

        public IPagedList<Employee> GetAllIndex(int pageNumber, int pageSize, string search) => db.Employees.Where(x => x.EFullName.Contains(search) || search == null).Include(e => e.Department).Include(e => e.Position).OrderBy(x=>x.EFullName).ToPagedList(pageNumber, pageSize);

        public async Task<Employee> GetAsync(Guid? id) => await db.Employees.FindAsync(id);

        public void Update(Employee item) => db.Entry(item).State = EntityState.Modified;
        public async Task<IEnumerable<Employee>> GetAllAsync() => await db.Employees.Include(e => e.Department).Include(e => e.Position).ToListAsync();

        public IEnumerable<Employee> GetEmployeesWithPositions()
        {
            return db.Employees.Include(p => p.Position).Include(e => e.Department);
        }

        public IEnumerable<Employee> GetEmployeesByDepartmentId(Guid id)
        {
            return db.Employees
                .Include(p => p.Position)
                .Where(e => e.DepartmentId == id);
        }

        public IEnumerable<Employee> GetDepartment()
        {
            return db.Employees
                .Include(e => e.Department)
                .Include(p => p.Position);
             
        }

        public string GetEmployeeNameById(Guid id)
        {
            return db.Employees
                .SingleOrDefault(e => e.EmployeeId == id)
                .EFullName;

        }

        public IEnumerable<Employee> GetAvailableEmployees(string id)
        {
            var currentUser = db.Users.Find(id);
            return db.Employees
                .Where(e =>
                    e.DepartmentId.ToString() == currentUser.DepartmentId
                );
        }
    }
}