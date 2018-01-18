using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PublicPara.CodeText.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
       
        public ActionResult Index()
        {
            

            //  var parameters = new List<object>();
            //  var sql = "INSERT INTO [dbo].[Tb1]([f1]) VALUES (@f1)";
            //  parameters.Add(new { f1="a" });
            //  parameters.Add(new { f1 = "b" });
            //  parameters.Add(new { f1 = "c" });
            //  parameters.Add(new { f1 = "d" });
            //  parameters.Add(new { f1 = "e" });
            //  SqlHelper2.DatabaseFactory.CreateDatabase().ExecuteNonQuery(sql, parameters);

            //  var sqllist = new List<string>();
            //  sqllist.Add("INSERT INTO [dbo].[Tb1]([f1]) VALUES ('a')");
            //  sqllist.Add("INSERT INTO [dbo].[Tb1]([f1]) VALUES ('b')");
            //  sqllist.Add("INSERT INTO [dbo].[Tb1]([f1]) VALUES ('c')");
            //  SqlHelper2.DatabaseFactory.CreateDatabase().ExecuteNonQuery(sqllist);
            //  //var ds= SqlHelper2.DatabaseFactory.CreateDatabase().ExecuteDataSet("select * from tb1",null);
            //  SqlHelper2.DatabaseFactory.CreateDatabase().ExecuteDataSet("select * from tb1", null,ds=> {
            //    Console.Write(ds);
            //});
            //  SqlHelper2.DatabaseFactory.CreateDatabase().ExecuteDataTable("select * from tb1", null, dt => {
            //      Console.Write(dt);
            //  });


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