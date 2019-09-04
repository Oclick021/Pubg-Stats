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
        public virtual SeasonStats DuoStats { get; set; }
        public virtual SeasonStats SquadStats { get; set; }
        public virtual ICollection<PlayerMatch> Matches { get; set; }
        public DateTime? CurrentSeasionLastUpdate { get; set; }
        public DateTime? LastMatchUpdate { get; set; }

        readonly PubgPlayerService playerService = new PubgPlayerService();


        public Player()
        {

        }



        public async Task GetMatches(PubgPlayer player = null)
        {
            if (LastMatchUpdate < DateTime.Now.AddMinutes(-10))
            {
                if (player == null)
                {
                    player = await PubgHelper.GetPubgPlayer(Id);
                }
                var list = new List<PlayerMatch>();
                using (var con = PubgDB.Instance)
                {
                    foreach (var matchId in player.MatchIds)
                    {

                        var matchFound = con.Matches.Find(matchId);
                        if (matchFound == null)
                        {
                            matchFound = await PubgHelper.GetPubgMatch(matchId);
                        }
                        else
                        {
                            con.Matches.Attach(matchFound);
                        }
                        list.Add(new PlayerMatch() { Player = this, Match = matchFound });
                    }
                    Matches.Concat(list);
                    await con.SaveChangesAsync();
                }
            }

        }
        public async Task GetPlayerStats()
        {
            if (CurrentSeasionLastUpdate == null || CurrentSeasionLastUpdate.Value <= DateTime.Now.AddMinutes(-15))
            {
                var playerSeason = await playerService.GetPlayerSeasonAsync(PubgPlatform.Steam, Id, SeasonStats.CurrentSeasonID, Credentials.PubgToken);
                SoloStats = new SeasonStats();
                SoloStats.Clone(playerSeason.GameModeStats.SoloFPP);
                DuoStats = new SeasonStats();
                DuoStats.Clone(playerSeason.GameModeStats.DuoFPP);
                SquadStats = new SeasonStats();
                SquadStats.Clone(playerSeason.GameModeStats.SquadFPP);
                CurrentSeasionLastUpdate = DateTime.Now;
            }
        }


        public async Task Save()
        {
            using (var con = PubgDB.Instance)
            {
                var player = await con.Players.FindAsync(Id);
                if (player == null)
                {
                    con.Players.Add(this);
                    await con.SaveChangesAsync();
                }
                else
                {
                    player.SoloStats = SoloStats;
                    player.DuoStats = DuoStats;
                    player.SquadStats = SquadStats;
                    player.CurrentSeasionLastUpdate = CurrentSeasionLastUpdate;
                    con.Players.Update(player);
                    await con.SaveChangesAsync();
                }
            }
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
