using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //SchoolDBEntities1 db = new SchoolDBEntities1();
            //ObjectResult<QueryBooks_Result> result=db.QueryBooks("*");

            //GetMethodInfo("WebApplication1.SchoolDBEntities1");

            return View();
        }

        private void GetMethodInfo(string typeName)
        {
            Type myType = Type.GetType(typeName);
            MethodInfo[] myArrayMethodInfo = myType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            foreach (MethodInfo myMethod in myArrayMethodInfo.Where(m => m.ReturnType.BaseType.ToString() == "System.Data.Entity.Core.Objects.ObjectResult"))
            {
                string name = myMethod.Name;
                //讀取參數
                foreach (ParameterInfo p1 in myMethod.GetParameters())
                {
                    string pName = p1.Name;
                    string pType = p1.ParameterType.Name;

                }

                string enumType = myMethod.ReturnType.ToString();
                int idx1 = enumType.IndexOf("[");
                int idx2 = enumType.LastIndexOf("]");

                string baseType = enumType.Substring(idx1 + 1, idx2 - idx1 - 1);
                Type returnModel = Type.GetType(enumType.Substring(idx1 + 1, idx2 - idx1 - 1));

                // 讀取回傳型別
                foreach (PropertyInfo p2 in returnModel.GetProperties())
                {
                    string pName = p2.Name;
                    string pType = p2.PropertyType.Name;
                }

            }


        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}