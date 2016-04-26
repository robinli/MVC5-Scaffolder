namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class iconcls : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.MenuItems", "IX_menuTitle");
            DropIndex("dbo.MenuItems", "IX_menuCode");
            DropIndex("dbo.MenuItems", "IX_menuUrl");
            AddColumn("dbo.MenuItems", "IconCls", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MenuItems", "IconCls");
            CreateIndex("dbo.MenuItems", "Url", unique: true, name: "IX_menuUrl");
            CreateIndex("dbo.MenuItems", "Code", unique: true, name: "IX_menuCode");
            CreateIndex("dbo.MenuItems", "Title", unique: true, name: "IX_menuTitle");
        }
    }
}
