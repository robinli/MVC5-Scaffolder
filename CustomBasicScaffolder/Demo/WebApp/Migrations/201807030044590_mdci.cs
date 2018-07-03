namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mdci : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.CodeItems", "CodeItem_IX");
            AddColumn("dbo.CodeItems", "Multiple", c => c.Boolean(nullable: false));
            AlterColumn("dbo.CodeItems", "Code", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.CodeItems", "Text", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.CodeItems", "Description", c => c.String(nullable: false, maxLength: 80));
            CreateIndex("dbo.CodeItems", new[] { "Code", "CodeType" }, unique: true, name: "CodeItem_IX");
        }
        
        public override void Down()
        {
            DropIndex("dbo.CodeItems", "CodeItem_IX");
            AlterColumn("dbo.CodeItems", "Description", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.CodeItems", "Text", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.CodeItems", "Code", c => c.String(nullable: false, maxLength: 20));
            DropColumn("dbo.CodeItems", "Multiple");
            CreateIndex("dbo.CodeItems", new[] { "Code", "CodeType" }, unique: true, name: "CodeItem_IX");
        }
    }
}
