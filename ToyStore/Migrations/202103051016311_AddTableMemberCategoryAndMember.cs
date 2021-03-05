namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableMemberCategoryAndMember : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MemberCategory",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Member",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        MemberCategoryID = c.Int(nullable: false),
                        Username = c.String(),
                        Password = c.String(),
                        FullName = c.String(),
                        Address = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.MemberCategory", t => t.MemberCategoryID, cascadeDelete: true)
                .Index(t => t.MemberCategoryID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Member", "MemberCategoryID", "dbo.MemberCategory");
            DropIndex("dbo.Member", new[] { "MemberCategoryID" });
            DropTable("dbo.Member");
            DropTable("dbo.MemberCategory");
        }
    }
}
