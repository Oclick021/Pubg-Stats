using Pubg.Net;
using Pubg.Net.Models.Stats;
using PubgSDK.Helpers;
using PubgSDK.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PubgSDK.Models
{

    public class SeasonStats : ISeasonStats
    {
        private static string currentSeasonID;

        public SeasonStats()
        {
        }

        public static string CurrentSeasonID
        {
            get
            {
                if (currentSeasonID == null)
                {
                    GetCurrentSeason();
                }
                return currentSeasonID;
            }
            set => currentSeasonID = value;
        }

        public string ID { get; set; }
        public float BestRankPoint { get; set; }
        public int Assists { get; set; }
        public int DailyKills { get; set; }
        public int DailyWins { get; set; }
        public float DamageDealt { get; set; }
        public int HeadshotKills { get; set; }
        public int Kills { get; set; }
        public float LongestKill { get; set; }
        public int MaxKillStreaks { get; set; }
        public float RankPoints { get; set; }
        public int RoundsPlayed { get; set; }
        public int Top10s { get; set; }
        public int WeeklyKills { get; set; }
        public int WeeklyWins { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }

        public static void GetCurrentSeason()
        {
            var seasons = new PubgSeasonService()
                   .GetSeasonsPC(PubgPlatform.Steam, Config.PubgToken);

            currentSeasonID = seasons
                .Where(s => s.IsCurrentSeason == true)
                .FirstOrDefault().Id;
        }

        public string GetKD()
        {
            if (Losses == 0)
            {
                return Kills.ToString();
            }
            double kd = (double)Kills / (double)Losses;
            return Math.Round(kd, 2).ToString();

        }

        public string GetTop10Rate()
        {
        double topRate =  ((double)Top10s/ (double)RoundsPlayed) * 100;
            return Math.Round(topRate, 2).ToString() + "%";

        }

        public string GetWinRate()
        {
            double winrate =  ((double)Wins / (double)RoundsPlayed) * 100;
            return Math.Round(winrate, 2).ToString() + "%";
        }

        public string GetListValue()
        {
            var values = new StringBuilder();
            values.AppendLine(BestRankPoint.ToString());
            values.AppendLine(Assists.ToString());
            values.AppendLine(DailyKills.ToString());
            values.AppendLine(DailyWins.ToString());
            values.AppendLine(DamageDealt.ToString());
            values.AppendLine(HeadshotKills.ToString());
            values.AppendLine(Kills.ToString());
            values.AppendLine(LongestKill.ToString());
            values.AppendLine(Losses.ToString());
            values.AppendLine(MaxKillStreaks.ToString());
            values.AppendLine(RankPoints.ToString());
            values.AppendLine(RoundsPlayed.ToString());
            values.AppendLine(Top10s.ToString());
            values.AppendLine(WeeklyKills.ToString());
            values.AppendLine(WeeklyWins.ToString());
            values.AppendLine(Wins.ToString());
            values.AppendLine(GetKD());
            values.AppendLine(GetWinRate());
            values.AppendLine(GetTop10Rate());
            return values.ToString();
        }
        public static string GetTitles()
        {
            var values = new StringBuilder();
            values.AppendLine("Best Rank Point");

            values.AppendLine("Assists");

            values.AppendLine("Daily Kills");

            values.AppendLine("Daily Wins");

            values.AppendLine("Damage Dealt");

            values.AppendLine("Headshot Kills");

            values.AppendLine("Kills");

            values.AppendLine("Longest Kill");

            values.AppendLine("Death");

            values.AppendLine("Max Kill Streaks");

            values.AppendLine("Rank Points");

            values.AppendLine("Rounds Played");

            values.AppendLine("Top 10s");

            values.AppendLine("Weekly Kills");

            values.AppendLine("Weekly Wins");

            values.AppendLine("Wins");

            values.AppendLine("K/D Ratio");
            values.AppendLine("Win Rate");
            values.AppendLine("Top 10 Rate");


            return values.ToString();
        }

    }
}
