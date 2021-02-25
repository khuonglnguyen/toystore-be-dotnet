namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForiegnKeyTableProductCategory : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.ProductCategory", "ParentID");
            AddForeignKey("dbo.ProductCategory", "ParentID", "dbo.ProductCategoryParent", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductCategory", "ParentID", "dbo.ProductCategoryParent");
            DropIndex("dbo.ProductCategory", new[] { "ParentID" });
        }
    }
}
