namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class desc : DbMigration
    {
        public override void Up()
        { 
            AlterColumn("dbo.Orders", "Customer", c => c.String(nullable: false, maxLength: 30));
            AlterColumn("dbo.Orders", "ShippingAddress", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "ShippingAddress", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Orders", "Customer", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
