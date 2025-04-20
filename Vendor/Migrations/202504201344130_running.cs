namespace Vendor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class running : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.DeliveryInfoes", "FkInvoiceID", "dbo.InvoiceModels");
            DropIndex("dbo.DeliveryInfoes", new[] { "FkInvoiceID" });
            DropTable("dbo.DeliveryInfoes");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.DeliveryInfoes", "FkInvoiceID");
            AddForeignKey("dbo.DeliveryInfoes", "FkInvoiceID", "dbo.InvoiceModels", "ID");
        }
    }
}
