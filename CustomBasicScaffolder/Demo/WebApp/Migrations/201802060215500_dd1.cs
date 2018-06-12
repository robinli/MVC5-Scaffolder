namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dd1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Employees", "Name", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.Employees", "Sex", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "Sex", c => c.String(maxLength: 10));
            AlterColumn("dbo.Employees", "Name", c => c.String(maxLength: 20));
        }
    }
}
