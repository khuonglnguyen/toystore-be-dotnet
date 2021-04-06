namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColIsRatingTableOrderDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetail", "IsRating", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetail", "IsRating");
        }
    }
}
