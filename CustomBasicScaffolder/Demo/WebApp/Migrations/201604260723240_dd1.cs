namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dd1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DataTableImportMappings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EntitySetName = c.String(nullable: false, maxLength: 50),
                        FieldName = c.String(nullable: false, maxLength: 50),
                        DefaultValue = c.String(maxLength: 50),
                        TypeName = c.String(maxLength: 30),
                        IsRequired = c.Boolean(nullable: false),
                        SourceFieldName = c.String(maxLength: 50),
                        IsEnabled = c.Boolean(nullable: false),
                        RegularExpression = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => new { t.EntitySetName, t.FieldName }, unique: true, name: "IX_DataTableImportMapping");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.DataTableImportMappings", "IX_DataTableImportMapping");
            DropTable("dbo.DataTableImportMappings");
        }
    }
}
