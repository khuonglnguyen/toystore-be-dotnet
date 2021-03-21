namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColDateTableComment : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comment", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comment", "Date");
        }
    }
}
