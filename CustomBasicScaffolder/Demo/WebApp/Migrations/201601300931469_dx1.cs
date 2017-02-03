namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dx1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.RoleMenus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 20),
                        MenuId = c.Int(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MenuItems", t => t.MenuId, cascadeDelete: true)
                .Index(t => new { t.RoleName, t.MenuId }, unique: true, name: "IX_rolemenu");
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RoleMenus", "MenuId", "dbo.MenuItems");
            DropIndex("dbo.RoleMenus", "IX_rolemenu");
            DropTable("dbo.RoleMenus");
        }
    }
}
