namespace Vendor.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class add_category1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Categories", "Products_id", c => c.Int());
            AddColumn("dbo.Products", "CategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Categories", "Products_id");
            AddForeignKey("dbo.Categories", "Products_id", "dbo.Products", "id");
        }

        public override void Down()
        {
            DropForeignKey("dbo.Categories", "Products_id", "dbo.Products");
            DropIndex("dbo.Categories", new[] { "Products_id" });
            DropColumn("dbo.Products", "CategoryId");
            DropColumn("dbo.Categories", "Products_id");
        }
    }
}
