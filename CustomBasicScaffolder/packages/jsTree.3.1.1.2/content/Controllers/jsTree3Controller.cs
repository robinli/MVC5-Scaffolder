using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using jsTree3.Models;
using Newtonsoft.Json;

namespace jsTree3.Controllers
{
    public class jsTree3Controller : Controller
    {
        public ActionResult Demo()
        {
            return View();
        }

        public ActionResult AJAXDemo()
        {
            return View();
        }
        public JsonResult GetJsTree3Data()
        {
            var root = new JsTree3Node() // Create our root node and ensure it is opened
            {
                id = Guid.NewGuid().ToString(),
                text = "Root Node",
                state = new State(true, false, false)
            };
            
            // Create a basic structure of nodes
            var children = new List<JsTree3Node>();
            for (int i = 0; i < 5; i++)
            {
                var node = JsTree3Node.NewNode(Guid.NewGuid().ToString());
                node.state = new State(IsPrime(i), false, false);

                for (int y = 0; y < 5; y++)
                {
                    node.children.Add(JsTree3Node.NewNode(Guid.NewGuid().ToString()));
                }

                children.Add(node);
            }

            // Add the sturcture to the root nodes children property
            root.children = children;
            
            // Return the object as JSON
            return Json(root, JsonRequestBehavior.AllowGet);
        }

        static bool IsPrime(int n)
        {
            if (n > 1)
            {
                return Enumerable.Range(1, n).Where(x => n % x == 0)
                                 .SequenceEqual(new[] { 1, n });
            }

            return false;
        }
    }
}
