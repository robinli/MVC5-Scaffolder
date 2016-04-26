namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class d : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoleMenus", "Create", c => c.Boolean(nullable: false));
            AddColumn("dbo.RoleMenus", "Edit", c => c.Boolean(nullable: false));
            AddColumn("dbo.RoleMenus", "Delete", c => c.Boolean(nullable: false));
            AddColumn("dbo.RoleMenus", "Import", c => c.Boolean(nullable: false));
            AddColumn("dbo.RoleMenus", "Export", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoleMenus", "Export");
            DropColumn("dbo.RoleMenus", "Import");
            DropColumn("dbo.RoleMenus", "Delete");
            DropColumn("dbo.RoleMenus", "Edit");
            DropColumn("dbo.RoleMenus", "Create");
        }
    }
}
