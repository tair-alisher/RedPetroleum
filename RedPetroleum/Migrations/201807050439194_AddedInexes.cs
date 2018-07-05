namespace RedPetroleum.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedInexes : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Departments", "Name", unique: true);
            CreateIndex("dbo.Positions", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Positions", new[] { "Name" });
            DropIndex("dbo.Departments", new[] { "Name" });
        }
    }
}
