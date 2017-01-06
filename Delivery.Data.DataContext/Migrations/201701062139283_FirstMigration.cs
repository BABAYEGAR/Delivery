namespace Delivery.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
          
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "ShishaId", "dbo.Shishas");
            DropForeignKey("dbo.Orders", "FlavourId", "dbo.Flavours");
            DropIndex("dbo.Orders", new[] { "FlavourId" });
            DropIndex("dbo.Orders", new[] { "ShishaId" });
            DropTable("dbo.Shishas");
            DropTable("dbo.Orders");
            DropTable("dbo.Flavours");
            DropTable("dbo.AppUsers");
        }
    }
}
