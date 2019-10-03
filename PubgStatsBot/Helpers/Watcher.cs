using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using PubgSDK.Helpers;
using PubgSDK.Models;
using PubgSDK.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PubgStatsBot.Helpers
{
    public class Watcher
    {
        //private const long watchman = 456447610551664662;
        readonly DiscordSocketClient client;
        public Watcher(DiscordSocketClient client)
        {
            //this.client = client;
            //StartWatch().ConfigureAwait(false);
        }
        public async Task StartWatch()
        {
            await Task.Run(async () =>
             {
                 while (true)
                 {
                     List<ulong> usersList = new List<ulong>();
                     try
                     {

                         foreach (var guild in client.Guilds)
                         {
                             foreach (var guildUser in guild.Users.Where(u => u.Status == UserStatus.Online))
                             {

                                 //Console.WriteLine(guildUser.Username);
                                 if (usersList.Contains(guildUser.Id))
                                 {
                                     continue;
                                 }
                                 using (var con = new BotDBContext())
                                 {
                                     if (con.UsersPlayers.Include(u => u.User).Any(u => u.User.DiscordId == guildUser.Id))
                                     {
                                         usersList.Add(guildUser.Id);
                                         await CheckUser(guildUser).ConfigureAwait(false);
                                     }
                                 }

                             }
                         }
                     }
                     catch (Exception ee)
                     {
                         Console.WriteLine(ee.ToString());
                     }

                     await Task.Delay(5000);
                 }
             });
            //return null;
        }

        async Task CheckUser(SocketGuildUser user)
        {
            using (var con = new BotDBContext())
            {
                var userInDb = await con.UsersPlayers.Include(u => u.User).Where(u => u.User.DiscordId == user.Id).Include(p => p.Players).ThenInclude(m => m.Matches).FirstOrDefaultAsync();
                if (userInDb != null)
                {
                    foreach (var player in userInDb.Players)
                    {
                        var playerRep = new PlayerRepository();
                     await   playerRep.GetPlayerById(player.PubgID);
                        foreach (var playerMatch in playerRep.Player.Matches.OrderByDescending(d => d.Match.CreatedAt))
                        {
                            if (playerMatch.Match.CreatedAt > DateTime.Now.AddHours(-4) && !player.Matches.Any(m => m.MatchId == playerMatch.MatchId))
                            {
                                if (playerMatch.Match.GetParticipantsMatchStats(playerRep.Player.Name).WinPlace < 10)
                                {
                                    var participants = playerMatch.Match.GetMyTeam(playerRep.Player.Name);
                                    await Discord.UserExtensions.SendMessageAsync(user, embed: EmbedHelper.GetParticipantsStats(participants, playerMatch.Match));
                                    player.Matches.Add(new Model.Match() { MatchId = playerMatch.MatchId });
                                    con.UsersPlayers.Update(userInDb);
                                    await con.SaveChangesAsync();
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }
        }

    }

}
