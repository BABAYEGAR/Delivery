namespace Delivery.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EightthMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Name", c => c.String(nullable: false));
            AddColumn("dbo.Orders", "DateOfOrder", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "Mobile", c => c.String(nullable: false));
            AddColumn("dbo.Orders", "OrderStatus", c => c.String());
            DropColumn("dbo.Orders", "CreatedBy");
            DropColumn("dbo.Orders", "DateCreated");
            DropColumn("dbo.Orders", "DateLastModified");
            DropColumn("dbo.Orders", "LastModifiedBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "LastModifiedBy", c => c.Long(nullable: false));
            AddColumn("dbo.Orders", "DateLastModified", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "DateCreated", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "CreatedBy", c => c.Long(nullable: false));
            DropColumn("dbo.Orders", "OrderStatus");
            DropColumn("dbo.Orders", "Mobile");
            DropColumn("dbo.Orders", "DateOfOrder");
            DropColumn("dbo.Orders", "Name");
        }
    }
}
