namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class t : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MenuItems", "Controller", c => c.String(maxLength: 100));
            AddColumn("dbo.MenuItems", "Action", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MenuItems", "Action");
            DropColumn("dbo.MenuItems", "Controller");
        }
    }
}
