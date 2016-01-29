namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dd : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Companies", "Name", c => c.String());
            AlterColumn("dbo.Companies", "Address", c => c.String());
            AlterColumn("dbo.Companies", "City", c => c.String());
            AlterColumn("dbo.Companies", "Province", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Companies", "Province", c => c.String(maxLength: 20));
            AlterColumn("dbo.Companies", "City", c => c.String(maxLength: 20));
            AlterColumn("dbo.Companies", "Address", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Companies", "Name", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
