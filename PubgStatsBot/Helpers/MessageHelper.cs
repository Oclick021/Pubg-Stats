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
            //Checks if the server is authorized
            if (!(_message.Channel is SocketGuildChannel chnl))
            {
                if (!_client.GetChannel(Credentials.ServerID).ChannelHasUser(_message.Author))
                {
                    await _message.Channel.SendMessageAsync("Only members of Pubg stats can use this bot. please consider joining our server first");
                    Console.WriteLine($"{_message.Author.Username} is Not a member");
                    return false;

                }
            }
            else
            {
                if (chnl.Guild.Id != Credentials.ServerID)
                {

                    await _message.Channel.SendMessageAsync("https://discord.gg/3FsMG3", embed: new EmbedBuilder() { Description = "این بات موقتا فقط برای استفاده در PubgStat میباشد لطفا ابتدا عوض گروه شوید", Color = Color.Gold }.Build());
                    Console.WriteLine($"{_message.Author.Username} is Not a member");
                    return false;
                }
            }
            return true;
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


        private async Task<Player> GetPlayer(string cmd, SocketMessage message)
        {
            string playerName = message.Content.Replace("!" + cmd, "").Trim();
            if (playerName.Contains(" "))
            {
                await message.Channel.SendMessageAsync("!خطا : اسم حاویه فاصله است");
                return null;
            }

            var player = await PubgHelper.GetPlayerByName(playerName);
            if (player.Id == null)
            {
                await message.Channel.SendMessageAsync("بازیکنی با این نام یافت نشد");
                return null;
            }
            return player;
        }

    }
}
