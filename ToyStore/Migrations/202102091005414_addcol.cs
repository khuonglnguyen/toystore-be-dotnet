namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcol : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Supplier", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Supplier", "LastUpdatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Supplier", "LastUpdatedDate");
            DropColumn("dbo.Supplier", "IsActive");
        }
    }
}
