using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using PubgSDK.Helpers;
using PubgSDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgStatsBot.Helpers
{
    public class MessageHelper
    {
        readonly DiscordSocketClient _client;
        readonly SocketMessage _message;
        public MessageHelper(DiscordSocketClient client, SocketMessage message)
        {
            _client = client;
            _message = message;
        }
        public async Task<bool> IsAuthorized()
        {
            return true;
            //Checks if the server is authorized
            //if (!(_message.Channel is SocketGuildChannel chnl))
            //{
            //    if (!_client.GetChannel(Credentials.ServerID).ChannelHasUser(_message.Author))
            //    {
            //        await _message.Channel.SendMessageAsync(Strings.OnlyMembersCanUse);
            //        Console.WriteLine($"{_message.Author.Username} {Strings.NotAMember}");
            //        return false;

            //    }
            //}
            //else
            //{
            //    if (chnl.Guild.Id != Credentials.ServerID)
            //    {

            //        await _message.Channel.SendMessageAsync("https://discord.gg/3FsMG3", embed: new EmbedBuilder() { Description = "این بات موقتا فقط برای استفاده در PubgStat میباشد لطفا ابتدا عوض گروه شوید", Color = Color.Gold }.Build());
            //        Console.WriteLine($"{_message.Author.Username} {Strings.NotAMember}");
            //        return false;
            //    }
            //}
            //return true;
        }
        public async Task GetStats(SocketMessage message)
        {
            var player = await GetPlayer("stats", message);
            if (player != null)
            {
                await player.GetPlayerStats();
                await message.Channel.SendMessageAsync(embed: EmbedHelper.GetStats($"{player.Name}'s Solo FPP", player.SoloStats, Color.Red));
                await message.Channel.SendMessageAsync(embed: EmbedHelper.GetStats($"{player.Name}'s Duo FPP", player.DuoStats, Color.Blue));
                await message.Channel.SendMessageAsync(embed: EmbedHelper.GetStats($"{player.Name}'s Squad FPP", player.SquadStats, Color.Teal));

            }
        }

        public async Task GetOnlineUsers()
        {
            await _client.DownloadUsersAsync(await _client.Rest.GetGuildsAsync());
            var stringBuilder = new StringBuilder();
            foreach (var server in _client.Guilds)
            {
                foreach (var user in server.Users)
                {
                    if (user.Status == UserStatus.Online)
                    {
                        stringBuilder.AppendLine($"{ user.Username} is Online");
                        //await Discord.UserExtensions.SendMessageAsync(prohumper, $"{ user.Username} is Online");
                    }
                }
            }
            await _message.Channel.SendMessageAsync(stringBuilder.ToString());
        }



        public async Task GetStatsCompare(SocketMessage message)
        {
            string[] playersName = message.Content.Replace("!compare", "").Trim().Split(" ");
            if (playersName.Length < 2)
            {
                await message.Channel.SendMessageAsync("خطا: اسم 2 بازیکن را وارد کنید!");
            }

            var players = await PubgHelper.GetPlayersByName(playersName);

            if (players.Count() == 0)
            {
                await message.Channel.SendMessageAsync("بازیکنی با این نام ها یافت نشد");
            }
            else
            {
                foreach (var player in players)
                {
                    await player.GetPlayerStats();
                }
                await message.Channel.SendMessageAsync(embed: EmbedHelper.GetCompare($"Compare", players, Color.Red));
            }
        }


        public async Task GetMatchStats(SocketMessage message)
        {
            var player = await GetPlayer("recents", message);
            if (player != null)
            {
                await player.GetMatches();
                await message.Channel.SendMessageAsync(embed: EmbedHelper.GetMatches($"{player.Name}'s Solo FPP", player, Color.Red));
            }
        }

        public async Task AddPlayerToWatch(SocketMessage msg)
        {
            var player = await GetPlayer("addWatch", msg);
            var user = await BotDBContext.Instance.Users.FirstOrDefaultAsync(u => u.DiscordId == msg.Author.Id);
            if (user == null) //It is a new User
            {

                var newUsersPlayer = new Model.UsersPlayers()
                {
                    User = new Model.User() { DiscordId = msg.Author.Id, Name = msg.Author.Username },
                    Players = new List<Model.Player>
            {
                new PubgStatsBot.Model.Player() { PubgID = player.Id }
            }
                };

                await BotDBContext.Instance.UsersPlayers.AddAsync(newUsersPlayer);
                await BotDBContext.Instance.SaveChangesAsync();
                await msg.Channel.SendMessageAsync(Strings.PlayerAddedToWatch);
            }
            else
            {
                var usersPlayers = await BotDBContext.Instance.UsersPlayers.Where(u => u.UserID == user.ID).Include(p => p.Players).FirstOrDefaultAsync();
                if (usersPlayers != null)
                {
                    var playersList = usersPlayers.Players.ToList();
                    if (playersList.FirstOrDefault(a => a.PubgID == player.Id) == null)
                    {
                        playersList.Add(new Model.Player() { PubgID = player.Id });
                        usersPlayers.Players = playersList;
                        BotDBContext.Instance.Update(usersPlayers);
                        await BotDBContext.Instance.SaveChangesAsync();
                        await msg.Channel.SendMessageAsync(Strings.PlayerAddedToWatch);
                    }
                    else
                    {
                        await msg.Channel.SendMessageAsync(Strings.PlayerIsAlreadySaved);
                    }

                }
            }

        }


        public async Task MyWatch(SocketMessage msg)
        {
            var user = await BotDBContext.Instance.Users.FirstOrDefaultAsync(u => u.DiscordId == msg.Author.Id);
            if (user != null)
            {
                var usersPlayers = await BotDBContext.Instance.UsersPlayers.Where(u => u.UserID == user.ID).Include(p => p.Players).FirstOrDefaultAsync();
                if (usersPlayers != null)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine(Strings.YourPlayers);
                    foreach (var player in usersPlayers.Players)
                    {
                        var playerInDB = await PubgDB.Instance.Players.FirstOrDefaultAsync(p => p.Id == player.PubgID);
                        builder.AppendLine(playerInDB.Name);
                    }
                    await msg.Channel.SendMessageAsync(builder.ToString());

                }
                else
                {
                    await msg.Channel.SendMessageAsync(Strings.NoWatchFound);
                }
            }
            else
            {
                await msg.Channel.SendMessageAsync(Strings.NoWatchFound);
            }

        }

        private async Task<Player> GetPlayer(string cmd, SocketMessage message)
        {
            string playerName = message.Content.Replace("!" + cmd, "").Trim();
            if (playerName.Contains(" "))
            {
                await message.Channel.SendMessageAsync();
                return null;
            }

            var player = await PubgHelper.GetPlayerByName(playerName);
            if (player == null)
            {
                await message.Channel.SendMessageAsync(Strings.PlayerNotFound);
                return null;
            }
            return player;
        }



    }
}
