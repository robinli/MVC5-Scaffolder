namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addressmaxlength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Companies", "Address", c => c.String(maxLength: 60));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Companies", "Address", c => c.String());
        }
    }
}
