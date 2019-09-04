﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PubgSDK.Helpers;

namespace PubgSDK.Migrations
{
    [DbContext(typeof(PubgDB))]
    [Migration("20190903222304_ini")]
    partial class ini
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PubgSDK.Models.Match", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("Duration");

                    b.Property<string>("GameMode");

                    b.Property<bool>("IsCustomMatch");

                    b.Property<string>("MapName");

                    b.Property<string>("SeasonState");

                    b.HasKey("Id");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("PubgSDK.Models.Participant", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Actor");

                    b.Property<string>("RosterId");

                    b.Property<string>("ShardId");

                    b.Property<long?>("StatsId");

                    b.HasKey("Id");

                    b.HasIndex("RosterId");

                    b.HasIndex("StatsId");

                    b.ToTable("Participants");
                });

            modelBuilder.Entity("PubgSDK.Models.ParticipantsStats", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Assists");

                    b.Property<int>("Boosts");

                    b.Property<int>("DBNOs");

                    b.Property<float>("DamageDealt");

                    b.Property<string>("DeathType");

                    b.Property<int>("HeadshotKills");

                    b.Property<int>("Heals");

                    b.Property<int>("KillPlace");

                    b.Property<int>("KillPointsDelta");

                    b.Property<int>("KillStreaks");

                    b.Property<int>("Kills");

                    b.Property<int>("LastKillPoints");

                    b.Property<int>("LastWinPoints");

                    b.Property<float>("LongestKill");

                    b.Property<int>("MostDamage");

                    b.Property<string>("Name");

                    b.Property<string>("PlayerId");

                    b.Property<int>("Revives");

                    b.Property<float>("RideDistance");

                    b.Property<int>("RoadKills");

                    b.Property<float>("SwimDistance");

                    b.Property<int>("TeamKills");

                    b.Property<float>("TimeSurvived");

                    b.Property<int>("VehicleDestroys");

                    b.Property<float>("WalkDistance");

                    b.Property<int>("WeaponsAcquired");

                    b.Property<int>("WinPlace");

                    b.Property<int>("WinPointsDelta");

                    b.HasKey("Id");

                    b.ToTable("ParticipantsStats");
                });

            modelBuilder.Entity("PubgSDK.Models.Player", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("CurrentSeasionLastUpdate");

                    b.Property<string>("DuoStatsID");

                    b.Property<DateTime?>("LastMatchUpdate");

                    b.Property<string>("Name");

                    b.Property<string>("SoloStatsID");

                    b.Property<string>("SquadStatsID");

                    b.HasKey("Id");

                    b.HasIndex("DuoStatsID");

                    b.HasIndex("SoloStatsID");

                    b.HasIndex("SquadStatsID");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("PubgSDK.Models.PlayerMatch", b =>
                {
                    b.Property<string>("PlayerId");

                    b.Property<string>("MatchId");

                    b.HasKey("PlayerId", "MatchId");

                    b.HasIndex("MatchId");

                    b.ToTable("PlayerMatch");
                });

            modelBuilder.Entity("PubgSDK.Models.Roster", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("MatchId");

                    b.Property<string>("ShardId");

                    b.Property<bool>("Won");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.ToTable("Rosters");
                });

            modelBuilder.Entity("PubgSDK.Models.SeasonStats", b =>
                {
                    b.Property<string>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Assists");

                    b.Property<float>("BestRankPoint");

                    b.Property<int>("DailyKills");

                    b.Property<int>("DailyWins");

                    b.Property<float>("DamageDealt");

                    b.Property<int>("HeadshotKills");

                    b.Property<int>("Kills");

                    b.Property<float>("LongestKill");

                    b.Property<int>("Losses");

                    b.Property<int>("MaxKillStreaks");

                    b.Property<float>("RankPoints");

                    b.Property<int>("RoundsPlayed");

                    b.Property<int>("Top10s");

                    b.Property<int>("WeeklyKills");

                    b.Property<int>("WeeklyWins");

                    b.Property<int>("Wins");

                    b.HasKey("ID");

                    b.ToTable("SeasonStats");
                });

            modelBuilder.Entity("PubgSDK.Models.Participant", b =>
                {
                    b.HasOne("PubgSDK.Models.Roster")
                        .WithMany("Participants")
                        .HasForeignKey("RosterId");

                    b.HasOne("PubgSDK.Models.ParticipantsStats", "Stats")
                        .WithMany()
                        .HasForeignKey("StatsId");
                });

            modelBuilder.Entity("PubgSDK.Models.Player", b =>
                {
                    b.HasOne("PubgSDK.Models.SeasonStats", "DuoStats")
                        .WithMany()
                        .HasForeignKey("DuoStatsID");

                    b.HasOne("PubgSDK.Models.SeasonStats", "SoloStats")
                        .WithMany()
                        .HasForeignKey("SoloStatsID");

                    b.HasOne("PubgSDK.Models.SeasonStats", "SquadStats")
                        .WithMany()
                        .HasForeignKey("SquadStatsID");
                });

            modelBuilder.Entity("PubgSDK.Models.PlayerMatch", b =>
                {
                    b.HasOne("PubgSDK.Models.Match", "Match")
                        .WithMany("Players")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("PubgSDK.Models.Player", "Player")
                        .WithMany("Matches")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("PubgSDK.Models.Roster", b =>
                {
                    b.HasOne("PubgSDK.Models.Match")
                        .WithMany("Rosters")
                        .HasForeignKey("MatchId");
                });
#pragma warning restore 612, 618
        }
    }
}
