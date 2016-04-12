namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_product : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "Unit", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "Unit", c => c.String(maxLength: 3));
        }
    }
}
