namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableGenderAndCol : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Gender",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 256),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Products", "GenderID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "GenderID");
            DropTable("dbo.Gender");
        }
    }
}
