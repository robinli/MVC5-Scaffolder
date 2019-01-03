#region Using

using System.Web.Mvc;

#endregion

namespace SmartAdminMvc.Controllers
{
    [Authorize]
    public class IconsController : Controller
    {
        // GET: /icons/fontawesome
        public ActionResult FontAwesome()
        {
            return View();
        }

        // GET: /icons/glyphicons
        public ActionResult GlyphIcons()
        {
            return View();
        }

        // GET: /icons/flags
        public ActionResult Flags()
        {
            return View();
        }


        // GET: /icons/grid
        public ActionResult Grid()
        {
            return View();
        }

        // GET: /icons/treeview
        public ActionResult TreeView()
        {
            return View();
        }

        // GET: /icons/nested-lists
        public ActionResult NestedLists()
        {
            return View();
        }

        // GET: /icons/jquery-ui
        public ActionResult JQueryUI()
        {
            return View();
        }

        // GET: /icons/typography
        public ActionResult Typography()
        {
            return View();
        }
    }
}