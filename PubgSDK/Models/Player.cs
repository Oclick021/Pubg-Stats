using Microsoft.EntityFrameworkCore;
using Pubg.Net;
using PubgSDK.Extentions;
using PubgSDK.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgSDK.Models
{
    public class Player
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public virtual SeasonStats SoloStats { get; set; }
        public string SoloStatsID { get; set; }
        public virtual SeasonStats DuoStats { get; set; }
        public string DuoStatsID { get; set; }

        public virtual SeasonStats SquadStats { get; set; }
        public string SquadStatsID { get; set; }

        public virtual List<PlayerMatch> Matches { get; set; }
        public DateTime? CurrentSeasionLastUpdate { get; set; }
        public DateTime? LastMatchUpdate { get; set; }



        public Player()
        {

        }


        public IEnumerable<Roster> GetLatestRosters()
        {
            return Matches.Select(r => r.Match.Rosters.Where(p => p.Participants.Any(s => s.Stats.Name == Name))).SelectMany(x => x);
        }
        public IEnumerable<Roster> GetWinningRosters()
        {
            return GetLatestRosters().Where(r => r.Won).ToList();
        }


    }
}
