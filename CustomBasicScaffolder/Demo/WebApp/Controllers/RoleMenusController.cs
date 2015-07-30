

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using Repository.Pattern.Infrastructure;
using WebApp.Models;
using WebApp.Services;
using WebApp.Repositories;
using WebApp.Extensions;


namespace WebApp.Controllers
{
      [Authorize]
    public class RoleMenusController : Controller
    {
        //private ApplicationUserManager userManager;
        //public ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    }
        //    private set
        //    {
        //        userManager = value;
        //    }
        //}
        //Please RegisterType UnityConfig.cs
        //container.RegisterType<IRepositoryAsync<RoleMenu>, Repository<RoleMenu>>();
        //container.RegisterType<IRoleMenuService, RoleMenuService>();

        //private TmsAppContext db = new TmsAppContext();
        private readonly IRoleMenuService _roleMenuService;
        private readonly IMenuItemService _menuItemService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        private ApplicationRoleManager _roleManager;
        public RoleMenusController(IRoleMenuService roleMenuService, IUnitOfWorkAsync unitOfWork, IMenuItemService menuItemService, ApplicationRoleManager roleManager)
        {
            _roleMenuService = roleMenuService;
            _menuItemService = menuItemService;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }

        // GET: RoleMenus/Index
        public ActionResult Index()
        {

            var rolemenus = _roleMenuService.Queryable().Include(r => r.MenuItem).AsQueryable();
            var menus = _menuItemService.Queryable().Include(x => x.SubMenus).Where(x => x.IsEnabled && x.Parent == null);
            var roles = _roleManager.Roles;
            var roleview = new List<RoleView>();
            foreach (var role in roles)
            {
                var mymenus = _roleMenuService.GetByRoleName(role.Name);
                RoleView r = new RoleView();
                r.RoleName = role.Name;
                r.Count = mymenus.Count();
                roleview.Add(r);
            }
            ViewBag.Menus = menus;
            ViewBag.Roles = roleview;
            return View(rolemenus);
        }

        [HttpGet]
        public ActionResult RenderMenus()
        {
            //var roles = UserManager.GetRolesAsync(this.User.Identity.GetUserId()).Result.ToArray();
            var roles = new string[] { "admin" };
            var menus = _roleMenuService.RenderMenus(roles);
            return PartialView("_navMenuBar", menus);
        }
        public ActionResult GetMenus(string roleName)
        {

            var rolemenus = _roleMenuService.GetByRoleName(roleName);
            //var all = _roleMenuService.RenderMenus(roleName);
            return Json(rolemenus, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult Submit(RoleMenusView[] selectmenus)
        {

            _roleMenuService.UpdateMenus(selectmenus);
            _unitOfWork.SaveChanges();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        // Get :RoleMenus/PageList
        // For Index View Boostrap-Table load  data 
        [HttpGet]
        public ActionResult PageList(int offset = 0, int limit = 10, string search = "", string sort = "", string order = "")
        {
            int totalCount = 0;
            int pagenum = offset / limit + 1;
            var rolemenus = _roleMenuService.Query(new RoleMenuQuery().WithAnySearch(search)).Include(r => r.MenuItem).OrderBy(n => n.OrderBy(sort, order)).SelectPage(pagenum, limit, out totalCount);

            var rows = rolemenus.Select(n => new { MenuItemTitle = (n.MenuItem == null ? "" : n.MenuItem.Title), Id = n.Id, RoleName = n.RoleName, MenuId = n.MenuId, IsEnabled = n.IsEnabled }).ToList();
            var pagelist = new { total = totalCount, rows = rows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }


        // GET: RoleMenus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleMenu roleMenu = _roleMenuService.Find(id);
            if (roleMenu == null)
            {
                return HttpNotFound();
            }
            return View(roleMenu);
        }


        // GET: RoleMenus/Create
        public ActionResult Create()
        {
            RoleMenu roleMenu = new RoleMenu();
            //set default value
            var menuitemRepository = _unitOfWork.Repository<MenuItem>();
            ViewBag.MenuId = new SelectList(menuitemRepository.Queryable(), "Id", "Title");
            return View(roleMenu);
        }

        // POST: RoleMenus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MenuItem,Id,RoleName,MenuId,IsEnabled")] RoleMenu roleMenu)
        {
            if (ModelState.IsValid)
            {
                _roleMenuService.Insert(roleMenu);
                _unitOfWork.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has append a RoleMenu record");
                return RedirectToAction("Index");
            }

            var menuitemRepository = _unitOfWork.Repository<MenuItem>();
            ViewBag.MenuId = new SelectList(menuitemRepository.Queryable(), "Id", "Title", roleMenu.MenuId);
            if (Request.IsAjaxRequest())
            {
                var modelStateErrors = String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
                return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
            }
            DisplayErrorMessage();
            return View(roleMenu);
        }

        // GET: RoleMenus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleMenu roleMenu = _roleMenuService.Find(id);
            if (roleMenu == null)
            {
                return HttpNotFound();
            }
            var menuitemRepository = _unitOfWork.Repository<MenuItem>();
            ViewBag.MenuId = new SelectList(menuitemRepository.Queryable(), "Id", "Title", roleMenu.MenuId);
            return View(roleMenu);
        }

        // POST: RoleMenus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MenuItem,Id,RoleName,MenuId,IsEnabled")] RoleMenu roleMenu)
        {
            if (ModelState.IsValid)
            {
                roleMenu.ObjectState = ObjectState.Modified;
                _roleMenuService.Update(roleMenu);

                _unitOfWork.SaveChanges();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has update a RoleMenu record");
                return RedirectToAction("Index");
            }
            var menuitemRepository = _unitOfWork.Repository<MenuItem>();
            ViewBag.MenuId = new SelectList(menuitemRepository.Queryable(), "Id", "Title", roleMenu.MenuId);
            if (Request.IsAjaxRequest())
            {
                var modelStateErrors = String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n => n.ErrorMessage)));
                return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
            }
            DisplayErrorMessage();
            return View(roleMenu);
        }

        // GET: RoleMenus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleMenu roleMenu = _roleMenuService.Find(id);
            if (roleMenu == null)
            {
                return HttpNotFound();
            }
            return View(roleMenu);
        }

        // POST: RoleMenus/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoleMenu roleMenu = _roleMenuService.Find(id);
            _roleMenuService.Delete(roleMenu);
            _unitOfWork.SaveChanges();
            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            DisplaySuccessMessage("Has delete a RoleMenu record");
            return RedirectToAction("Index");
        }






        private void DisplaySuccessMessage(string msgText)
        {
            TempData["SuccessMessage"] = msgText;
        }

        private void DisplayErrorMessage()
        {
            TempData["ErrorMessage"] = "Save changes was unsuccessful.";
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }



    }
}
