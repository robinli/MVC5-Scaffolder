namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class qc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Products", "IsRequiredQc", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Products", "IsRequiredQc");
        }
    }
}
