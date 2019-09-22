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

        readonly PubgPlayerService playerService = new PubgPlayerService();


        public Player()
        {

        }



        public async Task GetMatches(PubgPlayer player = null)
        {
            if (Matches == null)
                Matches = new List<PlayerMatch>();

            if (LastMatchUpdate == null || LastMatchUpdate < DateTime.Now.AddMinutes(-10))
            {
                if (player == null)
                {
                    player = await PubgHelper.GetPubgPlayer(Id);
                }

                foreach (var matchId in player.MatchIds.Take(10))
                {

                    var matchFound = await PubgDB.Instance.Matches.Where(m => m.Id == matchId).FirstOrDefaultAsync();
                    if (matchFound == null)
                        matchFound = await PubgHelper.GetPubgMatch(matchId);

                    if (!Matches.Any(m => m.MatchId == matchFound.Id))
                        Matches.Add(new PlayerMatch() { Player = this, Match = matchFound });
                }


                LastMatchUpdate = DateTime.Now;
                await SaveAsync();
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
                await SaveAsync();
            }
        }


        public async Task SaveAsync()
        {


            var player = await PubgDB.Instance.Players.FindAsync(Id);
            if (player == null)
            {
                await PubgDB.Instance.Players.AddAsync(this);
                await PubgDB.Instance.SaveChangesAsync();
            }
            else
            {
                //player.SoloStats = SoloStats;
                //player.DuoStats = DuoStats;
                //player.SquadStats = SquadStats;
                //player.Matches = Matches;
                //player.LastMatchUpdate = LastMatchUpdate;
                //player.CurrentSeasionLastUpdate = CurrentSeasionLastUpdate;
                PubgDB.Instance.Players.Update(this);
                await PubgDB.Instance.SaveChangesAsync();
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
