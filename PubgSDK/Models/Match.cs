using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PubgSDK.Models
{

    public class Match
    {
        public string Id { get; set; }

        public DateTime CreatedAt { get; set; }
        public int Duration { get; set; }
        public virtual ICollection<Roster> Rosters { get; set; }
        public string GameMode { get; set; }
        public string MapName { get; set; }
        public bool IsCustomMatch { get; set; }
        public string SeasonState { get; set; }

        public virtual ICollection<PlayerMatch> Players { get; set; }
        public Match()
        {

        }


        public  ParticipantsStats GetParticipantsMatchStats(string playerName)
        {
            var player = Rosters.Select(p => p.Participants.Where(pl => pl.Stats.Name == playerName).FirstOrDefault()).FirstOrDefault();
            return player.Stats;
        }
        public string GetParticipantsMatchStatsString(string playerName)
        {
            var stats = GetParticipantsMatchStats(playerName);
            var values = new StringBuilder();
            values.AppendLine(stats.DamageDealt.ToString() );
            values.AppendLine(stats.Kills.ToString());
            values.AppendLine(stats.HeadshotKills.ToString());
            values.AppendLine(stats.Assists.ToString());
            values.AppendLine(stats.LongestKill.ToString());
            values.AppendLine(stats.KillStreaks.ToString());
            values.AppendLine(stats.DBNOs.ToString());
            values.AppendLine(stats.Revives.ToString());
            values.AppendLine(stats.TimeSurvived.ToString());
            return values.ToString();
        }
        public static string GetTitles()
        {
            var values = new StringBuilder();
            values.AppendLine("DamageDealt");
            values.AppendLine("Kills");
            values.AppendLine("HeadshotKills");
            values.AppendLine("Assists");
            values.AppendLine("LongestKill");
            values.AppendLine("KillStreaks");
            values.AppendLine("DBNOs");
            values.AppendLine("Revives");
            values.AppendLine("TimeSurvived");
            values.AppendLine("WinPlace");
            return values.ToString();
        }


    }





}
