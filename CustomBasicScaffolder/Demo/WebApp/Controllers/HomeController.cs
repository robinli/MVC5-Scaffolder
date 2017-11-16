using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PublicPara.CodeText.Data;
namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            var paras = new List<object>();
            var sql = "INSERT INTO [dbo].[Tb1]([f1]) VALUES (@f1)";
            paras.Add(new { f1="a" });
            paras.Add(new { f1 = "a" });
            paras.Add(new { f1 = "a" });
            paras.Add(new { f1 = "a" });
            paras.Add(new { f1 = "a" });

            //var ds= SqlHelper2.DatabaseFactory.CreateDatabase().ExecuteDataSet("select * from tb1",null);
          SqlHelper2.DatabaseFactory.CreateDatabase().ExecuteDataSet("select * from tb1", null,ds=> {
              Console.Write(ds);
          });
            SqlHelper2.DatabaseFactory.CreateDatabase().ExecuteDataTable("select * from tb1", null, dt => {
                Console.Write(dt);
            });


            DateTime.Now.ToString("G");
            var list = CodeListSet.CLS["AccountType"].EnumRecords();
            var val = CodeListSet.CLS["AccountType"].Code2Value("1");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult GetTime()
        {
            //ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult BlankPage() {
            return View();
        }
        public ActionResult AgileBoard() {
            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}