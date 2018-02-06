namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Employees", "Title", c => c.String(maxLength: 30));
            AddColumn("dbo.Employees", "IsDeleted", c => c.Int(nullable: false));
            AlterColumn("dbo.Employees", "Name", c => c.String(maxLength: 20));
            AlterColumn("dbo.Employees", "Sex", c => c.String(maxLength: 10));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Employees", "Sex", c => c.String());
            AlterColumn("dbo.Employees", "Name", c => c.String());
            DropColumn("dbo.Employees", "IsDeleted");
            DropColumn("dbo.Employees", "Title");
        }
    }
}
