using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WinSite.ViewModels
{
    public class IdeaViewModel
    {
        [Required(ErrorMessage ="Idea Title cannot be empty")]
        [DisplayName("Idea Title")]
        public string IdeaTitle {  get; set; }
        [Required(ErrorMessage = "Idea Description cannot be empty")]
        [DisplayName("Idea Description")]
        public string IdeaDescription { get; set; }
        [Required(ErrorMessage = "TeamName cannot be empty")]
        [DisplayName("Team Name")]
        public string TeamName {  get; set; }
    }
}