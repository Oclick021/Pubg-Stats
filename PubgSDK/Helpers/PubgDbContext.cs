using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pubg.Net;
using PubgSDK.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using PubgSDK.Extentions;

namespace PubgSDK.Helpers
{
    public class PubgDB : DbContext
    {

        //Enables these commonly used commands:
        //Add-Migration
        //Drop-Database
        //Get-DbContext
        //Scaffold-DbContext
        //Script-Migrations
        //Update-Database

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=pubg.db")
                .EnableSensitiveDataLogging(true)
                .EnableDetailedErrors(true);
            ;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerMatch>()
                .HasKey(t => new { t.PlayerId, t.MatchId });
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<SeasonStats> SeasonStats { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Roster> Rosters { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<ParticipantsStats> ParticipantsStats { get; set; }
        public DbSet<PlayerMatch> PlayerMatches { get; set; }


     


    }
}
