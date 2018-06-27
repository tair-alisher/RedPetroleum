namespace RedPetroleum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Update : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaskMarks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MarkId = c.Guid(nullable: false),
                        TaskListId = c.Guid(nullable: false),
                        MarkValue = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Marks", t => t.Id)
                .ForeignKey("dbo.TaskLists", t => t.TaskListId, cascadeDelete: false)
                .Index(t => t.Id)
                .Index(t => t.TaskListId);
            
            CreateTable(
                "dbo.Marks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskMarks", "TaskListId", "dbo.TaskLists");
            DropForeignKey("dbo.TaskMarks", "Id", "dbo.Marks");
            DropIndex("dbo.TaskMarks", new[] { "TaskListId" });
            DropIndex("dbo.TaskMarks", new[] { "Id" });
            DropTable("dbo.Marks");
            DropTable("dbo.TaskMarks");
        }
    }
}
