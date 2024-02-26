using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using WinSite.Identity;

[assembly: OwinStartup(typeof(WinSite.Startup))]

namespace WinSite
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the application for cookie-based authentication
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                LogoutPath = new PathString("/Account/Logout"), // Ensure logout path is set
                ExpireTimeSpan = TimeSpan.FromMinutes(30), // Set expiration time for the cookie
                SlidingExpiration = true // Set sliding expiration to renew the cookie expiration period on each request
            });

            // Create roles and users
            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            // Create a role manager and user manager
            var dbContext = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dbContext));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dbContext));

            // Create Admin Role if it doesn't exist
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole("Admin");
                roleManager.Create(role);
            }

            // Create Admin User if it doesn't exist
            if (userManager.FindByName("admin") == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@gmail.com",
                };

                string userPassword = "admin123";
                var result = userManager.Create(user, userPassword);

                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }
            }

            // Create Customer Role if it doesn't exist
            if (!roleManager.RoleExists("Customer"))
            {
                var role = new IdentityRole("Customer");
                roleManager.Create(role);
            }
        }
    }
}