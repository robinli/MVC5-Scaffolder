namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtitle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Works", "Title", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Works", "Title");
        }
    }
}
