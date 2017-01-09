namespace Delivery.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class seventhMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StockLogs",
                c => new
                    {
                        StockLogId = c.Long(nullable: false, identity: true),
                        ItemCategory = c.String(),
                        ItemName = c.String(),
                        Action = c.String(),
                        Amount = c.Long(nullable: false),
                        FlavourId = c.Long(),
                        ShishaId = c.Long(),
                        ActionDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.StockLogId)
                .ForeignKey("dbo.Flavours", t => t.FlavourId)
                .ForeignKey("dbo.Shishas", t => t.ShishaId)
                .Index(t => t.FlavourId)
                .Index(t => t.ShishaId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockLogs", "ShishaId", "dbo.Shishas");
            DropForeignKey("dbo.StockLogs", "FlavourId", "dbo.Flavours");
            DropIndex("dbo.StockLogs", new[] { "ShishaId" });
            DropIndex("dbo.StockLogs", new[] { "FlavourId" });
            DropTable("dbo.StockLogs");
        }
    }
}
