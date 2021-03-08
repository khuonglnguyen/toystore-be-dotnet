namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableImportCouponDetail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImportCouponDetail",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ImportCouponID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ImportCoupon", t => t.ImportCouponID, cascadeDelete: false)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: false)
                .Index(t => t.ImportCouponID)
                .Index(t => t.ProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ImportCouponDetail", "ProductID", "dbo.Products");
            DropForeignKey("dbo.ImportCouponDetail", "ImportCouponID", "dbo.ImportCoupon");
            DropIndex("dbo.ImportCouponDetail", new[] { "ProductID" });
            DropIndex("dbo.ImportCouponDetail", new[] { "ImportCouponID" });
            DropTable("dbo.ImportCouponDetail");
        }
    }
}
