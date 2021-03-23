namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColEmloyeeIDTableQA : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QA", "EmloyeeID", c => c.Int(nullable: false));
            CreateIndex("dbo.QA", "EmloyeeID");
            AddForeignKey("dbo.QA", "EmloyeeID", "dbo.Emloyee", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QA", "EmloyeeID", "dbo.Emloyee");
            DropIndex("dbo.QA", new[] { "EmloyeeID" });
            DropColumn("dbo.QA", "EmloyeeID");
        }
    }
}
