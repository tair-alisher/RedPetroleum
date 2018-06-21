﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

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

        public IEnumerable<Employee> GetAll() => db.Employees;

        public async Task<Employee> GetAsync(Guid? id) => await db.Employees.FindAsync(id);

        public void Update(Employee item) => db.Entry(item).State = EntityState.Modified;
        public async Task<IEnumerable<Employee>> GetAllAsync() => await db.Employees.Include(e => e.Department).Include(e => e.Position).ToListAsync();

        public IEnumerable<Employee> GetEmployeesWithPositions()
        {
            return db.Employees.Include(p => p.Position);
        }

        public IEnumerable<Employee> GetEmployeesByDepartmentId(Guid id)
        {
            return db.Employees
                .Include(p => p.Position)
                .Where(e => e.DepartmentId == id);
        }
    }
}