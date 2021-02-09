namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcoll : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Producer", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Producer", "LastUpdatedDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Producer", "LastUpdatedDate");
            DropColumn("dbo.Producer", "IsActive");
        }
    }
}
