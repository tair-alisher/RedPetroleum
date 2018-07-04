namespace RedPetroleum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableEmployeeId : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TaskLists", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.TaskLists", new[] { "EmployeeId" });
            AlterColumn("dbo.TaskLists", "EmployeeId", c => c.Guid(nullable: true));
            CreateIndex("dbo.TaskLists", "EmployeeId");
            AddForeignKey("dbo.TaskLists", "EmployeeId", "dbo.Employees", "EmployeeId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskLists", "EmployeeId", "dbo.Employees");
            DropIndex("dbo.TaskLists", new[] { "EmployeeId" });
            AlterColumn("dbo.TaskLists", "EmployeeId", c => c.Guid(nullable: false));
            CreateIndex("dbo.TaskLists", "EmployeeId");
            AddForeignKey("dbo.TaskLists", "EmployeeId", "dbo.Employees", "EmployeeId", cascadeDelete: true);
        }
    }
}
