using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using WebApplication1;

namespace WebApplication1.Areas.SP2.Controllers
{
    public class ReportController : Controller
    {
        private SchoolDBEntities1 db = new SchoolDBEntities1();

        //GET: QueryBooks2
        public ActionResult QueryBooks2()
        {
            return View();
        }

        //POST: QueryBooks2
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult QueryBooks2(QueryBooks2_QueryFormViewModel vm)
        {
            vm.Result = db.QueryBooks2(vm.queryBookName).ToList();
            if (vm.DoExport && vm.Result.Count>0)
            {
                return QueryBooks2Export(vm.Result);
            }
            return View(vm);
        }


        private ActionResult QueryBooks2Export(List<QueryBooks2_Result> data)
        {
            //Export Excel
            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet ws = package.Workbook.Worksheets.Add("QueryBooks2_Result");
                int currRow = 1;
                ws.Cells[currRow, 1].Value = "ISBN";
                ws.Cells[currRow, 2].Value = "BOOK NAME";
                ws.Cells[currRow, 3].Value = "AUTHOR name";
                ws.Cells[currRow, 4].Value = "PUBLISH";
                ws.Cells[currRow, 5].Value = "VERSION";
                ws.Cells[currRow, 6].Value = "PRICE";
                foreach (QueryBooks2_Result item in data)
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
                string fileName = string.Format("QueryBooks2-{0}.xlsx", System.DateTime.Today.ToString("yyyy-MM-dd"));
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
