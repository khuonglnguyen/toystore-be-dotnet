namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatefield : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Producer",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 256),
                        Imfomation = c.String(maxLength: 256),
                        Logo = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ProductCategory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 256),
                        Description = c.String(maxLength: 500),
                        Image = c.String(maxLength: 256),
                        IsActive = c.Boolean(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                        CategoryID = c.Int(nullable: false),
                        Image1 = c.String(maxLength: 256),
                        Image2 = c.String(maxLength: 256),
                        Image3 = c.String(maxLength: 256),
                        Image4 = c.String(maxLength: 256),
                        ClipReview = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PromotionPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        Description = c.String(nullable: false, maxLength: 500),
                        HomeFlag = c.Boolean(),
                        HotFlag = c.Boolean(),
                        ViewCount = c.Int(),
                        CommentCount = c.Int(),
                        PurchasedCount = c.Int(),
                        SupplierID = c.Int(nullable: false),
                        ProducerID = c.Int(nullable: false),
                        IsNew = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        LastUpdatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Producer", t => t.ProducerID, cascadeDelete: true)
                .ForeignKey("dbo.ProductCategory", t => t.CategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Supplier", t => t.SupplierID, cascadeDelete: true)
                .Index(t => t.CategoryID)
                .Index(t => t.SupplierID)
                .Index(t => t.ProducerID);
            
            CreateTable(
                "dbo.Supplier",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 256),
                        Address = c.String(),
                        Phone = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "SupplierID", "dbo.Supplier");
            DropForeignKey("dbo.Products", "CategoryID", "dbo.ProductCategory");
            DropForeignKey("dbo.Products", "ProducerID", "dbo.Producer");
            DropIndex("dbo.Products", new[] { "ProducerID" });
            DropIndex("dbo.Products", new[] { "SupplierID" });
            DropIndex("dbo.Products", new[] { "CategoryID" });
            DropTable("dbo.Supplier");
            DropTable("dbo.Products");
            DropTable("dbo.ProductCategory");
            DropTable("dbo.Producer");
        }
    }
}
