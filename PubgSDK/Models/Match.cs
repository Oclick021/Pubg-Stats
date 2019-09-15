using PubgSDK.Extentions;
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


        public ParticipantsStats GetParticipantsMatchStats(string playerName)
        {
            foreach (var participants in from roster in Rosters
                                         from participants in roster.Participants
                                         where participants.Stats.Name == playerName
                                         select participants)
            {
                return participants.Stats;
            }
            return null;
        }
        public IEnumerable<Participant> GetMyTeam(string playerName)
        {
            foreach (var roster in Rosters)
            {
                foreach (var participant in roster.Participants)
                {
                    if (participant.Stats.Name == playerName)
                    {
                        return roster.Participants;
                    }
                }
            }
            return null;
        }
        public string GetMapName()
        {
            switch (MapName)
            {
                case "Desert_Main":
                    return "MIR";
                case "DihorOtok_Main":
                    return "VIK";

                case "Savage_Main":
                    return "SAN";
                case "Unspecified":
                    return "ERA";
                case "Erangel_Main":
                    return "ERA OG";
                default:
                    return "";
            }
        }
        public string GetParticipantsMatchStatsString(string playerName)
        {
            var stats = GetParticipantsMatchStats(playerName);
            var values = new StringBuilder();
            values.AppendLine(stats.DamageDealt.ToString("F1"));
            values.AppendLine(stats.Kills.ToString());
            values.AppendLine(stats.HeadshotKills.ToString());
            values.AppendLine(stats.Assists.ToString());
            values.AppendLine(stats.LongestKill.ToString("F1")+ "  M");
            values.AppendLine(stats.KillStreaks.ToString());
            values.AppendLine(stats.DBNOs.ToString());
            values.AppendLine(stats.Revives.ToString());
            values.AppendLine(new TimeSpan(0,0, (int)stats.TimeSurvived).ToString());
            values.AppendLine(CreatedAt.TimeAgo());

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
