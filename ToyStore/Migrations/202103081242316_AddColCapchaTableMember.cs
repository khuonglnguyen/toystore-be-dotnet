namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddColCapchaTableMember : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Member", "Capcha", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Member", "Capcha");
        }
    }
}
