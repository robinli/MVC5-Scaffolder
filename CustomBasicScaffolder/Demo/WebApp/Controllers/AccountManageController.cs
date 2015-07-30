
 

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
    public class AccountManageController : Controller
    {
        private ApplicationUserManager _userManager;

        public AccountManageController()
        {
        }

        public AccountManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set { _signInManager = value; }
        }
        public ActionResult Index()
        {
            return View();
        }
        // Get :AccountManager/PageList
        // For Index View Boostrap-Table load  data 
        [HttpGet]
        public ActionResult PageList(int offset = 0, int limit = 10, string search = "", string sort = "", string order = "")
        {
            int totalCount = 0;
            int pagenum = offset / limit + 1;

            var users = _userManager.Users.Where(n => n.UserName.Contains(search) || n.Email.Contains(search) || n.PhoneNumber.Contains(search)).OrderByName(sort, order);
            totalCount = users.Count();
            var datalist = users.Skip(offset).Take(limit);
            var rows = datalist.Select(n => new { Id = n.Id, UserName = n.UserName, Email = n.Email, PhoneNumber = n.PhoneNumber, AccessFailedCount = n.AccessFailedCount, LockoutEnabled = n.LockoutEnabled, LockoutEndDateUtc = n.LockoutEndDateUtc }).ToList();
            var pagelist = new { total = totalCount, rows = rows };
            return Json(pagelist, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "AccountManager");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        [HttpGet]
        public async Task<ActionResult> Edit(string id)
        {

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return View("Error");
            }

            return View(user);

        }

        [HttpPost]
        public async Task<ActionResult> Edit(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var item = await UserManager.FindByIdAsync(user.Id);
                item.UserName = user.UserName;
                item.PhoneNumber = user.PhoneNumber;
                item.Email = user.Email;
                var result = await UserManager.UpdateAsync(item);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "AccountManager");
                }
                AddErrors(result);
            }
            return View(user);

        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(id);
                var result = await UserManager.DeleteAsync(user);
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