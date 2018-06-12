namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class f : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CodeItems", "BaseCodeId", "dbo.BaseCodes");
            DropIndex("dbo.BaseCodes", new[] { "CodeType" });
            DropIndex("dbo.CodeItems", "CodeItem_IX");
            AddColumn("dbo.CodeItems", "CodeType", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.BaseCodes", "CodeType", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.CodeItems", "Text", c => c.String(nullable: false, maxLength: 20));
            CreateIndex("dbo.BaseCodes", "CodeType", unique: true);
            CreateIndex("dbo.CodeItems", new[] { "Code", "CodeType" }, unique: true, name: "CodeItem_IX");
            DropColumn("dbo.CodeItems", "BaseCodeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CodeItems", "BaseCodeId", c => c.Int(nullable: false));
            DropIndex("dbo.CodeItems", "CodeItem_IX");
            DropIndex("dbo.BaseCodes", new[] { "CodeType" });
            AlterColumn("dbo.CodeItems", "Text", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.BaseCodes", "CodeType", c => c.String(nullable: false, maxLength: 30));
            DropColumn("dbo.CodeItems", "CodeType");
            CreateIndex("dbo.CodeItems", new[] { "Code", "BaseCodeId" }, unique: true, name: "CodeItem_IX");
            CreateIndex("dbo.BaseCodes", "CodeType", unique: true);
            AddForeignKey("dbo.CodeItems", "BaseCodeId", "dbo.BaseCodes", "Id", cascadeDelete: true);
        }
    }
}
