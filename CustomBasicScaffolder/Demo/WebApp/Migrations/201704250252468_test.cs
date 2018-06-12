namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
         
            CreateTable(
                "dbo.BaseCodes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CodeType = c.String(nullable: false, maxLength: 30),
                        Description = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.CodeType, unique: true);
            
            CreateTable(
                "dbo.CodeItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 20),
                        Text = c.String(nullable: false, maxLength: 20),
                        BaseCodeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.BaseCodes", t => t.BaseCodeId, cascadeDelete: true)
                .Index(t => new { t.Code, t.BaseCodeId }, unique: true, name: "CodeItem_IX");
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Unit = c.String(maxLength: 10),
                        UnitPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        StockQty = c.Int(nullable: false),
                        ConfirmDateTime = c.DateTime(),
                        IsRequiredQc = c.Boolean(nullable: false),
                        CategoryId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        City = c.String(),
                        Province = c.String(),
                        RegisterDate = c.DateTime(nullable: false),
                        Employees = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Manager = c.String(maxLength: 20),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Sex = c.String(),
                        Age = c.Int(nullable: false),
                        Brithday = c.DateTime(nullable: false),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
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
            
            CreateTable(
                "dbo.MenuItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        Description = c.String(maxLength: 100),
                        Code = c.String(nullable: false, maxLength: 20),
                        Url = c.String(nullable: false, maxLength: 100),
                        Controller = c.String(maxLength: 100),
                        Action = c.String(maxLength: 100),
                        IconCls = c.String(maxLength: 50),
                        IsEnabled = c.Boolean(nullable: false),
                        ParentId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MenuItems", t => t.ParentId)
                .Index(t => t.ParentId);
            
            CreateTable(
                "dbo.OrderDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Qty = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Customer = c.String(nullable: false, maxLength: 20),
                        ShippingAddress = c.String(nullable: false, maxLength: 50),
                        OrderDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoleMenus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false, maxLength: 20),
                        MenuId = c.Int(nullable: false),
                        IsEnabled = c.Boolean(nullable: false),
                        Create = c.Boolean(nullable: false),
                        Edit = c.Boolean(nullable: false),
                        Delete = c.Boolean(nullable: false),
                        Import = c.Boolean(nullable: false),
                        Export = c.Boolean(nullable: false),
                        FunctionPoint1 = c.Boolean(nullable: false),
                        FunctionPoint2 = c.Boolean(nullable: false),
                        FunctionPoint3 = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.MenuItems", t => t.MenuId, cascadeDelete: true)
                .Index(t => new { t.RoleName, t.MenuId }, unique: true, name: "IX_rolemenu");
            
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
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId });
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId });
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.RoleMenus", "MenuId", "dbo.MenuItems");
            DropForeignKey("dbo.OrderDetails", "ProductId", "dbo.Products");
            DropForeignKey("dbo.OrderDetails", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.MenuItems", "ParentId", "dbo.MenuItems");
            DropForeignKey("dbo.Employees", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Departments", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.CodeItems", "BaseCodeId", "dbo.BaseCodes");
            DropIndex("dbo.RoleMenus", "IX_rolemenu");
            DropIndex("dbo.OrderDetails", new[] { "OrderId" });
            DropIndex("dbo.OrderDetails", new[] { "ProductId" });
            DropIndex("dbo.MenuItems", new[] { "ParentId" });
            DropIndex("dbo.DataTableImportMappings", "IX_DataTableImportMapping");
            DropIndex("dbo.Employees", new[] { "CompanyId" });
            DropIndex("dbo.Departments", new[] { "CompanyId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropIndex("dbo.CodeItems", "CodeItem_IX");
            DropIndex("dbo.BaseCodes", new[] { "CodeType" });
            DropTable("dbo.Works");
            DropTable("dbo.RoleMenus");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderDetails");
            DropTable("dbo.MenuItems");
            DropTable("dbo.DataTableImportMappings");
            DropTable("dbo.Employees");
            DropTable("dbo.Departments");
            DropTable("dbo.Companies");
            DropTable("dbo.Products");
            DropTable("dbo.Categories");
            DropTable("dbo.CodeItems");
            DropTable("dbo.BaseCodes");
            CreateIndex("dbo.AspNetUserLogins", "UserId");
            CreateIndex("dbo.AspNetUserClaims", "UserId");
            CreateIndex("dbo.AspNetUsers", "UserName", unique: true, name: "UserNameIndex");
            CreateIndex("dbo.AspNetUserRoles", "RoleId");
            CreateIndex("dbo.AspNetUserRoles", "UserId");
            CreateIndex("dbo.AspNetRoles", "Name", unique: true, name: "RoleNameIndex");
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id", cascadeDelete: true);
        }
    }
}
