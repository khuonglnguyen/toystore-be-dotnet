namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColTableProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "AgeID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "AgeID");
        }
    }
}
