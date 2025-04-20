namespace Vendor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAddressToInvoiceOrWhatever : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ShippingInfoes", "InvoiceId", "dbo.InvoiceModels");
            DropIndex("dbo.ShippingInfoes", new[] { "InvoiceId" });
            DropTable("dbo.ShippingInfoes");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ShippingInfoes",
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
                        InvoiceId = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.ShippingInfoes", "InvoiceId");
            AddForeignKey("dbo.ShippingInfoes", "InvoiceId", "dbo.InvoiceModels", "ID");
        }
    }
}
