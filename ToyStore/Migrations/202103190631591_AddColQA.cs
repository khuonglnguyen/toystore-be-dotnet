namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColQA : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QA", "Question", c => c.String(maxLength: 1000));
            AddColumn("dbo.QA", "Answer", c => c.String(maxLength: 1000));
            DropColumn("dbo.QA", "Content");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QA", "Content", c => c.String(maxLength: 1000));
            DropColumn("dbo.QA", "Answer");
            DropColumn("dbo.QA", "Question");
        }
    }
}
