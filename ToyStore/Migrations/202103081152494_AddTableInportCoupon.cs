namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableInportCoupon : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImportCoupon",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ProducerID = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        IsDelete = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Producer", t => t.ProducerID, cascadeDelete: true)
                .Index(t => t.ProducerID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ImportCoupon", "ProducerID", "dbo.Producer");
            DropIndex("dbo.ImportCoupon", new[] { "ProducerID" });
            DropTable("dbo.ImportCoupon");
        }
    }
}
