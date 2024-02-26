namespace WinSite.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Idea
    {
        public int IdeaId { get; set; }

        [Required]
        [StringLength(50)]
        public string IdeaTitle { get; set; }

        public string IdeaDescription { get; set; }

        public int? Score { get; set; }

        [StringLength(50)]
        public string TeamName { get; set; }

        public virtual Team Team { get; set; }
    }
}
