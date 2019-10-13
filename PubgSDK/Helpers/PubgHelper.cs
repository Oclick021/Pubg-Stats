using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pubg.Net;
using PubgSDK.Models;
using PubgSDK.Repository;
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

        public static async Task<IEnumerable<Player>> GetPlayersByName(string[] names)
        {
            var players = new List<Player>();
            foreach (string name in names)
            {
                var playerRep = new PlayerRepository();
                await playerRep.GetPlayerByName(name);
                players.Add(playerRep.Player);
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
                    player = await playerService.GetPlayerAsync(PubgPlatform.Steam, id, Config.PubgToken);
                }
                else if (name != null)
                {
                    var result = await playerService.GetPlayersAsync(PubgPlatform.Steam, new GetPubgPlayersRequest
                    {
                        ApiKey = Config.PubgToken,
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



        public static async Task SavePubgMatch(Match match)
        {
            using (var con = new PubgDB())
            {
                var matchInDb = await con.Matches.FirstOrDefaultAsync(m => m.Id == match.Id);
                if (matchInDb == null)
                {

                    con.Matches.Add(match);
                    await con.SaveChangesAsync();
                }
            }
        }

        public static async Task<Match> GetPubgMatch(string matchID)
        {
            var service = new PubgMatchService();
            var match = await service.GetMatchAsync(PubgPlatform.Steam, matchID, Config.PubgToken);
            string serialized = JsonConvert.SerializeObject(match);
            var m = JsonConvert.DeserializeObject<Match>(serialized);
            return m;
        }
    }
}
