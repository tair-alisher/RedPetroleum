﻿using RedPetroleum.Models.Interfaces;
using RedPetroleum.Models.Entities;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using X.PagedList;

namespace RedPetroleum.Models.Repositories
{
    public class DepartmentRepository : IRepository<Department>
    {
        private ApplicationDbContext db;

        public DepartmentRepository(ApplicationDbContext context) => db = context;

        public void Create(Department item) => db.Departments.Add(item);

        public void Delete(Guid id)
        {
            Department dept = db.Departments.Find(id);
            if (dept != null)
                db.Departments.Remove(dept);
        }

        public Department Get(Guid id) => db.Departments.Find(id);

        public IEnumerable<Department> GetAll() => db.Departments;

        public IPagedList<Department> GetAllIndex(int pageNumber, int pageSize, string search) => db.Departments.Where(x => x.Name.Contains(search) || search == null).Include(d => d.Departments).OrderBy(x=>x.Name).ToPagedList(pageNumber, pageSize);

        public async Task<Department> GetAsync(Guid? id) => await db.Departments.FindAsync(id);

        public void Update(Department item) => db.Entry(item).State = EntityState.Modified;

        public async Task<IEnumerable<Department>> GetAllAsync() => await db.Departments.Include(d => d.Departments).ToListAsync();

        public string GetDepartmentNameById(Guid id)
        {
            return db.Departments
                .SingleOrDefault(d => d.DepartmentId == id)
                .Name;
        }

        public IEnumerable<Department> GetAvailableDepartments(string id)
        {
            var currentUser = db.Users.Find(id);
            var user = currentUser.DepartmentId;
            return db.Departments.Where(d => user.Contains(d.DepartmentId.ToString()));
        }
    }
}