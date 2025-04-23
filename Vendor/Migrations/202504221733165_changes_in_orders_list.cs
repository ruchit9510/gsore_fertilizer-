namespace Vendor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes_in_orders_list : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "IsAccepted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "IsAccepted");
        }
    }
}
