namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class á : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Member", "Username", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Member", "Username", c => c.String());
        }
    }
}
