namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md36 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Departments", "Name", c => c.String(maxLength: 10));
            AlterColumn("dbo.Departments", "Manager", c => c.String(maxLength: 10));
            AlterColumn("dbo.Works", "Name", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Works", "Status", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Works", "Status", c => c.String(nullable: false));
            AlterColumn("dbo.Works", "Name", c => c.String(nullable: false));
            AlterColumn("dbo.Departments", "Manager", c => c.String(maxLength: 20));
            AlterColumn("dbo.Departments", "Name", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
