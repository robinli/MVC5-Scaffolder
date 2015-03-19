namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class basecdoe : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BaseCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CodeType = c.String(nullable: false, maxLength: 20),
                        Description = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CodeItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 20),
                        Text = c.String(nullable: false, maxLength: 20),
                        BaseCodeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BaseCodes", t => t.BaseCodeId, cascadeDelete: true)
                .Index(t => t.BaseCodeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CodeItems", "BaseCodeId", "dbo.BaseCodes");
            DropIndex("dbo.CodeItems", new[] { "BaseCodeId" });
            DropTable("dbo.CodeItems");
            DropTable("dbo.BaseCodes");
        }
    }
}
