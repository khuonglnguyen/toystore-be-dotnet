namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropColTitleTableQA : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.QA", "Title");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QA", "Title", c => c.String(maxLength: 100));
        }
    }
}
