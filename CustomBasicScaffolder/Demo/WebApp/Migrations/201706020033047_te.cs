namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class te : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CodeItems", "Text", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CodeItems", "Text", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
