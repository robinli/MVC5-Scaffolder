using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Repository.Pattern.UnitOfWork;

namespace WebApp.Controllers
{
    public class ManagementController : Controller
    {
        private ApplicationUserManager userManager;
        private ApplicationRoleManager roleManager;
        private readonly IUnitOfWorkAsync _unitOfWork;
        public ApplicationUserManager UserManager
        {
            get
            {
                return userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                userManager = value;
            }
        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                roleManager = value;
            }
        }
        
        public ManagementController(
            IUnitOfWorkAsync _unitOfWork,
        ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            this._unitOfWork = _unitOfWork;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        protected override void Dispose(bool disposing)
        {
            //if (disposing && userManager != null)
            //{
            //    userManager.Dispose();
            //    userManager = null;
            //}
            //if (disposing && roleManager != null)
            //{
            //    roleManager.Dispose();
            //    roleManager = null;
            //}
            base.Dispose(disposing);
        }
        private ActionResult _Index()
        {
            //set default value
            var companyRepository = _unitOfWork.RepositoryAsync<Company>();
            ViewBag.CompanySelectList = companyRepository.Queryable().Select(r => new SelectListItem { Text = r.Name, Value = r.Id.ToString() }).ToArray();

            ViewBag.RoleSelectList = RoleManager.Roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToArray();
            try
            {
                ViewBag.MaxRolesCount = UserManager.Users.Max(u => u.Roles.Count);
            }
            catch(InvalidOperationException ex)
            {
                ViewBag.MaxRolesCount = 0;
                Console.WriteLine(ex);
            }
            return View("Index", new ManagementViewModel
            {
                Users = UserManager.Users.ToList(),
                Roles = RoleManager.Roles.ToList()
            });
        }
        public ActionResult Index()
        {
            if (RoleManager.Roles.Count() == 0)
            {
                RoleManager.Create(new ApplicationRole() { Name = "admin" });

            }
            
            return _Index();
        }

         

        [HttpPost]
        [Route("management/roles")]
        public async Task<ActionResult> AddRole(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await RoleManager.CreateAsync(new ApplicationRole { Name = model.Name });
                if (result.Succeeded)
                {
                    //RedirectToAction("Index");
                    return _Index();
                }
                else
                {
                    AddErrors(result);
                }
            }
            return _Index();
        }
        [HttpGet]
        [Route("management/roles/{id}")]
        public async Task<ActionResult> DeleteRole(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                await RoleManager.DeleteAsync(role);
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Button("Attach")]
        [Route("management/account/roles")]
        public async Task<ActionResult> AttachRole(AttachRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await UserManager.AddToRoleAsync(model.UserId, model.RoleName);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrors(result);
                }
            }
            return _Index();
        }
        [HttpPost]
        [Button("Purge")]
        [Route("management/account/roles")]
        public async Task<ActionResult> PurgeRole(AttachRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await UserManager.RemoveFromRoleAsync(model.UserId, model.RoleName);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrors(result);
                }
            }
            return _Index();
        }
        [HttpPost]
        [Route("management/account")]
        public async Task<ActionResult> AddAccount(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var companyRepository = _unitOfWork.RepositoryAsync<Company>();
                var companyid =Convert.ToInt32( model.CompanyCode);
                var companyName = companyRepository.Queryable().Where(x => x.Id == companyid).First().Name;
                var user = new ApplicationUser() { UserName = model.UserName, Email = model.Email,CompanyCode=model.CompanyCode,CompanyName= companyName };
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    result = await UserManager.AddToRoleAsync(user.Id, model.Role);
                    //return RedirectToAction("Index");
                }
                else
                {
                    AddErrors(result);
                }
            }
            return _Index();
        }
        [HttpGet]
        [Route("management/account/{id}")]
        public async Task<ActionResult> DeleteAccount(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                await UserManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}