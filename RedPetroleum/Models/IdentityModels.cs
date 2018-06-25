using System;
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
        public string EmployeeNames { get; set; }
        
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
        public DbSet<Mark> Marks { get; set; }
        public DbSet<TaskMark> TaskMarks { get; set; }
        public DbSet<ACL> ACLs { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    // configures one-to-many relationship
        //    modelBuilder.Entity<Position>()
        //        .HasMany<Employee>(g => g.Employees)
        //        .WithRequired(s => s.Position)
        //        .HasForeignKey<Guid>(s => s.PositionId);
        //    modelBuilder.Entity<Department>()
        //        .HasMany<Employee>(g => g.Employees)
        //        .WithRequired(s => s.Department)
        //        .HasForeignKey<Guid>(s => s.DepartmentId);
        //    modelBuilder.Entity<Employee>()
        //      .HasMany<TaskList>(g => g.TaskLists)
        //      .WithRequired(s => s.Employee)
        //      .HasForeignKey<Guid>(s => s.EmployeeId);
        //    modelBuilder.Entity<Department>()
        //        .HasOptional(x => x.Departments)
        //        .WithMany()
        //        .HasForeignKey(x => x.ParentId);
        //}
    }
}