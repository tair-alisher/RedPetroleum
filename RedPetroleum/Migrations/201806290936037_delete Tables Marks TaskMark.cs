namespace RedPetroleum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteTablesMarksTaskMark : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TaskMarks", "Id", "dbo.Marks");
            DropForeignKey("dbo.TaskMarks", "TaskListId", "dbo.TaskLists");
            DropIndex("dbo.TaskMarks", new[] { "Id" });
            DropIndex("dbo.TaskMarks", new[] { "TaskListId" });
            AddColumn("dbo.TaskLists", "SkillMark", c => c.Double());
            AddColumn("dbo.TaskLists", "EffectivenessMark", c => c.Double());
            AddColumn("dbo.TaskLists", "DisciplineMark", c => c.Double());
            AddColumn("dbo.TaskLists", "TimelinessMark", c => c.Double());
            AddColumn("dbo.TaskLists", "AverageMark", c => c.Double());
            
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Marks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TaskMarks",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        MarkId = c.Guid(nullable: false),
                        TaskListId = c.Guid(nullable: false),
                        MarkValue = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ACL",
                c => new
                    {
                        AclId = c.Guid(nullable: false),
                        UserId = c.String(),
                        RoleId = c.Int(nullable: false),
                        GrandUrl = c.String(),
                        DenyUrl = c.String(),
                    })
                .PrimaryKey(t => t.AclId);
            
            DropColumn("dbo.TaskLists", "AverageMark");
            DropColumn("dbo.TaskLists", "TimelinessMark");
            DropColumn("dbo.TaskLists", "DisciplineMark");
            DropColumn("dbo.TaskLists", "EffectivenessMark");
            DropColumn("dbo.TaskLists", "SkillMark");
            CreateIndex("dbo.TaskMarks", "TaskListId");
            CreateIndex("dbo.TaskMarks", "Id");
            AddForeignKey("dbo.TaskMarks", "TaskListId", "dbo.TaskLists", "TaskListId", cascadeDelete: true);
            AddForeignKey("dbo.TaskMarks", "Id", "dbo.Marks", "Id");
        }
    }
}
