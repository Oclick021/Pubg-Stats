using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pubg.Net;
using PubgSDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgSDK.Helpers
{
    public class PubgHelper
    {

        /// <summary>
        /// Retrieves Players data from database. if doesnt exists then GetPubgPlayer method is called
        /// </summary>
        public static async Task<Player> GetPlayerByName(string name)
        {
            var playerFound = await PubgDB.Instance.Players
              .Where(p => p.Name == name).FirstOrDefaultAsync();

            if (playerFound == null)
            {
                var onlinePlayer = await GetPubgPlayer(name: name);
                if (onlinePlayer != null)
                {
                    playerFound = new Player() { Name = name, Id = onlinePlayer.Id };
                    await playerFound.GetPlayerStats();
                    await playerFound.GetMatches(onlinePlayer);

                }
            }
            else
            {
                await playerFound.GetPlayerStats();
                await playerFound.GetMatches();
            }
            return playerFound;
        }

        public static async Task<IEnumerable<Player>> GetPlayersByName(string[] names)
        {
            var players = new List<Player>();
            foreach (string name in names)
            {
                players.Add(await GetPlayerByName(name));
            }
            return players;
        }
        /// <summary>
        /// Retrieves Pubgplayer data form Pubg corp database
        /// </summary>
        /// 
        public static async Task<PubgPlayer> GetPubgPlayer(string id = null, string name = null)
        {
            PubgPlayer player = null;
            try
            {


                var playerService = new PubgPlayerService();
                if (id != null)
                {
                    player = await playerService.GetPlayerAsync(PubgPlatform.Steam, id, Credentials.PubgToken);
                }
                else if (name != null)
                {
                    var result = await playerService.GetPlayersAsync(PubgPlatform.Steam, new GetPubgPlayersRequest
                    {
                        ApiKey = Credentials.PubgToken,
                        PlayerNames = new string[] { name }
                    });
                    player = result.FirstOrDefault();
                }
            }
            catch (Exception ss)
            {

            }
            return player;

        }





        public static async Task<Match> GetPubgMatch(string matchID)
        {
            var service = new PubgMatchService();
            var match = await service.GetMatchAsync(PubgPlatform.Steam, matchID, Credentials.PubgToken);
            string serialized = JsonConvert.SerializeObject(match);
            var m = JsonConvert.DeserializeObject<Match>(serialized);
            return m;
        }
    }
}
