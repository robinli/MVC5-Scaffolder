namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class md37 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "Remark", c => c.String(maxLength: 30));
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderDetails", "Remark");
        }
    }
}
