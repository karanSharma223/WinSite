using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WinSite.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Hackathon> Hackathons { get; set; }
        public virtual DbSet<Idea> Ideas { get; set; }
        public virtual DbSet<TeamMember> TeamMembers { get; set; }
        public virtual DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hackathon>()
                .Property(e => e.HackathonName)
                .IsUnicode(false);

            modelBuilder.Entity<Hackathon>()
                .Property(e => e.FirstPlace)
                .IsUnicode(false);

            modelBuilder.Entity<Hackathon>()
                .Property(e => e.SecondPlace)
                .IsUnicode(false);

            modelBuilder.Entity<Hackathon>()
                .Property(e => e.ThirdPlace)
                .IsUnicode(false);

            modelBuilder.Entity<Idea>()
                .Property(e => e.IdeaTitle)
                .IsUnicode(false);

            modelBuilder.Entity<Idea>()
                .Property(e => e.IdeaDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Idea>()
                .Property(e => e.TeamName)
                .IsUnicode(false);

            modelBuilder.Entity<TeamMember>()
                .Property(e => e.TeamName)
                .IsUnicode(false);

            modelBuilder.Entity<TeamMember>()
                .Property(e => e.TeamMates)
                .IsUnicode(false);

            modelBuilder.Entity<Team>()
                .Property(e => e.TeamName)
                .IsUnicode(false);

            modelBuilder.Entity<Team>()
                .Property(e => e.LeaderId)
                .IsUnicode(false);

            modelBuilder.Entity<Team>()
                .HasMany(e => e.Hackathons)
                .WithOptional(e => e.Team)
                .HasForeignKey(e => e.FirstPlace);

            modelBuilder.Entity<Team>()
                .HasMany(e => e.Hackathons1)
                .WithOptional(e => e.Team1)
                .HasForeignKey(e => e.SecondPlace);

            modelBuilder.Entity<Team>()
                .HasMany(e => e.Hackathons2)
                .WithOptional(e => e.Team2)
                .HasForeignKey(e => e.ThirdPlace);

            modelBuilder.Entity<Team>()
                .HasMany(e => e.TeamMembers)
                .WithRequired(e => e.Team)
                .WillCascadeOnDelete(false);
        }
    }
}
