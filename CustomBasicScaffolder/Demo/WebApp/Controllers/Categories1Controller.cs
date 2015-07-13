using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Services;
using Repository.Pattern.UnitOfWork;
using WebApp.Repositories;
using WebApp.Extensions;
namespace WebApp.Controllers
{
    public class Categories1Controller : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IUnitOfWorkAsync _unitOfWork;
        public Categories1Controller(ICategoryService categoryService, IUnitOfWorkAsync unitOfWork)
        {
            _categoryService  = categoryService;
            _unitOfWork = unitOfWork;
        }

        // GET: Categories1
        public async Task<ActionResult> Index()
        {
            return View(await _categoryService.Queryable().ToListAsync());
        }


        [HttpGet]
        public async Task<ActionResult> PageList(int offset = 0, int limit = 10, string search = "", string sort = "", string order = "")
        {
            int totalCount = 0;
            int pagenum = offset / limit + 1;
            var categories = await Task.Run<IEnumerable<Category>>(() =>
            {
                return _categoryService.Query(new CategoryQuery().WithAnySearch(search)).OrderBy(n => n.OrderBy(sort, order)).SelectPage(pagenum, limit, out totalCount);
            });

            var rows  = categories.Select(n => new { Id = n.Id, Name = n.Name }).ToList() ;
            var pagelist = new { total = totalCount, rows = rows };

            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }

        // GET: Categories1/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await _categoryService.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories1/Create
        public ActionResult Create()
        {
            Category category = new Category();
            return View(category);
        }

        // POST: Categories1/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryService.Insert(category);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Categories1/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await _categoryService.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories1/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryService.Update(category);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories1/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = await _categoryService.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Category category = await _categoryService.FindAsync(id);
            bool del=await _categoryService.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index");
        }



        // Get Detail Row By Id For Edit
        // Get : Categories/EditProduct/:id
        [HttpGet]
        public async Task<ActionResult> EditProduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var productRepository = _unitOfWork.RepositoryAsync<Product>();
            var product = await productRepository.FindAsync(id);

            var categoryRepository = _unitOfWork.RepositoryAsync<Category>();

            if (product == null)
            {
                ViewBag.CategoryId = new SelectList(await categoryRepository.Queryable().ToListAsync(), "Id", "Name");

                //return HttpNotFound();
                return PartialView("_ProductEditForm", new Product());
            }
            else
            {
                ViewBag.CategoryId = new SelectList(await categoryRepository.Queryable().ToListAsync(), "Id", "Name", product.CategoryId);

            }
            return PartialView("_ProductEditForm", product);

        }

        // Get Create Row By Id For Edit
        // Get : Categories/CreateProduct
        [HttpGet]
        public async Task<ActionResult> CreateProduct()
        {
            var categoryRepository = _unitOfWork.RepositoryAsync<Category>();
            ViewBag.CategoryId = new SelectList(await categoryRepository.Queryable().ToListAsync(), "Id", "Name");
            return PartialView("_ProductEditForm");

        }

        // Post Delete Detail Row By Id
        // Get : Categories/DeleteProduct/:id
        [HttpPost, ActionName("DeleteProduct")]
        public async Task<ActionResult> DeleteProductConfirmed(int id)
        {
            var productRepository = _unitOfWork.RepositoryAsync<Product>();
            var del = await productRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
            if (Request.IsAjaxRequest())
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            //DisplaySuccessMessage("Has delete a Order record");
            return RedirectToAction("Index");
        }



        // Get : Categories/GetProductsByCategoryId/:id
        [HttpGet]
        public ActionResult GetProductsByCategoryId(int id)
        {
            var products = _categoryService.GetProductsByCategoryId(id);
            if (Request.IsAjaxRequest())
            {
                return Json(products.Select(n => new { CategoryName = (n.Category == null ? "" : n.Category.Name), Id = n.Id, Name = n.Name, Unit = n.Unit, UnitPrice = n.UnitPrice, StockQty = n.StockQty, ConfirmDateTime = n.ConfirmDateTime, CategoryId = n.CategoryId }), JsonRequestBehavior.AllowGet);
            }
            return View(products);

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
