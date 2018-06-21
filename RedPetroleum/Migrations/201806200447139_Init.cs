namespace RedPetroleum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TaskLists", "Mark", c => c.Double());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TaskLists", "Mark", c => c.Double(nullable: false));
        }
    }
}
