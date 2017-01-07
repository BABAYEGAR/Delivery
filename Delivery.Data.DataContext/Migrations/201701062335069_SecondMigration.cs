namespace Delivery.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecondMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Shishas", "Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Shishas", "Image");
        }
    }
}
