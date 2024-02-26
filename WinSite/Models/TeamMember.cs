namespace WinSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TeamMember
    {
        public int TeamMemberId { get; set; }

        [Required]
        [StringLength(50)]
        public string TeamName { get; set; }

        [StringLength(50)]
        public string TeamMates { get; set; }

        public virtual Team Team { get; set; }
    }
}
