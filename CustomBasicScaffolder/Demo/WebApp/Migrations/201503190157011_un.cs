namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class un : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.BaseCodes", "CodeType", unique: true);
            CreateIndex("dbo.CodeItems", "Code", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.CodeItems", new[] { "Code" });
            DropIndex("dbo.BaseCodes", new[] { "CodeType" });
        }
    }
}
