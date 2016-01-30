namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _base : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.BaseCodes", new[] { "CodeType" });
            AlterColumn("dbo.BaseCodes", "CodeType", c => c.String(nullable: false, maxLength: 30));
            CreateIndex("dbo.BaseCodes", "CodeType", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.BaseCodes", new[] { "CodeType" });
            AlterColumn("dbo.BaseCodes", "CodeType", c => c.String(nullable: false, maxLength: 20));
            CreateIndex("dbo.BaseCodes", "CodeType", unique: true);
        }
    }
}
