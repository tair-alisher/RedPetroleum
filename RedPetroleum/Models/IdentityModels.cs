﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using RedPetroleum.Models.Entities;

namespace RedPetroleum.Models
{
    // В профиль пользователя можно добавить дополнительные данные, если указать больше свойств для класса ApplicationUser. Подробности см. на странице https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public string EmployeeId { get; set; }
        public string DepartmentId { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public ApplicationDbContext(string connectionString) : base(connectionString)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<TaskList> TaskLists { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    // configures one-to-many relationship
        //    //modelBuilder.Entity<Position>()
        //    //    .HasMany<Employee>(g => g.Employees)
        //    //    .WithRequired(s => s.Position)
        //    //    .HasForeignKey<Guid>(s => s.PositionId)
        //    //    .WillCascadeOnDelete(false);
        //    //modelBuilder.Entity<Department>()
        //    //    .HasMany<Employee>(g => g.Employees)
        //    //    .WithRequired(s => s.Department)
        //    //    .HasForeignKey<Guid>(s => s.DepartmentId)
        //    //    .WillCascadeOnDelete(false);
        //    //modelBuilder.Entity<Employee>()
        //    //  .HasMany<TaskList>(g => g.TaskLists)
        //    //  .WithRequired(s => s.Employee)
        //    //  .HasForeignKey<Guid>(s => s.EmployeeId)
        //    //    .WillCascadeOnDelete(false);
        //    //modelBuilder.Entity<Department>()
        //    //    .HasOptional(x => x.Departments)
        //    //    .WithMany()
        //    //    .HasForeignKey(x => x.ParentId)
        //    //    .WillCascadeOnDelete(false);
        //}
    }
}