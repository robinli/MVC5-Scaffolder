namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class c : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CodeItems", "Description", c => c.String(nullable: false, maxLength: 50));
            AddColumn("dbo.CodeItems", "IsDisabled", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CodeItems", "IsDisabled");
            DropColumn("dbo.CodeItems", "Description");
        }
    }
}
