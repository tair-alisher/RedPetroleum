namespace RedPetroleum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletedRelation : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Employees_EmployeeId", "dbo.Employees");
            DropIndex("dbo.AspNetUsers", new[] { "Employees_EmployeeId" });
            AddColumn("dbo.AspNetUsers", "EmployeIds", c => c.String());
            DropColumn("dbo.AspNetUsers", "Employees_EmployeeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Employees_EmployeeId", c => c.Guid());
            DropColumn("dbo.AspNetUsers", "EmployeIds");
            CreateIndex("dbo.AspNetUsers", "Employees_EmployeeId");
            AddForeignKey("dbo.AspNetUsers", "Employees_EmployeeId", "dbo.Employees", "EmployeeId");
        }
    }
}
