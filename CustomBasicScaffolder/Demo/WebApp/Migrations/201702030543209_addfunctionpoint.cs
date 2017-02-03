namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfunctionpoint : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RoleMenus", "FunctionPoint1", c => c.Boolean(nullable: false));
            AddColumn("dbo.RoleMenus", "FunctionPoint2", c => c.Boolean(nullable: false));
            AddColumn("dbo.RoleMenus", "FunctionPoint3", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RoleMenus", "FunctionPoint3");
            DropColumn("dbo.RoleMenus", "FunctionPoint2");
            DropColumn("dbo.RoleMenus", "FunctionPoint1");
        }
    }
}
