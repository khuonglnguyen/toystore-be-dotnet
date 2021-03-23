namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeyTableEmloyee : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Emloyee", "EmloyeeTypeID", c => c.Int(nullable: false));
            CreateIndex("dbo.Emloyee", "EmloyeeTypeID");
            AddForeignKey("dbo.Emloyee", "EmloyeeTypeID", "dbo.EmloyeeType", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Emloyee", "EmloyeeTypeID", "dbo.EmloyeeType");
            DropIndex("dbo.Emloyee", new[] { "EmloyeeTypeID" });
            DropColumn("dbo.Emloyee", "EmloyeeTypeID");
        }
    }
}
