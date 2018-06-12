namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deltitle : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Works", "Title");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Works", "Title", c => c.String());
        }
    }
}
