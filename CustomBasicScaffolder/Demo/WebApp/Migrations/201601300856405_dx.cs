namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dx : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MenuItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 20),
                        Description = c.String(maxLength: 100),
                        Code = c.String(nullable: false, maxLength: 20),
                        Url = c.String(nullable: false, maxLength: 100),
                        IsEnabled = c.Boolean(nullable: false),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MenuItems", t => t.ParentId)
                .Index(t => t.Title, unique: true, name: "IX_menuTitle")
                .Index(t => t.Code, unique: true, name: "IX_menuCode")
                .Index(t => t.Url, unique: true, name: "IX_menuUrl")
                .Index(t => t.ParentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MenuItems", "ParentId", "dbo.MenuItems");
            DropIndex("dbo.MenuItems", new[] { "ParentId" });
            DropIndex("dbo.MenuItems", "IX_menuUrl");
            DropIndex("dbo.MenuItems", "IX_menuCode");
            DropIndex("dbo.MenuItems", "IX_menuTitle");
            DropTable("dbo.MenuItems");
        }
    }
}
