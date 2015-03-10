using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebApp.Models;
using WebApp.Extensions;
namespace WebApp.Controllers
{
    [Authorize]
    public class RoleManagerController : Controller
    {
        private ApplicationRoleManager _roleManager;

        public RoleManagerController()
        {
        }

        public RoleManagerController(ApplicationRoleManager roleManager)
        {
            RoleManager = roleManager;

        }
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager;
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ActionResult Index()
        {
            return View();
        }
        // Get :RoleManager/PageList
        // For Index View Boostrap-Table load  data 
        [HttpGet]
        public ActionResult PageList(int offset = 0, int limit = 10, string search = "", string sort = "", string order = "")
        {
            int totalCount = 0;
            int pagenum = offset / limit + 1;

            var roles = _roleManager.Roles.Where(n => n.Name.Contains(search)).OrderByName(sort, order);
            totalCount = roles.Count();
            var datalist = roles.Skip(offset).Take(limit);
            var rows = datalist.Select(n => new { Id = n.Id, Name = n.Name }).ToList();
            var pagelist = new { total = totalCount, rows = rows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(ApplicationRole role)
        {
            if (ModelState.IsValid)
            {

                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "RoleManager");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(role);
        }
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {

            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return View("Error");
            }

            return View(role);

        }

        [HttpPost]
        public async Task<ActionResult> Edit(ApplicationRole role)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "AccountManager");
                }
                AddErrors(result);
            }
            return View(role);

        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(id);
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    if (Request.IsAjaxRequest())
                    {
                        return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                    }
                    return RedirectToAction("Index", "AccountManager");
                }
                AddErrors(result);
            }
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
    }
    
}