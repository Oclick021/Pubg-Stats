using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Pubg.Net;
using PubgSDK.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using PubgSDK.Extentions;
using PubgStatsBot.Model;

namespace PubgSDK.Helpers
{
    public class BotDBContext : DbContext
    {
        private static BotDBContext instance;

        public DbSet<User> Users { get; set; }
        public DbSet<UsersPlayers> UsersPlayers { get; set; }

        //Enables these commonly used commands:
        //Add-Migration
        //Drop-Database
        //Get-DbContext
        //Scaffold-DbContext
        //Script-Migrationsj
        //Update-Database

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlite("Data Source=botDB.db")
                .EnableSensitiveDataLogging(true)
                .EnableDetailedErrors(true);
            ;
        }

        public static BotDBContext Instance
        {
            get
            {
                if (instance == null )
                {
                    instance = new BotDBContext();
                }
                return instance;
            }
            set => instance = value;
        }

    }
}
