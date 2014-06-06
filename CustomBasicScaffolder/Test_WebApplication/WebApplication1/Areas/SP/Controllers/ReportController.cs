using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebApplication1;

namespace WebApplication1.Areas.SP.Controllers
{
    public class ReportController : Controller
    {
        private SchoolDBEntities1 db = new SchoolDBEntities1();

        //GET: QueryBooks
        public ActionResult QueryBooks()
        {
            return View();
        }

        //POST: QueryBooks
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QueryBooks(QueryBooks_QueryFormViewModel vm)
        {
            vm.Result = db.QueryBooks(vm.queryBookName, vm.queryAuthor).ToList();
            if (vm.DoExport && vm.Result.Count>0)
            {
                return QueryBooksExport(vm.Result);
            }
            return View(vm);
        }


        private ActionResult QueryBooksExport(List<QueryBooks_Result> data)
        {
            //Export Excel
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.Add("QueryBooks_Result");
                int currRow = 1;
                ws.Cells[currRow, 1].Value = "ISBN";
                ws.Cells[currRow, 2].Value = "Book name";
                ws.Cells[currRow, 3].Value = "Author";
                ws.Cells[currRow, 4].Value = "Publish date";
                ws.Cells[currRow, 5].Value = "Version";
                ws.Cells[currRow, 6].Value = "List price";
                foreach (QueryBooks_Result item in data)
                {
                    currRow += 1;
                    ws.Cells[currRow, 1].Value = item.ID;
                    ws.Cells[currRow, 2].Value = item.BOOKNAME;
                    ws.Cells[currRow, 3].Value = item.AUTHOR;
                    ws.Cells[currRow, 4].Value = item.PUBLISH_UTC;
                    ws.Cells[currRow, 5].Value = item.VERSION_NUM;
                    ws.Cells[currRow, 6].Value = item.LIST_PRICE;
                }
                //輸出
                var stream = new MemoryStream();
                package.SaveAs(stream);
                string fileName = string.Format("QueryBooks-{0}.xlsx", System.DateTime.Today.ToString("yyyy-MM-dd"));
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                stream.Position = 0;
                return File(stream, contentType, fileName);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
