


using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;
using Repository.Pattern.Infrastructure;
using WebApp.Models;
using WebApp.Services;
using WebApp.Repositories;
 


namespace WebApp.Controllers
{
    public class CompaniesController : Controller
    {
        
        //Please RegisterType UnityConfig.cs
        //container.RegisterType<IRepositoryAsync<Company>, Repository<Company>>();
        //container.RegisterType<ICompanyService, CompanyService>();
        
        //private StoreContext db = new StoreContext();
        private readonly ICompanyService  _companyService;
        private readonly IUnitOfWorkAsync _unitOfWork;

        public CompaniesController (ICompanyService  companyService, IUnitOfWorkAsync unitOfWork)
        {
            _companyService  = companyService;
            _unitOfWork = unitOfWork;
        }

        // GET: Companies/Index
        public async Task<ActionResult> Index()
        {
            
            var companies  = _companyService.Queryable();
            return View(await companies.ToListAsync()  );
			
        }

        // Get :Companies/PageList
        // For Index View Boostrap-Table load  data 
        [HttpGet]
				 public async Task<ActionResult> GetData(int page = 1, int rows = 10, string sort = "Id", string order = "asc", string filterRules = "")
				{
			var filters = JsonConvert.DeserializeObject<IEnumerable<filterRule>>(filterRules);
            int totalCount = 0;
            //int pagenum = offset / limit +1;
											var companies  = await  _companyService
						.Query(new CompanyQuery().Withfilter(filters))
							.OrderBy(n=>n.OrderBy(sort,order))
							.SelectPage(page, rows, out totalCount)
							.AsQueryable()
							.ToListAsync();
				
			            var datarows = companies .Select(  n => new {  Id = n.Id , Name = n.Name , Address = n.Address , City = n.City , Province = n.Province , RegisterDate = n.RegisterDate , Employees = n.Employees }).ToList();
            var pagelist = new { total = totalCount, rows = datarows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }

		[HttpPost]
				public async Task<ActionResult> SaveData(CompanyChangeViewModel companies)
        {
            if (companies.updated != null)
            {
                foreach (var updated in companies.updated)
                {
                    _companyService.Update(updated);
                }
            }
            if (companies.deleted != null)
            {
                foreach (var deleted in companies.deleted)
                {
                    _companyService.Delete(deleted);
                }
            }
            if (companies.inserted != null)
            {
                foreach (var inserted in companies.inserted)
                {
                    _companyService.Insert(inserted);
                }
            }
            await _unitOfWork.SaveChangesAsync();

            return Json(new {Success=true}, JsonRequestBehavior.AllowGet);
        }
		
		
		
       
        // GET: Companies/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var  company = await _companyService.FindAsync(id);

            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }
        

        // GET: Companies/Create
		        public ActionResult Create()
		        {
            var company = new Company();
            //set default value
            return View(company);
        }

        // POST: Companies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Departments,Employee,Id,Name,Address,City,Province,RegisterDate,Employees")] Company company)
        {
            if (ModelState.IsValid)
            {
             				_companyService.Insert(company);
                           await _unitOfWork.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has append a Company record");
                return RedirectToAction("Index");
            }
			else {
			 var modelStateErrors =String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
             if (Request.IsAjaxRequest())
             {
               return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
             }
             DisplayErrorMessage(modelStateErrors);
			}
			
            return View(company);
        }

        // GET: Companies/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var company = await _companyService.FindAsync(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Departments,Employee,Id,Name,Address,City,Province,RegisterDate,Employees")] Company company)
        {
            if (ModelState.IsValid)
            {
                company.ObjectState = ObjectState.Modified;
                				_companyService.Update(company);
                                
                await   _unitOfWork.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
                DisplaySuccessMessage("Has update a Company record");
                return RedirectToAction("Index");
            }
			else {
			var modelStateErrors =String.Join("", this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors.Select(n=>n.ErrorMessage)));
            if (Request.IsAjaxRequest())
            {
                return Json(new { success = false, err = modelStateErrors }, JsonRequestBehavior.AllowGet);
            }
            DisplayErrorMessage(modelStateErrors);
			}
			
            return View(company);
        }

        // GET: Companies/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var company = await _companyService.FindAsync(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var company = await  _companyService.FindAsync(id);
             _companyService.Delete(company);
            await _unitOfWork.SaveChangesAsync();
           if (Request.IsAjaxRequest())
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            DisplaySuccessMessage("Has delete a Company record");
            return RedirectToAction("Index");
        }


       

 

		//导出Excel
		[HttpPost]
        public ActionResult ExportExcel( string filterRules = "",string sort = "Id", string order = "asc")
        {
            var fileName = "companies_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            var stream=  _companyService.ExportExcel(filterRules,sort, order );
            return File(stream, "application/vnd.ms-excel", fileName);
      
        }
		


        private void DisplaySuccessMessage(string msgText)
        {
            TempData["SuccessMessage"] = msgText;
        }

        private void DisplayErrorMessage(string msgText)
        {
            TempData["ErrorMessage"] = msgText;
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
