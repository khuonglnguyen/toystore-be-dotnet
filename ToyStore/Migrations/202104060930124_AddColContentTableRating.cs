namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColContentTableRating : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rating", "Content", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rating", "Content");
        }
    }
}
