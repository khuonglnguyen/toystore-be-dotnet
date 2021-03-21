namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColDateTableQA : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QA", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.QA", "Date");
        }
    }
}
