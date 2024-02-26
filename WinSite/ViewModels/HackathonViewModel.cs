using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WinSite.ViewModels
{
    public class HackathonViewModel
    {
        
        public string HackathonName { get; set; }

        public int Teamsize { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        
        public string FirstPlace { get; set; }

        
        public string SecondPlace { get; set; }

        
        public string ThirdPlace { get; set; }

    }
}