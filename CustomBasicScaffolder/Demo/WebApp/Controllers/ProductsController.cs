


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
using PagedList;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace WebApp.Controllers
{
    public class ProductsController : Controller
    {
        //private StoreContext db = new StoreContext();
        private readonly IProductService  _productService;
        
        private readonly IUnitOfWorkAsync _unitOfWork;

        public ProductsController (IProductService  productService, IUnitOfWorkAsync unitOfWork)
        {
            _productService  = productService;
            _unitOfWork = unitOfWork;
        }

        // GET: Products/Index
        public ActionResult Index()
        {
            
            var products  = _productService.Queryable().Include(p => p.Category).AsQueryable();
            
             return View(products);
        }
        public ActionResult GetCategories()
        {
            var categoryRepository = _unitOfWork.Repository<Category>();
            var data = categoryRepository.Queryable().ToList();
            var rows = data.Select(n => new { Id = n.Id, Name = n.Name });
            return Json(rows, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
        {
            var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);

            int totalCount = 0;
            //int pagenum = offset / limit + 1;
            var product = _productService.Query(new ProductQuery().Withfilter(filters)).Include(p => p.Category).OrderBy(n => n.OrderBy(sort, order)).SelectPage(page, rows, out totalCount);

            var data = product.Select(n => new { CategoryName = n.Category.Name,CategoryId = n.CategoryId, Id = n.Id, Name = n.Name, Unit = n.Unit, UnitPrice = n.UnitPrice, StockQty = n.StockQty, ConfirmDateTime = n.ConfirmDateTime }).ToList();
            var pagelist = new { total = totalCount, rows = data };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Save(ProductChangeModel products)
        {
            if (products.updated != null)
            {
                foreach (var updated in products.updated)
                {
                    _productService.Update(updated);
                }
            }
            if (products.deleted != null)
            {
                foreach (var deleted in products.deleted)
                {
                    _productService.Delete(deleted);
                }
            }
            if (products.inserted != null)
            {
                foreach (var inserted in products.inserted)
                {
                    _productService.Insert(inserted);
                }
            }
            _unitOfWork.SaveChanges();
            //products = new Dictionary<string, List<Product>>();
            //var list= new List<Product>();
            //list.Add(new Product(){ Id=1, Name="Name"});


            //products.Add("updated", list);
            //products.Add("deleted", list);
            //products.Add("create", list);
            //JavaScriptSerializer js = new JavaScriptSerializer();

            //var str = js.Serialize(products);
            Console.Write(products);
            return Json(new {Success=true}, JsonRequestBehavior.AllowGet);
        }
        
         

       
        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _productService.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        

        // GET: Products/Create
        public ActionResult Create()
        {
            var categoryRepository = _unitOfWork.Repository<Category>();
            ViewBag.CategoryId = new SelectList(categoryRepository.Queryable(), "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Category,Id,Name,Unit,UnitPrice,StockQty,ConfirmDateTime,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
             	_productService.Insert(product);
                _unitOfWork.SaveChanges();
                DisplaySuccessMessage("Has append a Product record");
                return RedirectToAction("Index");
            }

            var categoryRepository = _unitOfWork.Repository<Category>();
            ViewBag.CategoryId = new SelectList(categoryRepository.Queryable(), "Id", "Name", product.CategoryId);
            DisplayErrorMessage();
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _productService.Find(id);

 


            if (product == null)
            {
                return HttpNotFound();
            }
            var categoryRepository = _unitOfWork.Repository<Category>();
            ViewBag.CategoryId = new SelectList(categoryRepository.Queryable(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Category,Id,Name,Unit,UnitPrice,StockQty,ConfirmDateTime,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.ObjectState = ObjectState.Modified;
                				_productService.Update(product);
                                
                _unitOfWork.SaveChanges();
                DisplaySuccessMessage("Has update a Product record");
                return RedirectToAction("Index");
            }
            var categoryRepository = _unitOfWork.Repository<Category>();
            ViewBag.CategoryId = new SelectList(categoryRepository.Queryable(), "Id", "Name", product.CategoryId);
            DisplayErrorMessage();
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _productService.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product =  _productService.Find(id);
             _productService.Delete(product);
            _unitOfWork.SaveChanges();
            DisplaySuccessMessage("Has delete a Product record");
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
                //_unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    public class ProductChangeModel
    {
        public IEnumerable<Product> inserted { get; set; }
        public IEnumerable<Product> deleted { get; set; }
        public IEnumerable<Product> updated { get; set; }
    }
}
