namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class te1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Works", "Created");
            DropColumn("dbo.Works", "CreatedBy");
            DropColumn("dbo.Works", "LastModified");
            DropColumn("dbo.Works", "LastModifiedBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Works", "LastModifiedBy", c => c.String());
            AddColumn("dbo.Works", "LastModified", c => c.DateTime());
            AddColumn("dbo.Works", "CreatedBy", c => c.String());
            AddColumn("dbo.Works", "Created", c => c.DateTime());
        }
    }
}
