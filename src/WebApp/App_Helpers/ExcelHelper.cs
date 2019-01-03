using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebApp
{
    public class ExcelHelper
    {
        public static List<T> GetClassFromExcel<T>(string path) where T : class
        {
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                var retList = new List<T>();

                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();
                var hasHeader = true; // adjust it accordingly( i've mentioned that this is a simple approach)
                var fielddic = new Dictionary<string, int>();
                var idx = 0;
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    var field = (hasHeader ? firstRowCell.Text : string.Format("Column{0}", firstRowCell.Start.Column));
                    fielddic.Add(field, idx++);
                }
                var startRow = hasHeader ? 2 : 1;
                for (var rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    var objT = Activator.CreateInstance<T>();
                    var myType = typeof(T);
                    var myProp = myType.GetProperties();

                    for (var i = 0; i < myProp.Count(); i++)
                    {
                        var colidx = fielddic[myProp[i].Name];
                        myProp[i].SetValue(objT, wsRow[rowNum, colidx + 1].Value);
                    }
                    retList.Add(objT);

                }


                return retList;
            }
        }

        public static DataTable GetDataTableFromExcel(string path)
        {
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();
                var tbl = new DataTable();
                var hasHeader = true; // adjust it accordingly( i've mentioned that this is a simple approach)
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (var rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    var row = tbl.NewRow();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = getCellText(cell);
                    }
                    tbl.Rows.Add(row);
                }
                return tbl;
            }
        }

        public static DataTable GetDataTableFromExcel(Stream filestream)
        {
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {

                pck.Load(filestream);

                var ws = pck.Workbook.Worksheets.First();
                var tbl = new DataTable();
                var hasHeader = true; // adjust it accordingly( i've mentioned that this is a simple approach)
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (var rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    var row = tbl.NewRow();
                    foreach (var cell in wsRow)
                    {
                        //row[cell.Start.Column - 1] = cell.Text;
                        row[cell.Start.Column - 1] = getCellText(cell);
                    }
                    tbl.Rows.Add(row);
                }
                return tbl;
            }
        }
        private static string getCellText(ExcelRangeBase cell) {
           
      
            var txt = cell.Text;
            if (string.IsNullOrEmpty(txt)) {
                return string.Empty;
            }
            var ty = cell.Value.GetType();
            if (ty == typeof(string))
            {
                return cell.Text;
            }
            else if (ty == typeof(bool))
            {
                return cell.Value.ToString();
            }
            else if (ty == typeof(DateTime))
            {
                var style = DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal;
                //DateTime result;
                //TimeSpan timespan;
                if (DateTime.TryParse(cell.Value.ToString(), CultureInfo.CurrentCulture, style, out var result))
                {
                    return result.ToString(CultureInfo.CurrentCulture);
                }
                else if (TimeSpan.TryParse(txt, out var timespan)){
                    return DateTime.Now.Add(timespan).ToString(CultureInfo.CurrentCulture);
                }
                else
                {
                    return DateTime.MinValue.ToString(CultureInfo.CurrentCulture);
                }

            }
            else if (ty == typeof(decimal) || ty == typeof(double) ||
               ty == typeof(float))
            {
                 
                if (decimal.TryParse(txt, out var num))
                {
                    return num.ToString();
                }
                else
                {
                    return txt;
                }
            }
            else
            {
                return txt;
            }
        }

        public static string ExcelContentType => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public static DataTable ToDataTable<T>(List<T> data)
        {
            var props = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();

            for (var i = 0; i < props.Count; i++)
            {
                var prop = props[i];
                //table.Columns.Add(prop.Name, prop.PropertyType);
                //string displayName = AttributeHelper.GetDisplayName(data.First(), prop.Name);
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType); // to avoid nullable types
            }

            var values = new object[props.Count];
            foreach (var item in data)
            {
                for (var i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }

                table.Rows.Add(values);
            }
            return table;
        }

        public static Stream  ExportExcel(DataTable dt,  params string[] ignoredColumns)
        {
            var Heading="";
            var stream = new MemoryStream();
            using (var pck = new ExcelPackage())
            {
                var ws = pck.Workbook.Worksheets.Add("Exported Data");
                var StartFromRow = string.IsNullOrEmpty(Heading) ? 1 : 3;

                // add the content into the Excel file
                ws.Cells["A" + StartFromRow].LoadFromDataTable(dt, true);

                // autofit width of cells with small content
                var colindex = 1;
                foreach (DataColumn col in dt.Columns)
                {
                    var columnCells = ws.Cells[ws.Dimension.Start.Row, colindex, ws.Dimension.End.Row, colindex];
                    var maxLength = columnCells.Max(cell => cell.Value.ToString().Count());
                    if (maxLength < 150)
                        ws.Column(colindex).AutoFit();

                    colindex++;
                }

                // format header - bold, yellow on black
                using (var r = ws.Cells[StartFromRow, 1, StartFromRow, dt.Columns.Count])
                {
                    r.Style.Font.Color.SetColor(System.Drawing.Color.Yellow);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black);
                }

                // format cells - add borders
                using (var r = ws.Cells[StartFromRow + 1, 1, StartFromRow + dt.Rows.Count, dt.Columns.Count])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                }

                // removed ignored columns
                for (var i = dt.Columns.Count - 1; i >= 0; i--)
                {
                    if (ignoredColumns.Contains(dt.Columns[i].ColumnName))
                    {
                        ws.DeleteColumn(i + 1);
                    }
                }

                // add header and an additional column (left) and row (top)
                if (!string.IsNullOrEmpty(Heading))
                {
                    ws.Cells["A1"].Value = Heading;
                    ws.Cells["A1"].Style.Font.Size = 20;

                    ws.InsertColumn(1, 1);
                    ws.InsertRow(1, 1);
                    ws.Column(1).Width = 5;
                }

                pck.SaveAs(stream);
            }
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        public static Stream ExportExcel(DataSet ds, params string[] ignoredColumns)
        {
            var Heading = "";
            var stream = new MemoryStream();
            using (var pck = new ExcelPackage())
            {
                foreach (DataTable dt in ds.Tables)
                {
                    var ws = pck.Workbook.Worksheets.Add("Exported Data");
                    var StartFromRow = string.IsNullOrEmpty(Heading) ? 1 : 3;

                    // add the content into the Excel file
                    ws.Cells["A" + StartFromRow].LoadFromDataTable(dt, true);

                    // autofit width of cells with small content
                    var colindex = 1;
                    foreach (DataColumn col in dt.Columns)
                    {
                        var columnCells = ws.Cells[ws.Dimension.Start.Row, colindex, ws.Dimension.End.Row, colindex];
                        var maxLength = columnCells.Max(cell => cell.Value.ToString().Count());
                        if (maxLength < 150)
                            ws.Column(colindex).AutoFit();

                        colindex++;
                    }

                    // format header - bold, yellow on black
                    using (var r = ws.Cells[StartFromRow, 1, StartFromRow, dt.Columns.Count])
                    {
                        r.Style.Font.Color.SetColor(System.Drawing.Color.Yellow);
                        r.Style.Font.Bold = true;
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black);
                    }

                    // format cells - add borders
                    using (var r = ws.Cells[StartFromRow + 1, 1, StartFromRow + dt.Rows.Count, dt.Columns.Count])
                    {
                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                    }

                    // removed ignored columns
                    for (var i = dt.Columns.Count - 1; i >= 0; i--)
                    {
                        if (ignoredColumns.Contains(dt.Columns[i].ColumnName))
                        {
                            ws.DeleteColumn(i + 1);
                        }
                    }

                    // add header and an additional column (left) and row (top)
                    if (!string.IsNullOrEmpty(Heading))
                    {
                        ws.Cells["A1"].Value = Heading;
                        ws.Cells["A1"].Style.Font.Size = 20;

                        ws.InsertColumn(1, 1);
                        ws.InsertRow(1, 1);
                        ws.Column(1).Width = 5;
                    }
                }

                pck.SaveAs(stream);
            }

            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }




        public static Stream ExportExcel<T>(List<T> list, params string[] ignoredColumns)
        {
            var Heading = "";
            var stream = new MemoryStream();
            using (var pck = new ExcelPackage())
            {
                var ws = pck.Workbook.Worksheets.Add("Sheet1");
                var StartFromRow = string.IsNullOrEmpty(Heading) ? 1 : 3;

                // add the content into the Excel file
                ws.Cells["A" + StartFromRow].LoadFromCollection<T>(list, true, OfficeOpenXml.Table.TableStyles.None);

                // autofit width of cells with small content
                var colindex = 1;
                //
                var props = TypeDescriptor.GetProperties(typeof(T));
                foreach (var col in props)
                {
                    var columnCells = ws.Cells[ws.Dimension.Start.Row, colindex, ws.Dimension.End.Row, colindex];
                    var maxLength = columnCells.Max(cell => (cell.Value == null ? "" : cell.Value.ToString()).Count());
                    if (maxLength < 150)
                        ws.Column(colindex).AutoFit();

                    colindex++;
                }

                // format header - bold, yellow on black
                using (var r = ws.Cells[StartFromRow, 1, StartFromRow, props.Count])
                {
                    //r.Style.Font.Color.SetColor(System.Drawing.Color.Yellow);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // format cells - add borders
                using (var r = ws.Cells[StartFromRow + 1, 1, StartFromRow + list.Count, props.Count])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                }

                // removed ignored columns
                for (var i = props.Count - 1; i >= 0; i--)
                {
                    if (ignoredColumns.Contains(props[i].Name))
                    {
                        ws.DeleteColumn(i + 1);
                    }
                }

                // add header and an additional column (left) and row (top)
                if (!string.IsNullOrEmpty(Heading))
                {
                    ws.Cells["A1"].Value = Heading;
                    ws.Cells["A1"].Style.Font.Size = 20;

                    ws.InsertColumn(1, 1);
                    ws.InsertRow(1, 1);
                    ws.Column(1).Width = 5;
                }

                pck.SaveAs(stream);
            }
            stream.Seek(0, SeekOrigin.Begin);
            return stream;

        }

        public static FileInfo ExportExcel<T>(List<T> list, string fileName, params string[] ignoredColumns)
        {
            var Heading = "";
            var fileinfo = new FileInfo(HttpContext.Current.Server.MapPath("~" + fileName));
            using (var pck = new ExcelPackage())
            {
                var ws = pck.Workbook.Worksheets.Add("Sheet1");
                var StartFromRow = string.IsNullOrEmpty(Heading) ? 1 : 3;

                // add the content into the Excel file
                ws.Cells["A" + StartFromRow].LoadFromCollection<T>(list, true, OfficeOpenXml.Table.TableStyles.None);

                // autofit width of cells with small content
                var colindex = 1;
                //
                var props = TypeDescriptor.GetProperties(typeof(T));
                foreach (var col in props)
                {
                    var columnCells = ws.Cells[ws.Dimension.Start.Row, colindex, ws.Dimension.End.Row, colindex];
                    var maxLength = columnCells.Max(cell => (cell.Value == null ? "" : cell.Value.ToString()).Count());
                    if (maxLength < 150)
                        ws.Column(colindex).AutoFit();

                    colindex++;
                }

                // format header - bold, yellow on black
                using (var r = ws.Cells[StartFromRow, 1, StartFromRow, props.Count])
                {
                    //r.Style.Font.Color.SetColor(System.Drawing.Color.Yellow);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }

                // format cells - add borders
                using (var r = ws.Cells[StartFromRow + 1, 1, StartFromRow + list.Count, props.Count])
                {
                    r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                    r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                    r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);
                }

                // removed ignored columns
                for (var i = props.Count - 1; i >= 0; i--)
                {
                    if (ignoredColumns.Contains(props[i].Name))
                    {
                        ws.DeleteColumn(i + 1);
                    }
                }

                // add header and an additional column (left) and row (top)
                if (!string.IsNullOrEmpty(Heading))
                {
                    ws.Cells["A1"].Value = Heading;
                    ws.Cells["A1"].Style.Font.Size = 20;

                    ws.InsertColumn(1, 1);
                    ws.InsertRow(1, 1);
                    ws.Column(1).Width = 5;
                }

                pck.SaveAs(fileinfo);
            }
            return fileinfo;

        }


        public static Stream ExportExcel<T>(Type modelType, List<T> list, params string[] ignoredColumns)
        {
            var Heading = "";
            var stream = new MemoryStream();
            using (var pck = new ExcelPackage())
            {
                var ws = pck.Workbook.Worksheets.Add(modelType.Name);
                var StartFromRow = string.IsNullOrEmpty(Heading) ? 1 : 3;

                // add the content into the Excel file
                ws.Cells["A" + StartFromRow].LoadFromCollection<T>(list, true, OfficeOpenXml.Table.TableStyles.None);

                var _ty = typeof(T);
                var t = _ty;
                var PropertyInfos = t.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);

                for (var i = 1; i <= PropertyInfos.Count(); i++)
                {
                    //string fieldName = ws.Cells[1, i].Value.ToString();//PropertyInfos.Where(x => x.Name == ws.Cells[0, i].Value.ToString()).Select(x=>x.GetCustomAttributes(typeof(DisplayAttribute), true).);
                    var displayName = AttributeHelper.GetDisplayName(modelType, PropertyInfos[i - 1].Name);
                    var fieldtype = PropertyInfos[i - 1].PropertyType;
                    var cell = ws.Cells[1, i];
                    if (fieldtype == typeof(string))
                    {
                        cell.Value = displayName;
                    }
                    else if (fieldtype == typeof(int))
                    {
                        cell.Value = displayName;
                    }
                    else if (fieldtype == typeof(decimal) || fieldtype == typeof(Nullable<decimal>))
                    {
                        cell.Value = displayName;
                        cell.Style.Numberformat.Format = "#,##0.00";
                    }
                    else if (fieldtype == typeof(DateTime) || fieldtype == typeof(Nullable<DateTime>))
                    {
                        cell.Value = displayName;
                        cell.Style.Numberformat.Format = "mm/dd/yyyy hh:mm";
                    }
                    else
                    {
                        cell.Value = displayName;
                    }
                    
                    
                     
                }

                // autofit width of cells with small content
                var colindex = 1;
                //
                var props = TypeDescriptor.GetProperties(typeof(T));
                foreach (var col in props)
                {
                    var fieldtype=PropertyInfos[colindex - 1].PropertyType;
                    var fieldname= PropertyInfos[colindex - 1].Name;
                    var columnCells = ws.Cells[ws.Dimension.Start.Row, colindex, ws.Dimension.End.Row, colindex];
                   
                    if (fieldtype == typeof(decimal) || fieldtype == typeof(Nullable<decimal>))
                    {
                        columnCells.Style.Numberformat.Format = "#,##0.00";
                    }
                    else if (fieldtype == typeof(DateTime) || fieldtype == typeof(Nullable<DateTime>))
                    {
                        if (fieldname.ToLower().IndexOf("time") > 0)
                        {
                            columnCells.Style.Numberformat.Format = "mm/dd/yyyy hh:mm";
                        }
                        else
                        {
                            columnCells.Style.Numberformat.Format = "mm/dd/yyyy";
                        }
                        
                    }
                    else
                    {
                        
                    }
                    var maxLength = columnCells.Max(cell => ( cell.Value == null ? "" : cell.Text.ToString() ).Count());
                    if (maxLength < 150)
                    {
                        ws.Column(colindex).AutoFit();
                    }
                    colindex++;
                }

                // format header - bold, yellow on black
                using (var r = ws.Cells[StartFromRow, 1, StartFromRow, props.Count])
                {
                    //r.Style.Font.Color.SetColor(System.Drawing.Color.Yellow);
                    r.AutoFilter = true;
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                }
                if (list.Count > 0)
                {
                    // format cells - add borders
                    using (var r = ws.Cells[StartFromRow + 0, 1, StartFromRow + list.Count, props.Count])
                    {
                        r.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        r.Style.Border.Right.Style = ExcelBorderStyle.Thin;

                        r.Style.Border.Top.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Bottom.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Left.Color.SetColor(System.Drawing.Color.Black);
                        r.Style.Border.Right.Color.SetColor(System.Drawing.Color.Black);

                         
                    }
                }

                // removed ignored columns
                for (var i = props.Count - 1; i >= 0; i--)
                {
                    if (ignoredColumns.Contains(props[i].Name))
                    {
                        ws.DeleteColumn(i + 1);
                    }
                }

                // add header and an additional column (left) and row (top)
                if (!string.IsNullOrEmpty(Heading))
                {
                    ws.Cells["A1"].Value = Heading;
                    ws.Cells["A1"].Style.Font.Size = 20;

                    ws.InsertColumn(1, 1);
                    ws.InsertRow(1, 1);
                    ws.Column(1).Width = 5;
                }

                pck.SaveAs(stream);
            }
            stream.Seek(0, SeekOrigin.Begin);
            return stream;

        }
    }
}