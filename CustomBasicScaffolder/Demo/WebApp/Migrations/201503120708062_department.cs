namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class department : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Departments", "Name", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Departments", "Manager", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Departments", "Manager", c => c.String());
            AlterColumn("dbo.Departments", "Name", c => c.String());
        }
    }
}
