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
    public class AdminController : Controller
    {
        private Model1 db = new Model1();

        // GET: Admin
        public ActionResult Index()
        {
            var appDbContext = new ApplicationDbContext();
            var userStore = new ApplicationUserStore(appDbContext);
            var userManager = new ApplicationUserManager(userStore);
            int userCount = userManager.Users.Count();

            DashboardViewModel dashboardViewModel = new DashboardViewModel();
            dashboardViewModel.ideaCount = db.Ideas.Count();
            dashboardViewModel.teamCount = db.Teams.Count();
            dashboardViewModel.memberCount = userCount-1;
            
            return View(dashboardViewModel);
        }

        public ActionResult Teams()
        {
            var appDbContext = new ApplicationDbContext();
            var userStore = new ApplicationUserStore(appDbContext);
            var userManager = new ApplicationUserManager(userStore);
            var teams = db.Teams.Select(x => x.TeamName).ToList();
            List<TeamModelView> teamModels = new List<TeamModelView>();
            foreach (string team in teams)
            {
                //Team Name
                TeamModelView teamModel = new TeamModelView();
                teamModel.TeamName = team;

                //Idea Name
                string ideaName = (from idea in db.Ideas
                                   where idea.TeamName == team
                                   select idea.TeamName).FirstOrDefault();

                teamModel.IdeaName = ideaName;

                //Team Members

                List<string> allTeamMembers = new List<string>();
                List<string> allTeamMembersEmail = new List<string>();
                List<string> allTeamMemberIds = (from member in db.TeamMembers
                                                 where member.TeamName == team
                                                 select member.TeamMates).ToList();
                foreach (var iter in allTeamMemberIds)
                {
                    ApplicationUser mate = userManager.FindById(iter);
                    allTeamMembers.Add(mate.UserName);
                    allTeamMembersEmail.Add(mate.Email);
                }

                teamModel.AllEmails = allTeamMembersEmail;
                teamModel.Members = allTeamMembers;

                //Team Leaders
                var leaderID = (from t in db.Teams
                                where t.TeamName == team
                                select t.LeaderId).FirstOrDefault();
                teamModel.LeaderName = userManager.FindById(leaderID).UserName;

                teamModels.Add(teamModel);

            }

            return View(teamModels);
        }

        public ActionResult Ideas()
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
                temp.TeamLeader = user.UserName;
                ideaModels.Add(temp);
            }
            return View(ideaModels);
        }

        public ActionResult Hackathons()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Hackathons(HackathonViewModel hack)
        {
            Hackathon temp = new Hackathon();
            temp.HackathonName = hack.HackathonName;
            temp.StartDate = hack.StartDate;
            temp.EndDate = hack.EndDate;
            temp.Teamsize = hack.Teamsize;
            db.Hackathons.Add(temp);
            return RedirectToAction("Hackathons", "Admin");
        }
    }
}