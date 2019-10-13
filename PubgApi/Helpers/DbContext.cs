using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PubgAPI.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace PubgAPI.Helpers
{
    public class ApiDbContext : DbContext
    {
        //private static BotDBContext instance;

        public DbSet<User> Users { get; set; }

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
                .UseSqlite("Data Source=Api.db")
                .EnableSensitiveDataLogging(true)
                .EnableDetailedErrors(true);
            ;
        }


    }
}
