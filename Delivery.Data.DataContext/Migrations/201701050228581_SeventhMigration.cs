namespace Delivery.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeventhMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Email", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Email");
        }
    }
}
