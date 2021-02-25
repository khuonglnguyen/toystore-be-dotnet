namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColTableProductCategory : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Products", "ParentID", "dbo.ProductCategoryParent");
            DropIndex("dbo.Products", new[] { "ParentID" });
            AddColumn("dbo.ProductCategory", "ParentID", c => c.Int(nullable: false));
            DropColumn("dbo.Products", "ParentID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "ParentID", c => c.Int(nullable: false));
            DropColumn("dbo.ProductCategory", "ParentID");
            CreateIndex("dbo.Products", "ParentID");
            AddForeignKey("dbo.Products", "ParentID", "dbo.ProductCategoryParent", "ID", cascadeDelete: true);
        }
    }
}
