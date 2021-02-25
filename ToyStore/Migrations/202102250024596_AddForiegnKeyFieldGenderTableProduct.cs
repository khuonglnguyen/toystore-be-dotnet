namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForiegnKeyFieldGenderTableProduct : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Products", "GenderID");
            AddForeignKey("dbo.Products", "GenderID", "dbo.Gender", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Products", "GenderID", "dbo.Gender");
            DropIndex("dbo.Products", new[] { "GenderID" });
        }
    }
}
