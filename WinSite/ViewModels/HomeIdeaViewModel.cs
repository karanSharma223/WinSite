using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WinSite.ViewModels
{
    public class HomeIdeaViewModel
    {
        
        [DisplayName("Idea Title")]
        public string IdeaTitle { get; set; }
        
        [DisplayName("Idea Description")]
        public string IdeaDescription { get; set; }
        
        [DisplayName("Team Name")]
        public string TeamName { get; set; }
        public string TeamLeader {  get; set; }
    }
}