namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColIsApprovedTableOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Order", "IsApproved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Order", "IsApproved");
        }
    }
}
