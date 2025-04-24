namespace Vendor.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class superAdmin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdminLogins", "FirstName", c => c.String(maxLength: 50));
            AddColumn("dbo.AdminLogins", "LastName", c => c.String(maxLength: 50));
            AddColumn("dbo.AdminLogins", "MobileNumber", c => c.String(maxLength: 15));
            AddColumn("dbo.AdminLogins", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.AdminLogins", "LastLogin", c => c.DateTime());
            AddColumn("dbo.AdminLogins", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.AdminLogins", "UpdatedAt", c => c.DateTime());
            AddColumn("dbo.AdminLogins", "RoleId", c => c.Int());
            AddColumn("dbo.SignupLogins", "Mobile", c => c.String(maxLength: 15));
            AddColumn("dbo.SignupLogins", "Address", c => c.String(maxLength: 200));
            AddColumn("dbo.SignupLogins", "CreatedAt", c => c.DateTime(nullable: false));
            AddColumn("dbo.SignupLogins", "UpdatedAt", c => c.DateTime());
            AddColumn("dbo.SignupLogins", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.SignupLogins", "IsActive");
            DropColumn("dbo.SignupLogins", "UpdatedAt");
            DropColumn("dbo.SignupLogins", "CreatedAt");
            DropColumn("dbo.SignupLogins", "Address");
            DropColumn("dbo.SignupLogins", "Mobile");
            DropColumn("dbo.AdminLogins", "RoleId");
            DropColumn("dbo.AdminLogins", "UpdatedAt");
            DropColumn("dbo.AdminLogins", "CreatedAt");
            DropColumn("dbo.AdminLogins", "LastLogin");
            DropColumn("dbo.AdminLogins", "IsActive");
            DropColumn("dbo.AdminLogins", "MobileNumber");
            DropColumn("dbo.AdminLogins", "LastName");
            DropColumn("dbo.AdminLogins", "FirstName");
        }
    }
}
