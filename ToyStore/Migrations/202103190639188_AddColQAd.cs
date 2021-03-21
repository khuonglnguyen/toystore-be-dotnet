namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColQAd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QA", "DateQuestion", c => c.DateTime(nullable: false));
            AddColumn("dbo.QA", "DateAnswer", c => c.DateTime(nullable: false));
            DropColumn("dbo.QA", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QA", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.QA", "DateAnswer");
            DropColumn("dbo.QA", "DateQuestion");
        }
    }
}
