///<summary>
///Provides functionality to the /MenuItem/ route.
///<date> 9/26/2018 5:56:36 PM </date>
///Create By SmartCode MVC5 Scaffolder for Visual Studio
///TODO: RegisterType UnityConfig.cs
///container.RegisterType<IRepositoryAsync<MenuItem>, Repository<MenuItem>>();
///container.RegisterType<IMenuItemService, MenuItemService>();
///
///Copyright (c) 2012-2018 neo.zhu
///Dual licensed under the MIT (http://www.opensource.org/licenses/mit-license.php)
///and GPL (http://www.opensource.org/licenses/gpl-license.php) licenses.
///</summary>
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Repository.Pattern.Infrastructure;
using Repository.Pattern.UnitOfWork;
using TrackableEntities;
using WebApp.Models;
using WebApp.Repositories;
using WebApp.Services;
using Z.EntityFramework.Plus;
namespace WebApp.Controllers
{
    //[Authorize]
    public class MenuItemsController : Controller
    {
        private readonly IMenuItemService menuItemService;
        private readonly IUnitOfWorkAsync unitOfWork;
        public MenuItemsController(IMenuItemService menuItemService, IUnitOfWorkAsync unitOfWork)
        {
            this.menuItemService = menuItemService;
            this.unitOfWork = unitOfWork;
        }
        //GET: MenuItems/Index
        //[OutputCache(Duration = 360, VaryByParam = "none")]
        public ActionResult Index()
        {
            return View();
        }
        //Get :MenuItems/GetData
        //For Index View datagrid datasource url
        [HttpGet]
        public async Task<JsonResult> GetDataAsync(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
            var totalCount = 0;
            //int pagenum = offset / limit +1;
            var pagerows = (await menuItemService
                                       .Query(new MenuItemQuery().Withfilter(filters)).Include(m => m.Parent)
                                       .OrderBy(n => n.OrderBy(sort, order))
                                       .SelectPageAsync(page, rows, out totalCount))
                                       .Select(n => new
                                       {

                                           ParentTitle = (n.Parent == null ? "" : n.Parent.Title),
                                           Id = n.Id,
                                           Title = n.Title,
                                           Description = n.Description,
                                           Code = n.Code,
                                           Url = n.Url,
                                           Controller = n.Controller,
                                           Action = n.Action,
                                           IconCls = n.IconCls,
                                           IsEnabled = n.IsEnabled,
                                           ParentId = n.ParentId
                                       }).ToList();
            var pagelist = new { total = totalCount, rows = pagerows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public async Task<JsonResult> GetDataByParentIdAsync(int parentid, int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
            var totalCount = 0;
            var pagerows = (await menuItemService
                       .Query(new MenuItemQuery().ByParentIdWithfilter(parentid, filters)).Include(m => m.Parent)
                       .OrderBy(n => n.OrderBy(sort, order))
                       .SelectPageAsync(page, rows, out totalCount))
                       .Select(n => new
                       {

                           ParentTitle = (n.Parent == null ? "" : n.Parent.Title),
                           Id = n.Id,
                           Title = n.Title,
                           Description = n.Description,
                           Code = n.Code,
                           Url = n.Url,
                           Controller = n.Controller,
                           Action = n.Action,
                           IconCls = n.IconCls,
                           IsEnabled = n.IsEnabled,
                           ParentId = n.ParentId
                       }).ToList();
            var pagelist = new { total = totalCount, rows = pagerows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }
        //easyui datagrid post acceptChanges 
        [HttpPost]
        public async Task<JsonResult> SaveDataAsync(MenuItemChangeViewModel menuitems)
        {
            if (menuitems == null)
            {
                throw new ArgumentNullException(nameof(menuitems));
            }
            if (ModelState.IsValid)
            {
                if (menuitems.updated != null)
                {
                    foreach (var item in menuitems.updated)
                    {
                        menuItemService.Update(item);
                    }
                }
                if (menuitems.deleted != null)
                {
                    foreach (var item in menuitems.deleted)
                    {
                        menuItemService.Delete(item);
                    }
                }
                if (menuitems.inserted != null)
                {
                    foreach (var item in menuitems.inserted)
                    {
                        menuItemService.Insert(item);
                    }
                }
                try
                {
                    var result = await unitOfWork.SaveChangesAsync();
                    return Json(new { success = true, result = result }, JsonRequestBehavior.AllowGet);
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
                    return Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var modelStateErrors = string.Join(",", ModelState.Keys.SelectMany(key => ModelState[key].Errors.Select(n => n.ErrorMessage)));
                return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
            }

        }
        //[OutputCache(Duration = 360, VaryByParam = "none")]
        public async Task<JsonResult> GetMenuItemsAsync(string q = "")
        {
            var menuitemRepository = unitOfWork.RepositoryAsync<MenuItem>();
            var rows = await menuitemRepository
                            .Queryable()
                            .Where(n => n.Title.Contains(q))
                            .OrderBy(n => n.Title)
                            .Select(n => new { Id = n.Id, Title = n.Title })
                            .ToListAsync();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
        //GET: MenuItems/Details/:id
        public ActionResult Details(int id)
        {

            var menuItem = menuItemService.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            return View(menuItem);
        }
        //GET: MenuItems/GetItemAsync/:id
        [HttpGet]
        public async Task<JsonResult> GetItemAsync(int id)
        {
            var menuItem = await menuItemService.FindAsync(id);
            return Json(menuItem, JsonRequestBehavior.AllowGet);
        }
        //GET: MenuItems/Create
        public ActionResult Create()
        {
            var menuItem = new MenuItem();
            //set default value
            var menuitemRepository = unitOfWork.RepositoryAsync<MenuItem>();
            ViewBag.ParentId = new SelectList(menuitemRepository.Queryable().OrderBy(n => n.Title), "Id", "Title");
            return View(menuItem);
        }
        //POST: MenuItems/Create
        //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(MenuItem menuItem)
        {
            if (menuItem == null)
            {
                throw new ArgumentNullException(nameof(menuItem));
            }
            if (ModelState.IsValid)
            {
                menuItem.TrackingState = TrackingState.Added;
                foreach (var item in menuItem.SubMenus)
                {
                    item.ParentId = menuItem.Id;
                    item.TrackingState = TrackingState.Added;
                }
                menuItemService.ApplyChanges(menuItem);
                try
                {
                    var result = await unitOfWork.SaveChangesAsync();
                    return Json(new { success = true, result = result }, JsonRequestBehavior.AllowGet);
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
                    return Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
                }
                //DisplaySuccessMessage("Has update a menuItem record");
            }
            else
            {
                var modelStateErrors = string.Join(",", ModelState.Keys.SelectMany(key => ModelState[key].Errors.Select(n => n.ErrorMessage)));
                return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
                //DisplayErrorMessage(modelStateErrors);
            }
            //var menuitemRepository = this.unitOfWork.RepositoryAsync<MenuItem>();
            //ViewBag.ParentId = new SelectList(await menuitemRepository.Queryable().OrderBy(n=>n.Title).ToListAsync(), "Id", "Title", menuItem.ParentId);
            //return View(menuItem);
        }
        //GET: MenuItems/PopupEdit/:id
        //[OutputCache(Duration = 360, VaryByParam = "id")]
        public async Task<JsonResult> PopupEditAsync(int id)
        {

            var menuItem = await menuItemService.FindAsync(id);
            return Json(menuItem, JsonRequestBehavior.AllowGet);
        }

        //GET: MenuItems/Edit/:id
        public ActionResult Edit(int id)
        {
            var menuItem = menuItemService.Find(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            var menuitemRepository = unitOfWork.RepositoryAsync<MenuItem>();
            ViewBag.ParentId = new SelectList(menuitemRepository.Queryable().OrderBy(n => n.Title), "Id", "Title", menuItem.ParentId);
            return View(menuItem);
        }
        //POST: MenuItems/Edit/:id
        //To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(MenuItem menuItem)
        {
            if (menuItem == null)
            {
                throw new ArgumentNullException(nameof(menuItem));
            }
            if (ModelState.IsValid)
            {
                menuItem.TrackingState = TrackingState.Modified;
                foreach (var item in menuItem.SubMenus)
                {
                    item.ParentId = menuItem.Id;
                    //set ObjectState with conditions
                    if (item.Id <= 0)
                        item.TrackingState = TrackingState.Added;
                    else
                        item.TrackingState = TrackingState.Modified;
                }

                menuItemService.ApplyChanges(menuItem);
                try
                {
                    var result = await unitOfWork.SaveChangesAsync();
                    return Json(new { success = true, result = result }, JsonRequestBehavior.AllowGet);
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
                    return Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
                }

                //DisplaySuccessMessage("Has update a MenuItem record");
                //return RedirectToAction("Index");
            }
            else
            {
                var modelStateErrors = string.Join(",", ModelState.Keys.SelectMany(key => ModelState[key].Errors.Select(n => n.ErrorMessage)));
                return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
                //DisplayErrorMessage(modelStateErrors);
            }
            //var menuitemRepository = this.unitOfWork.RepositoryAsync<MenuItem>();
            //return View(menuItem);
        }
        //GET: MenuItems/Delete/:id
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var menuItem = await menuItemService.FindAsync(id);
            if (menuItem == null)
            {
                return HttpNotFound();
            }
            return View(menuItem);
        }
        //POST: MenuItems/Delete/:id
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var menuItem = await menuItemService.FindAsync(id);
            menuItemService.Delete(menuItem);
            var result = await unitOfWork.SaveChangesAsync();
            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true, result = result }, JsonRequestBehavior.AllowGet);
            }
            DisplaySuccessMessage("Has delete a MenuItem record");
            return RedirectToAction("Index");
        }
        //Get Detail Row By Id For Edit
        //Get : MenuItems/EditMenuItem/:id
        [HttpGet]
        public async Task<ActionResult> EditMenuItem(int id)
        {
            var menuitemRepository = unitOfWork.RepositoryAsync<MenuItem>();
            var menuitem = await menuitemRepository.FindAsync(id);
       
            if (menuitem == null)
            {
                ViewBag.ParentId = new SelectList(await menuitemRepository.Queryable().OrderBy(n => n.Title).ToListAsync(), "Id", "Title");
                //return HttpNotFound();
                return PartialView("_MenuItemEditForm", new MenuItem());
            }
            else
            {
                ViewBag.ParentId = new SelectList(await menuitemRepository.Queryable().ToListAsync(), "Id", "Title", menuitem.ParentId);
            }
            return PartialView("_MenuItemEditForm", menuitem);
        }
        //Get Create Row By Id For Edit
        //Get : MenuItems/CreateMenuItem
        [HttpGet]
        public async Task<ActionResult> CreateMenuItemAsync()
        {
            var menuitemRepository = unitOfWork.RepositoryAsync<MenuItem>();
            ViewBag.ParentId = new SelectList(await menuitemRepository.Queryable().OrderBy(n => n.Title).ToListAsync(), "Id", "Title");
            return PartialView("_MenuItemEditForm");
        }
        //Post Delete Detail Row By Id
        //Get : MenuItems/DeleteMenuItem/:id
        [HttpPost, ActionName("DeleteMenuItem")]
        public async Task<ActionResult> DeleteMenuItemConfirmedAsync(int id)
        {
            var menuitemRepository = unitOfWork.RepositoryAsync<MenuItem>();
            menuitemRepository.Delete(id);
            var result = await unitOfWork.SaveChangesAsync();
            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true, result = result }, JsonRequestBehavior.AllowGet);
            }
            DisplaySuccessMessage("Has delete a Order record");
            return RedirectToAction("Index");
        }

        //Get : MenuItems/GetSubMenusByParentId/:id
        [HttpGet]
        public async Task<JsonResult> GetSubMenusByParentIdAsync(int id)
        {
            var submenus = menuItemService.GetSubMenusByParentId(id);
            var data = await submenus.AsQueryable().ToListAsync();
            var rows = data.Select(n => new
            {

                ParentTitle = (n.Parent == null ? "" : n.Parent.Title),
                Id = n.Id,
                Title = n.Title,
                Description = n.Description,
                Code = n.Code,
                Url = n.Url,
                Controller = n.Controller,
                Action = n.Action,
                IconCls = n.IconCls,
                IsEnabled = n.IsEnabled,
                ParentId = n.ParentId
            });
            return Json(rows, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult CreateWithController()
        {
            try
            {
                this.menuItemService.CreateWithController();
                this.unitOfWork.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                var errormessage = string.Join(",", e.EntityValidationErrors.Select(x => x.ValidationErrors.FirstOrDefault()?.PropertyName + ":" + x.ValidationErrors.FirstOrDefault()?.ErrorMessage));
                return Json(new { success = false, err = errormessage }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, err = e.GetBaseException().Message }, JsonRequestBehavior.AllowGet);
            }
        }

        //删除选中的记录
        [HttpPost]
        public async Task<JsonResult> DeleteCheckedAsync(int[] id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            await menuItemService.Queryable().Where(x => id.Contains(x.Id)).DeleteAsync();
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);

        }
        //导出Excel
        [HttpPost]
        public ActionResult ExportExcel(string filterRules = "", string sort = "Id", string order = "asc")
        {
            var fileName = "menuitems_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            var stream = menuItemService.ExportExcel(filterRules, sort, order);
            return File(stream, "application/vnd.ms-excel", fileName);
        }
        private void DisplaySuccessMessage(string msgText) => TempData["SuccessMessage"] = msgText;
        private void DisplayErrorMessage(string msgText) => TempData["ErrorMessage"] = msgText;

    }
}
