namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mdp : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Products", "ConfirmDateTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Products", "ConfirmDateTime", c => c.DateTime());
        }
    }
}
