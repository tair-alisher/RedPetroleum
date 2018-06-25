namespace RedPetroleum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Names : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "EmployeeNames", c => c.String());
            DropColumn("dbo.AspNetUsers", "EmployeIds");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "EmployeIds", c => c.String());
            DropColumn("dbo.AspNetUsers", "EmployeeNames");
        }
    }
}
