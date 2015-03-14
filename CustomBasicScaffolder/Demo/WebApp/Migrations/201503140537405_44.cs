namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _44 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Address = c.String(nullable: false, maxLength: 50),
                        City = c.String(maxLength: 20),
                        Province = c.String(maxLength: 20),
                        RegisterDate = c.DateTime(nullable: false),
                        Employees = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Manager = c.String(maxLength: 20),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
            
            
            CreateTable(
                "dbo.Works",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 20),
                        Status = c.String(nullable: false, maxLength: 10),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(),
                        Enableed = c.Boolean(nullable: false),
                        Hour = c.Int(nullable: false),
                        Priority = c.Int(nullable: false),
                        Score = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
             
            DropForeignKey("dbo.Departments", "CompanyId", "dbo.Companies");
     
            DropIndex("dbo.Departments", new[] { "CompanyId" });
            DropTable("dbo.Works");
       
            
            DropTable("dbo.Departments");
            DropTable("dbo.Companies");
        }
    }
}
