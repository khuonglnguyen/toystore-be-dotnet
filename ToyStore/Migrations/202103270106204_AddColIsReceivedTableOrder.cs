namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColIsReceivedTableOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Order", "IsReceived", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Order", "IsReceived");
        }
    }
}
