namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeyTableProduct : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Products", "AgeID");
            AddForeignKey("dbo.Products", "AgeID", "dbo.Ages", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "AgeID", "dbo.Ages");
            DropIndex("dbo.Products", new[] { "AgeID" });
        }
    }
}
