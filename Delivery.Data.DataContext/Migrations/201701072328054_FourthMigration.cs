namespace Delivery.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FourthMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Flavours", "UnitAmount", c => c.Long(nullable: false));
            AddColumn("dbo.Orders", "TotalAmount", c => c.String(nullable: false));
            AddColumn("dbo.Shishas", "UnitAmount", c => c.Long(nullable: false));
            AlterColumn("dbo.Flavours", "Name", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Flavours", "Name", c => c.String());
            DropColumn("dbo.Shishas", "UnitAmount");
            DropColumn("dbo.Orders", "TotalAmount");
            DropColumn("dbo.Flavours", "UnitAmount");
        }
    }
}
