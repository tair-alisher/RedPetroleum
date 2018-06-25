namespace RedPetroleum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRequiredinEntities : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Departments", "Name", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Employees", "EFullName", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Positions", "Name", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Positions", "Name", c => c.String());
            AlterColumn("dbo.Employees", "EFullName", c => c.String());
            AlterColumn("dbo.Departments", "Name", c => c.String());
        }
    }
}
