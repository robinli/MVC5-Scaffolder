namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class adf : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BaseCodes", "Created", c => c.DateTime());
            AddColumn("dbo.BaseCodes", "CreatedBy", c => c.String());
            AddColumn("dbo.BaseCodes", "LastModified", c => c.DateTime());
            AddColumn("dbo.BaseCodes", "LastModifiedBy", c => c.String());
            AddColumn("dbo.CodeItems", "Created", c => c.DateTime());
            AddColumn("dbo.CodeItems", "CreatedBy", c => c.String());
            AddColumn("dbo.CodeItems", "LastModified", c => c.DateTime());
            AddColumn("dbo.CodeItems", "LastModifiedBy", c => c.String());
            AddColumn("dbo.Categories", "Created", c => c.DateTime());
            AddColumn("dbo.Categories", "CreatedBy", c => c.String());
            AddColumn("dbo.Categories", "LastModified", c => c.DateTime());
            AddColumn("dbo.Categories", "LastModifiedBy", c => c.String());
            AddColumn("dbo.Products", "Created", c => c.DateTime());
            AddColumn("dbo.Products", "CreatedBy", c => c.String());
            AddColumn("dbo.Products", "LastModified", c => c.DateTime());
            AddColumn("dbo.Products", "LastModifiedBy", c => c.String());
            AddColumn("dbo.Companies", "Created", c => c.DateTime());
            AddColumn("dbo.Companies", "CreatedBy", c => c.String());
            AddColumn("dbo.Companies", "LastModified", c => c.DateTime());
            AddColumn("dbo.Companies", "LastModifiedBy", c => c.String());
            AddColumn("dbo.Departments", "Created", c => c.DateTime());
            AddColumn("dbo.Departments", "CreatedBy", c => c.String());
            AddColumn("dbo.Departments", "LastModified", c => c.DateTime());
            AddColumn("dbo.Departments", "LastModifiedBy", c => c.String());
            AddColumn("dbo.Employees", "Created", c => c.DateTime());
            AddColumn("dbo.Employees", "CreatedBy", c => c.String());
            AddColumn("dbo.Employees", "LastModified", c => c.DateTime());
            AddColumn("dbo.Employees", "LastModifiedBy", c => c.String());
            AddColumn("dbo.DataTableImportMappings", "Created", c => c.DateTime());
            AddColumn("dbo.DataTableImportMappings", "CreatedBy", c => c.String());
            AddColumn("dbo.DataTableImportMappings", "LastModified", c => c.DateTime());
            AddColumn("dbo.DataTableImportMappings", "LastModifiedBy", c => c.String());
            AddColumn("dbo.MenuItems", "Created", c => c.DateTime());
            AddColumn("dbo.MenuItems", "CreatedBy", c => c.String());
            AddColumn("dbo.MenuItems", "LastModified", c => c.DateTime());
            AddColumn("dbo.MenuItems", "LastModifiedBy", c => c.String());
            AddColumn("dbo.Messages", "CreatedBy", c => c.String());
            AddColumn("dbo.Messages", "LastModified", c => c.DateTime());
            AddColumn("dbo.Messages", "LastModifiedBy", c => c.String());
            AddColumn("dbo.Notifications", "CreatedBy", c => c.String());
            AddColumn("dbo.Notifications", "LastModified", c => c.DateTime());
            AddColumn("dbo.Notifications", "LastModifiedBy", c => c.String());
            AddColumn("dbo.OrderDetails", "Created", c => c.DateTime());
            AddColumn("dbo.OrderDetails", "CreatedBy", c => c.String());
            AddColumn("dbo.OrderDetails", "LastModified", c => c.DateTime());
            AddColumn("dbo.OrderDetails", "LastModifiedBy", c => c.String());
            AddColumn("dbo.Orders", "Created", c => c.DateTime());
            AddColumn("dbo.Orders", "CreatedBy", c => c.String());
            AddColumn("dbo.Orders", "LastModified", c => c.DateTime());
            AddColumn("dbo.Orders", "LastModifiedBy", c => c.String());
            AddColumn("dbo.RoleMenus", "Created", c => c.DateTime());
            AddColumn("dbo.RoleMenus", "CreatedBy", c => c.String());
            AddColumn("dbo.RoleMenus", "LastModified", c => c.DateTime());
            AddColumn("dbo.RoleMenus", "LastModifiedBy", c => c.String());
            AddColumn("dbo.Works", "Created", c => c.DateTime());
            AddColumn("dbo.Works", "CreatedBy", c => c.String());
            AddColumn("dbo.Works", "LastModified", c => c.DateTime());
            AddColumn("dbo.Works", "LastModifiedBy", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Works", "LastModifiedBy");
            DropColumn("dbo.Works", "LastModified");
            DropColumn("dbo.Works", "CreatedBy");
            DropColumn("dbo.Works", "Created");
            DropColumn("dbo.RoleMenus", "LastModifiedBy");
            DropColumn("dbo.RoleMenus", "LastModified");
            DropColumn("dbo.RoleMenus", "CreatedBy");
            DropColumn("dbo.RoleMenus", "Created");
            DropColumn("dbo.Orders", "LastModifiedBy");
            DropColumn("dbo.Orders", "LastModified");
            DropColumn("dbo.Orders", "CreatedBy");
            DropColumn("dbo.Orders", "Created");
            DropColumn("dbo.OrderDetails", "LastModifiedBy");
            DropColumn("dbo.OrderDetails", "LastModified");
            DropColumn("dbo.OrderDetails", "CreatedBy");
            DropColumn("dbo.OrderDetails", "Created");
            DropColumn("dbo.Notifications", "LastModifiedBy");
            DropColumn("dbo.Notifications", "LastModified");
            DropColumn("dbo.Notifications", "CreatedBy");
            DropColumn("dbo.Messages", "LastModifiedBy");
            DropColumn("dbo.Messages", "LastModified");
            DropColumn("dbo.Messages", "CreatedBy");
            DropColumn("dbo.MenuItems", "LastModifiedBy");
            DropColumn("dbo.MenuItems", "LastModified");
            DropColumn("dbo.MenuItems", "CreatedBy");
            DropColumn("dbo.MenuItems", "Created");
            DropColumn("dbo.DataTableImportMappings", "LastModifiedBy");
            DropColumn("dbo.DataTableImportMappings", "LastModified");
            DropColumn("dbo.DataTableImportMappings", "CreatedBy");
            DropColumn("dbo.DataTableImportMappings", "Created");
            DropColumn("dbo.Employees", "LastModifiedBy");
            DropColumn("dbo.Employees", "LastModified");
            DropColumn("dbo.Employees", "CreatedBy");
            DropColumn("dbo.Employees", "Created");
            DropColumn("dbo.Departments", "LastModifiedBy");
            DropColumn("dbo.Departments", "LastModified");
            DropColumn("dbo.Departments", "CreatedBy");
            DropColumn("dbo.Departments", "Created");
            DropColumn("dbo.Companies", "LastModifiedBy");
            DropColumn("dbo.Companies", "LastModified");
            DropColumn("dbo.Companies", "CreatedBy");
            DropColumn("dbo.Companies", "Created");
            DropColumn("dbo.Products", "LastModifiedBy");
            DropColumn("dbo.Products", "LastModified");
            DropColumn("dbo.Products", "CreatedBy");
            DropColumn("dbo.Products", "Created");
            DropColumn("dbo.Categories", "LastModifiedBy");
            DropColumn("dbo.Categories", "LastModified");
            DropColumn("dbo.Categories", "CreatedBy");
            DropColumn("dbo.Categories", "Created");
            DropColumn("dbo.CodeItems", "LastModifiedBy");
            DropColumn("dbo.CodeItems", "LastModified");
            DropColumn("dbo.CodeItems", "CreatedBy");
            DropColumn("dbo.CodeItems", "Created");
            DropColumn("dbo.BaseCodes", "LastModifiedBy");
            DropColumn("dbo.BaseCodes", "LastModified");
            DropColumn("dbo.BaseCodes", "CreatedBy");
            DropColumn("dbo.BaseCodes", "Created");
        }
    }
}
