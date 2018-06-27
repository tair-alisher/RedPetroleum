namespace RedPetroleum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OneToOneRelationship : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Employee_EmployeeId", "dbo.Employees");
            DropIndex("dbo.AspNetUsers", new[] { "Employee_EmployeeId" });
            AddColumn("dbo.AspNetUsers", "DepartmentId", c => c.String());
            DropColumn("dbo.AspNetUsers", "Employee_EmployeeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Employee_EmployeeId", c => c.Guid());
            DropColumn("dbo.AspNetUsers", "DepartmentId");
            CreateIndex("dbo.AspNetUsers", "Employee_EmployeeId");
            AddForeignKey("dbo.AspNetUsers", "Employee_EmployeeId", "dbo.Employees", "EmployeeId");
        }
    }
}
