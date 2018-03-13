using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
   
    public class MiscController : Controller
    {
        // GET: /misc/pricing
        public ActionResult Pricing()
        {
            return View();
        }

        // GET: /misc/invoice
        public ActionResult Invoice()
        {
            return View();
        }

        // GET: /misc/blank
        public ActionResult Blank()
        {
            return View();
        }

        // GET: /misc/email-template
        public ActionResult EmailTemplate()
        {
            return View();
        }

        // GET: /misc/search
        public ActionResult Search()
        {
            return View();
        }

        // GET: /misc/ck-editor
        public ActionResult CKEditor()
        {
            return View();
        }

        // GET: /misc/error404
        public ActionResult Error404()
        {
            return View();
        }

        // GET: /misc/error500
        public ActionResult Error500()
        {
            return View();
        }
    }
}