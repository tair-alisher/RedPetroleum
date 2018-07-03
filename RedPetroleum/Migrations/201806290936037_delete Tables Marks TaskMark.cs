namespace RedPetroleum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deleteTablesMarksTaskMark : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskLists", "SkillMark", c => c.Double());
            AddColumn("dbo.TaskLists", "EffectivenessMark", c => c.Double());
            AddColumn("dbo.TaskLists", "DisciplineMark", c => c.Double());
            AddColumn("dbo.TaskLists", "TimelinessMark", c => c.Double());
            AddColumn("dbo.TaskLists", "AverageMark", c => c.Double());
            
        }
        
        public override void Down()
        {
           
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
            
        }
    }
}
