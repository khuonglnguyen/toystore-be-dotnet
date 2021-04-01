namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeColTableImportCoupon : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ImportCoupon", "ProducerID", "dbo.Producer");
            DropIndex("dbo.ImportCoupon", new[] { "ProducerID" });
            AddColumn("dbo.ImportCoupon", "SupplierID", c => c.Int(nullable: false));
            CreateIndex("dbo.ImportCoupon", "SupplierID");
            AddForeignKey("dbo.ImportCoupon", "SupplierID", "dbo.Supplier", "ID", cascadeDelete: true);
            DropColumn("dbo.ImportCoupon", "ProducerID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ImportCoupon", "ProducerID", c => c.Int(nullable: false));
            DropForeignKey("dbo.ImportCoupon", "SupplierID", "dbo.Supplier");
            DropIndex("dbo.ImportCoupon", new[] { "SupplierID" });
            DropColumn("dbo.ImportCoupon", "SupplierID");
            CreateIndex("dbo.ImportCoupon", "ProducerID");
            AddForeignKey("dbo.ImportCoupon", "ProducerID", "dbo.Producer", "ID", cascadeDelete: true);
        }
    }
}
