using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WinSite.ViewModels
{
    public class TeamModelView
    {
        [DisplayName("Team Name")]
        public string TeamName { get; set; }
        [DisplayName("Leader Name")]
        public string LeaderName { get; set; }
        public string IdeaName { get; set; }
        public List<string> Members { get; set;}

        public List<string> AllEmails { get; set; }
    }
}