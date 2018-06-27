namespace RedPetroleum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTaskTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskLists", "DepartmentId", c => c.Guid());
            AddColumn("dbo.TaskLists", "TaskDate", c => c.DateTime());
            CreateIndex("dbo.TaskLists", "DepartmentId");
            AddForeignKey("dbo.TaskLists", "DepartmentId", "dbo.Departments", "DepartmentId", false);
            DropColumn("dbo.TaskLists", "CommentEmployer");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaskLists", "CommentEmployer", c => c.String());
            DropForeignKey("dbo.TaskLists", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.TaskLists", new[] { "DepartmentId" });
            DropColumn("dbo.TaskLists", "TaskDate");
            DropColumn("dbo.TaskLists", "DepartmentId");
        }
    }
}
