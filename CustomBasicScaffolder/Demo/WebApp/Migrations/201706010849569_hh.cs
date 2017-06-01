namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class hh : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Content = c.String(maxLength: 255),
                        Link = c.String(maxLength: 255),
                        Read = c.Int(nullable: false),
                        From = c.String(),
                        To = c.String(),
                        Group = c.Int(nullable: false),
                        Created = c.DateTime(nullable: false),
                        Creator = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Notifications");
        }
    }
}
