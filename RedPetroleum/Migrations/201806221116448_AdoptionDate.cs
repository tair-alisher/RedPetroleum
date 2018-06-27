namespace RedPetroleum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdoptionDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "AdoptionDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Employees", "AdoptionDate");
        }
    }
}
