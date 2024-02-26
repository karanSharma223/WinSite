namespace WinSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Hackathon
    {
        public int HackathonId { get; set; }

        [Required]
        [StringLength(30)]
        public string HackathonName { get; set; }

        public int? Teamsize { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [StringLength(50)]
        public string FirstPlace { get; set; }

        [StringLength(50)]
        public string SecondPlace { get; set; }

        [StringLength(50)]
        public string ThirdPlace { get; set; }

        public virtual Team Team { get; set; }

        public virtual Team Team1 { get; set; }

        public virtual Team Team2 { get; set; }
    }
}
