namespace Delivery.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EightMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "DateOrderModified", c => c.DateTime(nullable: false));
            AddColumn("dbo.Orders", "AppUserId", c => c.Long());
            AlterColumn("dbo.Orders", "Email", c => c.String());
            AlterColumn("dbo.Orders", "Name", c => c.String());
            AlterColumn("dbo.Orders", "Mobile", c => c.String());
            AlterColumn("dbo.Orders", "TotalAmount", c => c.String());
            CreateIndex("dbo.Orders", "AppUserId");
            AddForeignKey("dbo.Orders", "AppUserId", "dbo.AppUsers", "AppUserId");
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "TotalAmount", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "Mobile", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Orders", "Email", c => c.String(nullable: false));
        }
    }
}
