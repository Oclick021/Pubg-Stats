using Microsoft.EntityFrameworkCore;
using Pubg.Net;
using PubgSDK.Extentions;
using PubgSDK.Helpers;
using PubgSDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgSDK.Repository
{
    public class PlayerRepository
    {

        private readonly PubgDB PubgDB;
        readonly PubgPlayerService playerService;

        public Player Player { get; set; }



        public PlayerRepository()
        {
            PubgDB = new PubgDB();
            playerService = new PubgPlayerService();
        }
        public async Task GetPlayerById(string id)
        {
            Player = await PubgDB.Players
            .Where(p => p.Id == id).FirstOrDefaultAsync();

            if (Player == null)
            {
                var onlinePlayer = await PubgHelper.GetPubgPlayer(id);
                if (onlinePlayer != null)
                {
                    Player = new Player() { Name = onlinePlayer.Name, Id = onlinePlayer.Id };
                    await GetPlayerStats();
                    await GetMatches(onlinePlayer);
                    PubgDB.Players.Add(Player);

                }
            }
            else
            {
                await GetPlayerStats();
                await GetMatches();
                PubgDB.Players.Update(Player);

            }
            await PubgDB.SaveChangesAsync();
        }
        public async Task GetPlayerByName(string name)
        {

            Player = await PubgDB.Players
                .Where(p => p.Name == name)
                .Include(p => p.DuoStats)
                .Include(p => p.SoloStats)
                .Include(p => p.SquadStats)
                .Include(p => p.Matches)
                .FirstOrDefaultAsync();


            if (Player == null)
            {
                var onlinePlayer = await PubgHelper.GetPubgPlayer(name: name);
                if (onlinePlayer != null)
                {
                    Player = new Player() { Name = name, Id = onlinePlayer.Id };
                    await GetPlayerStats();
                    await GetMatches(onlinePlayer);
                    PubgDB.Players.Add(Player);

                }
            }
            else
            {
                await GetPlayerStats();
                await GetMatches();
                PubgDB.Players.Update(Player);
            }
            await PubgDB.SaveChangesAsync();
        }


        public PlayerRepository(Player player)
        {
            playerService = new PubgPlayerService();
            PubgDB = new PubgDB();
            Player = player;
        }





        public async Task LoadMatches()
        {

            foreach (var match in Player.Matches)
            {
                match.Match = await PubgDB.Matches
                    .Where(m => m.Id == match.MatchId)
                    .Include(m => m.Rosters)
                            .ThenInclude(r => r.Participants)
                                 .ThenInclude(p => p.Stats)
                    .FirstOrDefaultAsync();
            }

        }


        public async Task GetMatches(PubgPlayer player = null)
        {
            if (Player.Matches == null)
                Player.Matches = new List<PlayerMatch>();

            if (Player.LastMatchUpdate == null || Player.LastMatchUpdate < DateTime.Now.AddMinutes(-10))
            {
                if (player == null)
                {
                    player = await PubgHelper.GetPubgPlayer(Player.Id);
                }

                foreach (var matchId in player.MatchIds.Take(10))
                {
                    Match matchFound = null;

                    matchFound = await PubgDB.Matches.Where(m => m.Id == matchId).FirstOrDefaultAsync();
                    if (matchFound == null)
                        matchFound = await PubgHelper.GetPubgMatch(matchId);

                    if (!Player.Matches.Any(m => m.MatchId == matchFound.Id))
                    {
                        Player.Matches.Add(new PlayerMatch() { Player = Player, Match = matchFound });
                        PubgDB.Attach(matchFound);
                    }

                    await PubgHelper.SavePubgMatch(matchFound);
                }
                Player.LastMatchUpdate = DateTime.Now;
            }

        }


        public async Task GetPlayerStats()
        {
            if (Player.CurrentSeasionLastUpdate == null || Player.CurrentSeasionLastUpdate.Value <= DateTime.Now.AddMinutes(-15))
            {
                var playerSeason = await playerService.GetPlayerSeasonAsync(PubgPlatform.Steam, Player.Id, SeasonStats.CurrentSeasonID, Credentials.PubgToken);
                Player.SoloStats = new SeasonStats();
                Player.SoloStats.Clone(playerSeason.GameModeStats.SoloFPP);
                Player.DuoStats = new SeasonStats();
                Player.DuoStats.Clone(playerSeason.GameModeStats.DuoFPP);
                Player.SquadStats = new SeasonStats();
                Player.SquadStats.Clone(playerSeason.GameModeStats.SquadFPP);
                Player.CurrentSeasionLastUpdate = DateTime.Now;
            }
        }

    }
}
