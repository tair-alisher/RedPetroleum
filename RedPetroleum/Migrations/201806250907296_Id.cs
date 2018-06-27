namespace RedPetroleum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Id : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "EmployeeId", c => c.String());
            DropColumn("dbo.AspNetUsers", "EmployeeNames");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "EmployeeNames", c => c.String());
            DropColumn("dbo.AspNetUsers", "EmployeeId");
        }
    }
}
