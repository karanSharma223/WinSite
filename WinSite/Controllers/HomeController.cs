using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WinSite.Identity;
using WinSite.Models;
using WinSite.ViewModels;

namespace WinSite.Controllers
{
    public class HomeController : Controller
    {

        private Model1 db = new Model1();
        // GET: Home
        public ActionResult Index()
        {
            var appDbContext = new ApplicationDbContext();
            var userStore = new ApplicationUserStore(appDbContext);
            var userManager = new ApplicationUserManager(userStore);
            var ideas = db.Ideas.ToList();
            List<HomeIdeaViewModel> ideaModels = new List<HomeIdeaViewModel>();
            foreach (var idea in ideas)
            {
                HomeIdeaViewModel temp = new HomeIdeaViewModel();
                temp.IdeaTitle = idea.IdeaTitle;
                temp.IdeaDescription = idea.IdeaDescription;
                temp.TeamName = idea.TeamName;
                var leadId = from lead in db.Teams
                             where temp.TeamName == lead.TeamName
                             select lead.LeaderId;
                ApplicationUser user = userManager.FindById(leadId.FirstOrDefault());
                if (user != null)
                {
                    temp.TeamLeader = user.UserName;
                }
                    ;
                ideaModels.Add(temp);
            }
            return View(ideaModels);
        }
    }
}