namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change102 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "HomeFlag", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Products", "HotFlag", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "HotFlag", c => c.Boolean());
            AlterColumn("dbo.Products", "HomeFlag", c => c.Boolean());
        }
    }
}
