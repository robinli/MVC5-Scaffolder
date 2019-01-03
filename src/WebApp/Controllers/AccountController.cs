#region Using

using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

using WebApp.Models;
using WebApp.Services;

#endregion

namespace WebApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // TODO: This should be moved to the constructor of the controller in combination with a DependencyResolver setup
        // NOTE: You can use NuGet to find a strategy for the various IoC packages out there (i.e. StructureMap.MVC5)
        //private readonly UserManager _manager = UserManager.Create();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }
        private ApplicationSignInManager _signInManager;

        public ApplicationSignInManager SignInManager
        {
            get => _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            private set => _signInManager = value;
        }
        private readonly ICompanyService _companyService;
        public AccountController(ICompanyService companyService )
        {
            //UserManager = userManager;
            //SignInManager = signInManager;
            _companyService = companyService;
        }

        // GET: /account/forgotpassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            // We do not want to use any existing identity information
            EnsureLoggedOut();

            return View();
        }

        // GET: /account/login
        [AllowAnonymous]
        
        public ActionResult Login(string returnUrl)
        {
            // We do not want to use any existing identity information
            EnsureLoggedOut();

            // Store the originating URL so we can attach it to a form field

            return View(new AccountLoginModel { ReturnUrl = returnUrl });
        }

        // POST: /account/login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(AccountLoginModel viewModel)
        {
            // Ensure we have a valid viewModel to work with
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // Verify if a user exists with the provided identity information

            // If a user was found
            if (await UserManager.FindByEmailAsync(viewModel.Email) != null)
            {
                switch (await SignInManager.PasswordSignInAsync((await UserManager.FindByEmailAsync(viewModel.Email)).UserName, viewModel.Password, viewModel.RememberMe, shouldLockout: false))
                {
                    case SignInStatus.Success:
                        // Then create an identity for it and sign it in
                        await SignInAsync(await UserManager.FindByEmailAsync(viewModel.Email), viewModel.RememberMe);
                        // If the user came from a specific page, redirect back to it
                        return RedirectToLocal(viewModel.ReturnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(viewModel);
                }
                
            }

            // No existing user was found that matched the given criteria
            ModelState.AddModelError("", "Invalid username or password.");

            // If we got this far, something failed, redisplay form
            return View(viewModel);
        }

        // GET: /account/error
        [AllowAnonymous]
        public ActionResult Error()
        {
            // We do not want to use any existing identity information
            EnsureLoggedOut();

            return View();
        }

        // GET: /account/register
        [AllowAnonymous]

        public ActionResult Register()
        {
            // We do not want to use any existing identity information
            EnsureLoggedOut();
            var data = _companyService.Queryable().Select(x => new ListItem() { Value = x.Id.ToString(), Text = x.Name });

            ViewBag.companylist = data;
            var model = new AccountRegistrationModel
            {
                CompanyCode = data.FirstOrDefault() != null ? data.FirstOrDefault().Value : "",
                CompanyName = data.FirstOrDefault() != null ? data.FirstOrDefault().Text : ""
            };

            return View(model);
        }

        // POST: /account/register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(AccountRegistrationModel viewModel)
        {
            var data = this._companyService.Queryable().Select(x => new ListItem() { Value = x.Id.ToString(), Text = x.Name });
            ViewBag.companylist = data;

            // Ensure we have a valid viewModel to work with
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }


            // Try to create a user with the given identity
            try
            {

                // Prepare the identity with the provided information
                var user = new ApplicationUser
                {
                    UserName = viewModel.Username,
                    FullName = viewModel.Lastname + "." + viewModel.Firstname,
                    CompanyCode = viewModel.CompanyCode,
                    CompanyName = viewModel.CompanyName,
                    Email = viewModel.Email,
                    AccountType = 0
                };
                var result = await UserManager.CreateAsync(user, viewModel.Password);

                // If the user could not be created
                if (!result.Succeeded)
                {
                    // Add all errors to the page so they can be used to display what went wrong
                    AddErrors(result);

                    return View(viewModel);
                }

                // If the user was able to be created we can sign it in immediately
                // Note: Consider using the email verification proces
                await SignInAsync(user, true);

                return RedirectToLocal();
            }
            catch (DbEntityValidationException ex)
            {
                // Add all errors to the page so they can be used to display what went wrong
                AddErrors(ex);

                return View(viewModel);
            }
        }

        // POST: /account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            // First we clean the authentication ticket like always
            FormsAuthentication.SignOut();
            this.SignInManager.SignOut();
            // Second we clear the principal to ensure the user does not retain any authentication
            HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);

            // Last we redirect to a controller/action that requires authentication to ensure a redirect takes place
            // this clears the Request.IsAuthenticated flag since this triggers a new request
            return RedirectToLocal();
        }
        public new ActionResult Profile() {
            return View();
        }
        private ActionResult RedirectToLocal(string returnUrl = "")
        {
            // If the return url starts with a slash "/" we assume it belongs to our site
            // so we will redirect to this "action"
            if (!returnUrl.IsNullOrWhiteSpace() && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            // If we cannot verify if the url is local to our host we redirect to a default location
            return RedirectToAction("index", "home");
        }

        private void AddErrors(DbEntityValidationException exc)
        {
            foreach (var error in exc.EntityValidationErrors.SelectMany(validationErrors => validationErrors.ValidationErrors.Select(validationError => validationError.ErrorMessage)))
            {
                ModelState.AddModelError("", error);
            }
        }

        private void AddErrors(IdentityResult result)
        {
            // Add all errors that were returned to the page error collection
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        
        private void EnsureLoggedOut()
        {
            // If the request is (still) marked as authenticated we send the user to the logout action
            if (Request.IsAuthenticated)
                Logout();
        }
        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            // Clear any lingering authencation data
            //FormsAuthentication.SignOut();
            //this.SignInManager.SignOut();

            // Create a claims based identity for the current user

            // Write the authentication cookie
            FormsAuthentication.SetAuthCookie((await this.SignInManager.CreateUserIdentityAsync(user)).Name, isPersistent);
        }
        
     

        // GET: /account/lock
        [AllowAnonymous]
        public ActionResult Lock()
        {
            return View();
        }
    }
}