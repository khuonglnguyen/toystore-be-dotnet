namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColIsDelivereTableOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Order", "IsDelivere", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Order", "IsDelivere");
        }
    }
}
