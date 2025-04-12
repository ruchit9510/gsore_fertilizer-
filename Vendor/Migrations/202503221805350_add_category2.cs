namespace Vendor.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class add_category2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "ProductName", c => c.String(nullable: false));
        }

        public override void Down()
        {
            AlterColumn("dbo.Products", "ProductName", c => c.String());
        }
    }
}
