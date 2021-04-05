namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColDateTableProductViewed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductViewed", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductViewed", "Date");
        }
    }
}
