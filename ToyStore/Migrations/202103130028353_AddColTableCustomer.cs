namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColTableCustomer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customer", "MemberCategoryID", c => c.Int(nullable: false));
            CreateIndex("dbo.Customer", "MemberCategoryID");
            AddForeignKey("dbo.Customer", "MemberCategoryID", "dbo.MemberCategory", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Customer", "MemberCategoryID", "dbo.MemberCategory");
            DropIndex("dbo.Customer", new[] { "MemberCategoryID" });
            DropColumn("dbo.Customer", "MemberCategoryID");
        }
    }
}
