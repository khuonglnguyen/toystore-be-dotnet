namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableEmloyee : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Decentralization", "MemberCategoryID", "dbo.MemberCategory");
            DropIndex("dbo.Decentralization", new[] { "MemberCategoryID" });
            CreateTable(
                "dbo.Emloyee",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        FullName = c.String(),
                        Address = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Decentralization", "EmloyeeID", c => c.Int(nullable: false));
            CreateIndex("dbo.Decentralization", "EmloyeeID");
            AddForeignKey("dbo.Decentralization", "EmloyeeID", "dbo.Emloyee", "ID", cascadeDelete: true);
            DropColumn("dbo.Decentralization", "MemberCategoryID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Decentralization", "MemberCategoryID", c => c.Int(nullable: false));
            DropForeignKey("dbo.Decentralization", "EmloyeeID", "dbo.Emloyee");
            DropIndex("dbo.Decentralization", new[] { "EmloyeeID" });
            DropColumn("dbo.Decentralization", "EmloyeeID");
            DropTable("dbo.Emloyee");
            CreateIndex("dbo.Decentralization", "MemberCategoryID");
            AddForeignKey("dbo.Decentralization", "MemberCategoryID", "dbo.MemberCategory", "ID", cascadeDelete: true);
        }
    }
}
