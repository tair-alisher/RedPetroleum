using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace RedPetroleum.Models.Tables
{
    public class PetroleumContext : DbContext
    {
        public PetroleumContext() : base("PetroleumContext")
        { }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Employee> Employees  { get; set; }
        public DbSet<ETask> ETasks { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<TaskList> TaskLists { get; set; }
        public DbSet<ACL> ACL { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // configures one-to-many relationship
            modelBuilder.Entity<ETask>()
                 .HasMany<TaskList>(g => g.TaskLists)
                 .WithRequired(s => s.ETask)
                 .HasForeignKey<Guid>(s => s.ETaskId);
            modelBuilder.Entity<Position>()
                .HasMany<Employee>(g => g.Employees)
                .WithRequired(s => s.Position)
                .HasForeignKey<Guid>(s => s.PositionId);
            modelBuilder.Entity<Department>()
                .HasMany<Employee>(g => g.Employees)
                .WithRequired(s => s.Department)
                .HasForeignKey<Guid>(s => s.DepartmentId);
            modelBuilder.Entity<Employee>()
              .HasMany<TaskList>(g => g.TaskLists)
              .WithRequired(s => s.Employee)
              .HasForeignKey<Guid>(s => s.EmployeeId);
            modelBuilder.Entity<Department>()
                .HasOptional(x=>x.Departments)
                .WithMany()
                .HasForeignKey(x => x.ParentId);
        }
    }
}