namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableProductViewed : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductViewed",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MemberID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Member", t => t.MemberID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.MemberID)
                .Index(t => t.ProductID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductViewed", "ProductID", "dbo.Products");
            DropForeignKey("dbo.ProductViewed", "MemberID", "dbo.Member");
            DropIndex("dbo.ProductViewed", new[] { "ProductID" });
            DropIndex("dbo.ProductViewed", new[] { "MemberID" });
            DropTable("dbo.ProductViewed");
        }
    }
}
