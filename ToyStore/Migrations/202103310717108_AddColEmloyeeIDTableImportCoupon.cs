namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColEmloyeeIDTableImportCoupon : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImportCoupon", "EmloyeeID", c => c.Int(nullable: false));
            CreateIndex("dbo.ImportCoupon", "EmloyeeID");
            AddForeignKey("dbo.ImportCoupon", "EmloyeeID", "dbo.Emloyee", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ImportCoupon", "EmloyeeID", "dbo.Emloyee");
            DropIndex("dbo.ImportCoupon", new[] { "EmloyeeID" });
            DropColumn("dbo.ImportCoupon", "EmloyeeID");
        }
    }
}
