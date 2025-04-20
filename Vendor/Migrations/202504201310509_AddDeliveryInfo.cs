namespace Vendor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDeliveryInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeliveryInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        StreetAddress = c.String(nullable: false),
                        City = c.String(nullable: false),
                        State = c.String(nullable: false),
                        PostalCode = c.String(nullable: false),
                        AdditionalNotes = c.String(),
                        DeliveryDate = c.DateTime(),
                        FkInvoiceID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.InvoiceModels", t => t.FkInvoiceID)
                .Index(t => t.FkInvoiceID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DeliveryInfoes", "FkInvoiceID", "dbo.InvoiceModels");
            DropIndex("dbo.DeliveryInfoes", new[] { "FkInvoiceID" });
            DropTable("dbo.DeliveryInfoes");
        }
    }
}
