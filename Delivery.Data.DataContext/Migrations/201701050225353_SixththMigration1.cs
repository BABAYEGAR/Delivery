namespace Delivery.Data.DataContext.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SixththMigration1 : DbMigration
    {
        public override void Up()
        {
     
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Long(nullable: false, identity: true),
                        Location = c.String(nullable: false),
                        ShishaId = c.Long(nullable: false),
                        FlavourId = c.Long(nullable: false),
                        Quantity = c.Int(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateLastModified = c.DateTime(nullable: false),
                        LastModifiedBy = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Flavours", t => t.FlavourId, cascadeDelete: true)
                .ForeignKey("dbo.Shishas", t => t.ShishaId, cascadeDelete: true)
                .Index(t => t.ShishaId)
                .Index(t => t.FlavourId);
            
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
        }
    }
}
