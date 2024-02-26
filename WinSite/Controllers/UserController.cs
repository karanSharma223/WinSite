using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using WinSite.Identity;
using WinSite.Models;
using WinSite.ViewModels;

namespace WinSite.Controllers
{
    public class UserController : Controller
    {
        private Model1 db = new Model1();
        int limit = 6;
        private List<string> GetTeamsPartOf(string uid)
        {
            var q = from tm in db.TeamMembers
                    where uid == tm.TeamMates
                    select tm.TeamName;
            List<string> temp = q.ToList();
            return temp;
        }


        [Authorize]
        // GET: User
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var appDbContext = new ApplicationDbContext();
                var userStore = new ApplicationUserStore(appDbContext);
                var userManager = new ApplicationUserManager(userStore);
                string uid = User.Identity.GetUserId();
                //fetching ideas to view

                var temp = GetTeamsPartOf(uid);

                var ideasUserNotPartOf = db.Ideas
                .Where(idea => !temp.Contains(idea.TeamName))
                .ToList();

                ViewBag.Ideas = ideasUserNotPartOf;

                List<string> TeamLeads = new List<string>();
                foreach (var ide in ideasUserNotPartOf)
                {
                    var qwery = from team in db.Teams
                                where team.TeamName == ide.TeamName
                                select team;

                    ApplicationUser user = userManager.FindById((qwery.FirstOrDefault<Team>().LeaderId));
                    TeamLeads.Add(user.UserName);
                }
                ViewBag.TeamLeads = TeamLeads;
                // Pass the list of ideas to the view
                return View();
            }
            else return RedirectToAction("Login", "Account");
            
        }

        [HttpPost]
        public ActionResult Index(IdeaViewModel idea)
        {
            
            if (ModelState.IsValid)
            {
                //adding Team
                Team team = new Team();
                team.TeamName = idea.TeamName;
                team.LeaderId = User.Identity.GetUserId();
                db.Teams.Add(team);
                db.SaveChanges();

                //adding to TeamMembers
                TeamMember tm = new TeamMember();
                tm.TeamName = idea.TeamName;
                tm.TeamMates = User.Identity.GetUserId();
                db.TeamMembers.Add(tm);
                db.SaveChanges();

                //adding Idea
                Idea ideaTemp = new Idea();
                ideaTemp.IdeaTitle = idea.IdeaTitle;
                ideaTemp.IdeaDescription = idea.IdeaDescription;
                ideaTemp.TeamName = idea.TeamName;
                ideaTemp.Score = 0;

                // Add the idea to the Ideas DbSet
                db.Ideas.Add(ideaTemp);

                // Save changes to the database
                db.SaveChanges();

                // Redirect to a success page or return a view
                return RedirectToAction("Index", "User"); // Redirect to Home/Index
            }

            // If model state is not valid, return the view with errors
            return View(idea);
        }



        [HttpPost]
        public ActionResult JoinTeam(string TeamName)
        {
            var appDbContext = new ApplicationDbContext();
            var userStore = new ApplicationUserStore(appDbContext);
            var userManager = new ApplicationUserManager(userStore);
           // return Content(TeamName);
            string uid = User.Identity.GetUserId();
            int memberCount = db.TeamMembers
            .Where(tm => tm.TeamName == TeamName)
            .Count();

            if (TeamName != null && memberCount < limit)
            {
                TeamMember tm = new TeamMember();
                tm.TeamName = TeamName;
                tm.TeamMates = User.Identity.GetUserId();
                db.TeamMembers.Add(tm);
                tm.TeamMemberId = 56;
                db.TeamMembers.Add(tm);
                db.SaveChanges();

                // Redirect to some success page or back to the same page
                return RedirectToAction("Index","User");
            }
            else
            {
                // Handle the case where the idea is not found
                return RedirectToAction("Index","User");
            }
        }

        public ActionResult Ideas() {

            string uid = User.Identity?.GetUserId();


            var temp = GetTeamsPartOf(uid);

            var ideasPartOf = db.Ideas
            .Where(idea => temp.Contains(idea.TeamName))
            .ToList();

            ViewBag.Ideas = ideasPartOf;

            return View(); 

        }

        public ActionResult Teams() 
        {
            var appDbContext = new ApplicationDbContext();
            var userStore = new ApplicationUserStore(appDbContext);
            var userManager = new ApplicationUserManager(userStore);
            var teams = GetTeamsPartOf(User.Identity.GetUserId());
            List<TeamModelView> teamModels = new List<TeamModelView>();
            foreach(string team in teams)
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
                foreach(var iter in allTeamMemberIds)
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

       
    }
}