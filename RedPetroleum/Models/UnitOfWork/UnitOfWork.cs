using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using RedPetroleum.Models.Interfaces;
using RedPetroleum.Models.Repositories;

namespace RedPetroleum.Models.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext db;

        private DepartmentRepository departmentRepository;
        private EmployeeRepository employeeRepository;
        private PositionRepository positionRepository;
        private TaskListRepository listRepository;

        public UnitOfWork() => db = new ApplicationDbContext();

        public UnitOfWork(string connectionString) => db = new ApplicationDbContext(connectionString);

        public DepartmentRepository Departments
        {
            get
            {
                if (departmentRepository == null)
                    departmentRepository = new DepartmentRepository(db);
                return departmentRepository;
            }
        }

        public EmployeeRepository Employees
        {
            get
            {
                if (employeeRepository == null)
                    employeeRepository = new EmployeeRepository(db);
                return employeeRepository;
            }
        }

        public PositionRepository Positions
        {
            get
            {
                if (positionRepository == null)
                    positionRepository = new PositionRepository(db);
                return positionRepository;
            }
        }

        public TaskListRepository TaskLists
        {
            get
            {
                if (listRepository == null)
                    listRepository = new TaskListRepository(db);
                return listRepository;
            }
        }
        

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await db.SaveChangesAsync();
        }
    }
}