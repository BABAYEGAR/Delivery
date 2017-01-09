namespace Delivery.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NinthMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "TotalAmount", c => c.Long());
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.Orders", "TotalAmount", c => c.String());
        }
    }
}
