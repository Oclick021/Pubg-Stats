using Microsoft.EntityFrameworkCore;
using PubgStatsBot.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PubgStatsBot.Helpers
{
    public class PubgBotDBContext : DbContext
    {
        private static PubgBotDBContext instance;

        //Enables these commonly used commands:
        //Add-Migration
        //Drop-Database
        //Get-DbContext
        //Scaffold-DbContext
        //Script-Migrations
        //Update-Database

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies()
                .UseSqlServer(@"Server=.\;Database=pubgBot;Trusted_Connection=True;MultipleActiveResultSets=true")
                .EnableSensitiveDataLogging(true)
                .EnableDetailedErrors(true);
            ;
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<User> Users { get; set; }



        public static PubgBotDBContext Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PubgBotDBContext();
                }
                return instance;
            }
            set => instance = value;
        }
    }
}
