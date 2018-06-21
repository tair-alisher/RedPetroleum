namespace RedPetroleum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewGuid : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "EmployeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "EmployeId", c => c.Guid(nullable: false));
        }
    }
}
