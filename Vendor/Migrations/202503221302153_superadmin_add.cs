namespace Vendor.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class superadmin_add : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SuperAdminLogins",
                c => new
                {
                    SuperAdminId = c.Int(nullable: false, identity: true),
                    Email = c.String(nullable: false, maxLength: 50),
                    Password = c.String(nullable: false, maxLength: 100),
                })
                .PrimaryKey(t => t.SuperAdminId);

        }

        public override void Down()
        {
            DropTable("dbo.SuperAdminLogins");
        }
    }
}
