



using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Repository.Pattern.Repositories;
using Service.Pattern;

using WebApp.Models;
using WebApp.Repositories;
using System.Reflection;
using Repository.Pattern.Ef6;
using Z.EntityFramework.Plus;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace WebApp.Services
{
    public class DataTableImportMappingService : Service<DataTableImportMapping>, IDataTableImportMappingService
    {

        private readonly IRepositoryAsync<DataTableImportMapping> _repository;
        public DataTableImportMappingService(IRepositoryAsync<DataTableImportMapping> repository)
            : base(repository)
        {
            _repository = repository;
        }




        public IEnumerable<EntityInfo> GetAssemblyEntities()
        {
            List<EntityInfo> list = new List<EntityInfo>();

            Assembly asm = Assembly.GetExecutingAssembly();

            list = asm.GetTypes()
                   .Where(type => typeof(Entity).IsAssignableFrom(type))
                   .SelectMany(type => type.GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                   .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                   .Select(x => new EntityInfo { EntitySetName = x.DeclaringType.Name, FieldName = x.Name, FieldTypeName = x.PropertyType.Name, IsRequired = x.GetCustomAttributes().Where(f => f.TypeId.ToString().IndexOf("Required") >= 0).Any() })
                   .OrderBy(x => x.EntitySetName).ThenBy(x => x.FieldName).ToList();




            return list;
        }


        public void GenerateByEnityName(string entityName)
        {
            
            Assembly asm = Assembly.GetExecutingAssembly();
            var list = asm.GetTypes()
                   .Where(type => typeof(Entity).IsAssignableFrom(type))
                   .SelectMany(type => type.GetProperties(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                   .Where(m => m.DeclaringType.Name == entityName &&
                               m.PropertyType.BaseType !=typeof(Entity) &&
                             !m.GetCustomAttributes(
                                 typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute),
                                 true).Any()
                         )
                   .Select(x =>
                            new EntityInfo
                            {
                                EntitySetName = x.DeclaringType.Name,
                                FieldName = x.Name,
                                FieldTypeName = x.PropertyType.Name,
                                IsRequired = x.GetCustomAttributes()
                                            .Where(f => 
                                                    f.TypeId.ToString().IndexOf("Required") >= 0
                                                   ).Any() || 
                                                   (x.PropertyType==typeof(Int32) ||
                                                   x.PropertyType == typeof(DateTime) ||
                                                   x.PropertyType == typeof(Decimal) 
                                                   )
                            })
                   .OrderBy(x => x.EntitySetName)
                   .Where(x =>  x.FieldTypeName != "ICollection`1").ToList();

            var entityType = asm.GetTypes()
                   .Where(type => typeof(Entity).IsAssignableFrom(type)).Where(x => x.Name == entityName).First();

            this.Queryable().Where(x => x.EntitySetName == entityName).Delete();
            foreach (var item in list)
            {
                
                    DataTableImportMapping row = new DataTableImportMapping();
                    row.EntitySetName = item.EntitySetName;
                    row.FieldName = item.FieldName;
                    row.IsRequired = item.IsRequired;
                    row.TypeName = item.FieldTypeName;
                    row.IsEnabled = item.IsRequired;
                    row.SourceFieldName = AttributeHelper.GetDisplayName(entityType, item.FieldName);
                    this.Insert(row);
                
            }
        }


        public DataTableImportMapping FindMapping(string entitySetName, string sourceFieldName)
        {
            return this.Queryable().Where(x => x.EntitySetName == entitySetName && x.SourceFieldName == sourceFieldName).FirstOrDefault();
        }

        public void CreateExcelTemplate(string entityname, string filename)
        {
            var mapping = this.Queryable().Where(x => x.EntitySetName == entityname && x.IsEnabled == true).ToList();
            FileInfo finame = new FileInfo(filename);
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
            using (ExcelPackage excel = new ExcelPackage(finame))
            {
                var sheet = excel.Workbook.Worksheets.Add(entityname);
                int col = 0;
                foreach (var row in mapping) {
                    col++;
                    sheet.Cells[1, col].Value = row.SourceFieldName;
                    sheet.Cells[1, col].Style.Font.Bold = true;
                    sheet.Cells[1, col].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[1, col].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[1, col].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[1, col].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    sheet.Cells[1, col].Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    sheet.Cells[1, col].Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    sheet.Cells[1, col].Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    sheet.Cells[1, col].Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                    sheet.Cells[1, col].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    sheet.Cells[1, col].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                    if (row.TypeName=="DateTime")
                        sheet.Cells[1, col].Style.Numberformat.Format = "mm-dd-yyyy";
                }
                excel.Save();
            }
        }
         
    }
}



