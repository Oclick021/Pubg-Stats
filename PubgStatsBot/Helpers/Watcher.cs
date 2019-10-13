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
            this.client = client;
            StartWatch().ConfigureAwait(false);
        }
        public async Task StartWatch()
        {

            while (true)
            {
                List<ulong> usersList = new List<ulong>();
                try
                {
                    var discordHelper = new DiscordHelper(client);
                    var users = discordHelper.GetAllUsers();
                    var onlineUsers = users.Where(u => u.Status == UserStatus.Online);

                    foreach (var guildUser in onlineUsers)
                    {

                        //Console.WriteLine(guildUser.Username);
                        if (!usersList.Contains(guildUser.Id))
                        {
                            using (var con = new BotDBContext())
                            {
                                var userInDb = con.UsersPlayers.Include(u => u.User).ToList();
                                if (userInDb.Any(u => u.User.DiscordId == guildUser.Id))
                                {
                                    usersList.Add(guildUser.Id);
                                    await CheckUser(guildUser);
                                }
                            }
                        }

                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.ToString());
                }
                Console.WriteLine("Recycled");
                await Task.Delay(Config.Instance.WatcherDelay * 1000 * 60);
            }

            //return null;
        }

        async Task CheckUser(SocketGuildUser user)
        {
            using (var con = new BotDBContext())
            {
                var userInDb = await con.UsersPlayers.Include(u => u.User).Where(u => u.User.DiscordId == user.Id).Include(p => p.Players).ThenInclude(m => m.Matches).FirstOrDefaultAsync();
                if (userInDb != null)
                {
                    Console.WriteLine("UserCheck");
                    foreach (var player in userInDb.Players)
                    {
                        var playerRep = new PlayerRepository();
                        await playerRep.GetPlayerById(player.PubgID);
                        await playerRep.LoadMatches();
                        foreach (var playerMatch in playerRep.Player.Matches.OrderByDescending(d => d.Match.CreatedAt))
                        {
                            if (playerMatch.Match.CreatedAt > DateTime.Now.AddHours(-Config.Instance.WatchRecentHours))
                            {
                                if (!con.Notifications.Any(m => m.MatchID == playerMatch.MatchId && m.UserId == user.Id))
                                {


                                    if (playerMatch.Match.GetParticipantsMatchStats(playerRep.Player.Name).WinPlace < Config.Instance.WinPlace)
                                    {
                                        var participants = playerMatch.Match.GetMyTeam(playerRep.Player.Name);
                                        Console.WriteLine($"Message Sent to {user.Id} | {user.Username} {playerMatch.MatchId} at {DateTime.Now}");

                                        await Discord.UserExtensions.SendMessageAsync(user, embed: EmbedHelper.GetParticipantsStats(participants, playerMatch.Match));
                                        con.Notifications.Add(new Model.Notification() { UserId = user.Id, MatchID = playerMatch.MatchId });
                                        await con.SaveChangesAsync();
                                        player.Matches.Add(new Model.Match() { MatchId = playerMatch.MatchId });
                                        con.UsersPlayers.Update(userInDb);
                                        await con.SaveChangesAsync();
                                    }
                                }


                            }

                        }
                    }
                }
            }
        }

    }

}
