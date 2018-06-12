using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace WebApp
{
    public class ExcelHelper
    {
        public static List<T> getClassFromExcel<T>(string path) where T : class
        {
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                List<T> retList = new List<T>();

                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();
                bool hasHeader = true; // adjust it accordingly( i've mentioned that this is a simple approach)
                var fielddic = new Dictionary<string, int>();
                int idx = 0;
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    string field = (hasHeader ? firstRowCell.Text : string.Format("Column{0}", firstRowCell.Start.Column));
                    fielddic.Add(field, idx++);
                }
                var startRow = hasHeader ? 2 : 1;
                for (var rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    T objT = Activator.CreateInstance<T>();
                    Type myType = typeof(T);
                    PropertyInfo[] myProp = myType.GetProperties();

                    for (int i = 0; i < myProp.Count(); i++)
                    {
                        int colidx = fielddic[myProp[i].Name];
                        myProp[i].SetValue(objT, wsRow[rowNum, colidx + 1].Text);
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
                DataTable tbl = new DataTable();
                bool hasHeader = true; // adjust it accordingly( i've mentioned that this is a simple approach)
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
                        row[cell.Start.Column - 1] = cell.Text;
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
                DataTable tbl = new DataTable();
                bool hasHeader = true; // adjust it accordingly( i've mentioned that this is a simple approach)
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
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                    tbl.Rows.Add(row);
                }
                return tbl;
            }
        }

        public static string ExcelContentType { get { return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; } }

        public static DataTable ToDataTable<T>(List<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();

            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                //table.Columns.Add(prop.Name, prop.PropertyType);
                //string displayName = AttributeHelper.GetDisplayName(data.First(), prop.Name);
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType); // to avoid nullable types
            }

            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }

                table.Rows.Add(values);
            }
            return table;
        }

        public static Stream  ExportExcel(DataTable dt,  params string[] IgnoredColumns)
        {
            string Heading="";
            var stream = new MemoryStream();
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Exported Data");
                int StartFromRow = String.IsNullOrEmpty(Heading) ? 1 : 3;

                // add the content into the Excel file
                ws.Cells["A" + StartFromRow].LoadFromDataTable(dt, true);

                // autofit width of cells with small content
                int colindex = 1;
                foreach (DataColumn col in dt.Columns)
                {
                    ExcelRange columnCells = ws.Cells[ws.Dimension.Start.Row, colindex, ws.Dimension.End.Row, colindex];
                    int maxLength = columnCells.Max(cell => cell.Value.ToString().Count());
                    if (maxLength < 150)
                        ws.Column(colindex).AutoFit();

                    colindex++;
                }

                // format header - bold, yellow on black
                using (ExcelRange r = ws.Cells[StartFromRow, 1, StartFromRow, dt.Columns.Count])
                {
                    r.Style.Font.Color.SetColor(System.Drawing.Color.Yellow);
                    r.Style.Font.Bold = true;
                    r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    r.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black);
                }

                // format cells - add borders
                using (ExcelRange r = ws.Cells[StartFromRow + 1, 1, StartFromRow + dt.Rows.Count, dt.Columns.Count])
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
                for (int i = dt.Columns.Count - 1; i >= 0; i--)
                {
                    if (IgnoredColumns.Contains(dt.Columns[i].ColumnName))
                    {
                        ws.DeleteColumn(i + 1);
                    }
                }

                // add header and an additional column (left) and row (top)
                if (!String.IsNullOrEmpty(Heading))
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

        public static Stream ExportExcel(DataSet ds, params string[] IgnoredColumns)
        {
            string Heading = "";
            var stream = new MemoryStream();
            using (ExcelPackage pck = new ExcelPackage())
            {
                foreach (DataTable dt in ds.Tables)
                {
                    ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Exported Data");
                    int StartFromRow = String.IsNullOrEmpty(Heading) ? 1 : 3;

                    // add the content into the Excel file
                    ws.Cells["A" + StartFromRow].LoadFromDataTable(dt, true);

                    // autofit width of cells with small content
                    int colindex = 1;
                    foreach (DataColumn col in dt.Columns)
                    {
                        ExcelRange columnCells = ws.Cells[ws.Dimension.Start.Row, colindex, ws.Dimension.End.Row, colindex];
                        int maxLength = columnCells.Max(cell => cell.Value.ToString().Count());
                        if (maxLength < 150)
                            ws.Column(colindex).AutoFit();

                        colindex++;
                    }

                    // format header - bold, yellow on black
                    using (ExcelRange r = ws.Cells[StartFromRow, 1, StartFromRow, dt.Columns.Count])
                    {
                        r.Style.Font.Color.SetColor(System.Drawing.Color.Yellow);
                        r.Style.Font.Bold = true;
                        r.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        r.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black);
                    }

                    // format cells - add borders
                    using (ExcelRange r = ws.Cells[StartFromRow + 1, 1, StartFromRow + dt.Rows.Count, dt.Columns.Count])
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
                    for (int i = dt.Columns.Count - 1; i >= 0; i--)
                    {
                        if (IgnoredColumns.Contains(dt.Columns[i].ColumnName))
                        {
                            ws.DeleteColumn(i + 1);
                        }
                    }

                    // add header and an additional column (left) and row (top)
                    if (!String.IsNullOrEmpty(Heading))
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




        public static Stream ExportExcel<T>(List<T> list, params string[] IgnoredColumns)
        {
            string Heading = "";
            var stream = new MemoryStream();
            using (var pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");
                int StartFromRow = String.IsNullOrEmpty(Heading) ? 1 : 3;

                // add the content into the Excel file
                ws.Cells["A" + StartFromRow].LoadFromCollection<T>(list, true, OfficeOpenXml.Table.TableStyles.None);

                // autofit width of cells with small content
                int colindex = 1;
                //
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
                foreach (var col in props)
                {
                    ExcelRange columnCells = ws.Cells[ws.Dimension.Start.Row, colindex, ws.Dimension.End.Row, colindex];
                    int maxLength = columnCells.Max(cell => (cell.Value == null ? "" : cell.Value.ToString()).Count());
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
                for (int i = props.Count - 1; i >= 0; i--)
                {
                    if (IgnoredColumns.Contains(props[i].Name))
                    {
                        ws.DeleteColumn(i + 1);
                    }
                }

                // add header and an additional column (left) and row (top)
                if (!String.IsNullOrEmpty(Heading))
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

        public static FileInfo ExportExcel<T>(List<T> list, string fileName, params string[] IgnoredColumns)
        {
            string Heading = "";
            FileInfo fileinfo = new FileInfo(HttpContext.Current.Server.MapPath("~" + fileName));
            using (ExcelPackage pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Sheet1");
                int StartFromRow = String.IsNullOrEmpty(Heading) ? 1 : 3;

                // add the content into the Excel file
                ws.Cells["A" + StartFromRow].LoadFromCollection<T>(list, true, OfficeOpenXml.Table.TableStyles.None);

                // autofit width of cells with small content
                int colindex = 1;
                //
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
                foreach (var col in props)
                {
                    var columnCells = ws.Cells[ws.Dimension.Start.Row, colindex, ws.Dimension.End.Row, colindex];
                    int maxLength = columnCells.Max(cell => (cell.Value == null ? "" : cell.Value.ToString()).Count());
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
                for (int i = props.Count - 1; i >= 0; i--)
                {
                    if (IgnoredColumns.Contains(props[i].Name))
                    {
                        ws.DeleteColumn(i + 1);
                    }
                }

                // add header and an additional column (left) and row (top)
                if (!String.IsNullOrEmpty(Heading))
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


        public static Stream ExportExcel<T>(Type modelType, List<T> list, params string[] IgnoredColumns)
        {
            string Heading = "";
            var stream = new MemoryStream();
            using (var pck = new ExcelPackage())
            {
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add(modelType.Name);
                int StartFromRow = String.IsNullOrEmpty(Heading) ? 1 : 3;

                // add the content into the Excel file
                ws.Cells["A" + StartFromRow].LoadFromCollection<T>(list, true, OfficeOpenXml.Table.TableStyles.None);

                Type _ty = typeof(T);
                //if (list.Count > 0)
                //{
                //    _ty = list[0].GetType();
                //}

                Type t = _ty;
                System.Reflection.PropertyInfo[] PropertyInfos = t.GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic);

                for (int i = 1; i <= PropertyInfos.Count(); i++)
                {
                    //string fieldName = ws.Cells[1, i].Value.ToString();//PropertyInfos.Where(x => x.Name == ws.Cells[0, i].Value.ToString()).Select(x=>x.GetCustomAttributes(typeof(DisplayAttribute), true).);
                    string displayName = AttributeHelper.GetDisplayName(modelType, PropertyInfos[i - 1].Name);
                    ws.Cells[1, i].Value = displayName;
                }

                // autofit width of cells with small content
                int colindex = 1;
                //
                PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
                foreach (var col in props)
                {
                    ExcelRange columnCells = ws.Cells[ws.Dimension.Start.Row, colindex, ws.Dimension.End.Row, colindex];
                    int maxLength = columnCells.Max(cell => (cell.Value == null ? "" : cell.Value.ToString()).Count());
                    if (maxLength < 150)
                        ws.Column(colindex).AutoFit();

                    colindex++;
                }

                // format header - bold, yellow on black
                using (ExcelRange r = ws.Cells[StartFromRow, 1, StartFromRow, props.Count])
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
                for (int i = props.Count - 1; i >= 0; i--)
                {
                    if (IgnoredColumns.Contains(props[i].Name))
                    {
                        ws.DeleteColumn(i + 1);
                    }
                }

                // add header and an additional column (left) and row (top)
                if (!String.IsNullOrEmpty(Heading))
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