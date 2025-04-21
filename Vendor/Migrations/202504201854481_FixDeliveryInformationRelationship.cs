namespace Vendor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDeliveryInformationRelationship : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.InvoiceModels", "FullName", c => c.String());
            AddColumn("dbo.InvoiceModels", "MobileNumber", c => c.String());
            AddColumn("dbo.InvoiceModels", "DeliveryAddress", c => c.String());
            AddColumn("dbo.InvoiceModels", "Landmark", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.InvoiceModels", "Landmark");
            DropColumn("dbo.InvoiceModels", "DeliveryAddress");
            DropColumn("dbo.InvoiceModels", "MobileNumber");
            DropColumn("dbo.InvoiceModels", "FullName");
        }
    }
}
