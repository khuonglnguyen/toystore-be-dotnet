namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableAccessTimesCount : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccessTimesCount",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        AccessTimes = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AccessTimesCount");
        }
    }
}
