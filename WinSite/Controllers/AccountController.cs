using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using WinSite.Identity;
using WinSite.Models;
using WinSite.ViewModels; // Import the namespace containing RegisterViewModel

namespace WinSite.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        public ActionResult Register(RegisterViewModel rvm)
        {
            // Check if the model is valid
            if (ModelState.IsValid)
            {
                var appDbContext = new ApplicationDbContext();
                var userStore = new ApplicationUserStore(appDbContext);
                var userManager = new ApplicationUserManager(userStore);
                var passwordHash = Crypto.HashPassword(rvm.Password);
                var user = new ApplicationUser()
                {
                    Email = rvm.Email,
                    UserName = rvm.Username,
                    PasswordHash = passwordHash
                };

                IdentityResult result = userManager.Create(user);
                if (result.Succeeded)
                {
                    if (!userManager.IsInRole(user.Id, "Customer"))
                    {
                        userManager.AddToRole(user.Id, "Customer");
                    }

                    //login
                    var authenticationManager = HttpContext.GetOwinContext().Authentication;
                    var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    authenticationManager.SignIn(new AuthenticationProperties(), userIdentity);
                    
                }
                return RedirectToAction("Index", "User");
            }

            else
            {
                return View(rvm);
            }
            
        }

        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel lvm)
        {
            var appDbContext = new ApplicationDbContext();
            var userStore = new ApplicationUserStore(appDbContext);
            var userManager = new ApplicationUserManager(userStore);
            var user = userManager.FindByName(lvm.Username);

            if (user != null)
            {
                var passwordHasher = new PasswordHasher();
                var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user.PasswordHash, lvm.Password);

                if (passwordVerificationResult == PasswordVerificationResult.Success)
                {
                    var authenticationManager = HttpContext.GetOwinContext().Authentication;
                    var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);
                    authenticationManager.SignIn(new AuthenticationProperties(), userIdentity);

                    // Check if the user is in the "Admin" role
                    if (userManager.IsInRole(user.Id, "Admin"))
                    {
                        return RedirectToAction("Index","Admin");
                    }
                    return RedirectToAction("Index", "User");
                }
            }

            ModelState.AddModelError("myerror", "Invalid username or password");
            return View(lvm);
        }

        public ActionResult Logout()
        {
            var temp = User.Identity.IsAuthenticated;
            var authenticationManager = HttpContext.GetOwinContext().Authentication;
            authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
    }
}