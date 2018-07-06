namespace RedPetroleum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditedModels : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Departments", "Name", c => c.String(maxLength: 200));
            AlterColumn("dbo.Employees", "EFullName", c => c.String(maxLength: 100));
            AlterColumn("dbo.Positions", "Name", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Positions", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Employees", "EFullName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Departments", "Name", c => c.String(nullable: false, maxLength: 200));
        }
    }
}
