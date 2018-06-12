namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class messages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Group = c.Int(nullable: false),
                        ExtensionKey1 = c.String(maxLength: 50),
                        Type = c.Int(nullable: false),
                        Code = c.String(maxLength: 50),
                        Content = c.String(),
                        ExtensionKey2 = c.String(maxLength: 50),
                        Tags = c.String(maxLength: 255),
                        StackTrace = c.String(),
                        Method = c.String(maxLength: 255),
                        User = c.String(maxLength: 20),
                        Created = c.DateTime(nullable: false),
                        IsNew = c.Int(nullable: false),
                        IsNotification = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Messages");
        }
    }
}
