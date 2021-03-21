namespace ToyStore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateColTableOrderDetail : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OrderDetail", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrderDetail", "Price", c => c.Int(nullable: false));
        }
    }
}
