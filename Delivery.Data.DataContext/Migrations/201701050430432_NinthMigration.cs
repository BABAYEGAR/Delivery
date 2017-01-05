namespace Delivery.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NinthMigration : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "Location", c => c.String());
            AlterColumn("dbo.Orders", "Email", c => c.String());
            AlterColumn("dbo.Orders", "Name", c => c.String());
            AlterColumn("dbo.Orders", "Mobile", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "Mobile", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "Email", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "Location", c => c.String(nullable: false));
        }
    }
}
