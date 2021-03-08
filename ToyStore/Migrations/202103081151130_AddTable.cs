namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comment",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        MemberID = c.Int(nullable: false),
                        Content = c.String(maxLength: 1000),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Member", t => t.MemberID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.MemberID);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FullName = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Decentralization",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MemberCategoryID = c.Int(nullable: false),
                        RoleID = c.Int(nullable: false),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.MemberCategory", t => t.MemberCategoryID, cascadeDelete: true)
                .ForeignKey("dbo.Role", t => t.RoleID, cascadeDelete: true)
                .Index(t => t.MemberCategoryID)
                .Index(t => t.RoleID);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OrderDetail",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        OrderID = c.Int(nullable: false),
                        ProductID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Price = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Order", t => t.OrderID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.OrderID)
                .Index(t => t.ProductID);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        DateOrder = c.DateTime(nullable: false),
                        DateShip = c.DateTime(nullable: false),
                        Offer = c.Int(nullable: false),
                        IsPaid = c.Boolean(nullable: false),
                        IsCancel = c.Boolean(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customer", t => t.CustomerID, cascadeDelete: true)
                .Index(t => t.CustomerID);
            
            CreateTable(
                "dbo.QA",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        MemberID = c.Int(nullable: false),
                        Title = c.String(maxLength: 100),
                        Content = c.String(maxLength: 1000),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Member", t => t.MemberID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.MemberID);
            
            CreateTable(
                "dbo.Rating",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProductID = c.Int(nullable: false),
                        MemberID = c.Int(nullable: false),
                        Star = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Member", t => t.MemberID, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductID, cascadeDelete: true)
                .Index(t => t.ProductID)
                .Index(t => t.MemberID);
            
            AddColumn("dbo.Member", "EmailConfirmed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Rating", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Rating", "MemberID", "dbo.Member");
            DropForeignKey("dbo.QA", "ProductID", "dbo.Products");
            DropForeignKey("dbo.QA", "MemberID", "dbo.Member");
            DropForeignKey("dbo.OrderDetail", "ProductID", "dbo.Products");
            DropForeignKey("dbo.OrderDetail", "OrderID", "dbo.Order");
            DropForeignKey("dbo.Order", "CustomerID", "dbo.Customer");
            DropForeignKey("dbo.Decentralization", "RoleID", "dbo.Role");
            DropForeignKey("dbo.Decentralization", "MemberCategoryID", "dbo.MemberCategory");
            DropForeignKey("dbo.Comment", "ProductID", "dbo.Products");
            DropForeignKey("dbo.Comment", "MemberID", "dbo.Member");
            DropIndex("dbo.Rating", new[] { "MemberID" });
            DropIndex("dbo.Rating", new[] { "ProductID" });
            DropIndex("dbo.QA", new[] { "MemberID" });
            DropIndex("dbo.QA", new[] { "ProductID" });
            DropIndex("dbo.Order", new[] { "CustomerID" });
            DropIndex("dbo.OrderDetail", new[] { "ProductID" });
            DropIndex("dbo.OrderDetail", new[] { "OrderID" });
            DropIndex("dbo.Decentralization", new[] { "RoleID" });
            DropIndex("dbo.Decentralization", new[] { "MemberCategoryID" });
            DropIndex("dbo.Comment", new[] { "MemberID" });
            DropIndex("dbo.Comment", new[] { "ProductID" });
            DropColumn("dbo.Member", "EmailConfirmed");
            DropTable("dbo.Rating");
            DropTable("dbo.QA");
            DropTable("dbo.Order");
            DropTable("dbo.OrderDetail");
            DropTable("dbo.Role");
            DropTable("dbo.Decentralization");
            DropTable("dbo.Customer");
            DropTable("dbo.Comment");
        }
    }
}
