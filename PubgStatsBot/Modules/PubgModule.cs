using Discord;
using Discord.Commands;
using Microsoft.EntityFrameworkCore;
using PubgSDK.Helpers;
using PubgStatsBot.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgStatsBot.Modules
{
    public class PubgModule : ModuleBase<SocketCommandContext>
    {
        [Command("stats")]
        [Alias("allStats", "myStats")]
        public async Task AllStats([Remainder] string playersName = null)
        {
            if (playersName == null)
            {
                await ReplyAsync(Strings.PlayerNameRequired);
                return;
            }

            var player = await PubgHelper.GetPlayerByName(playersName);
            if (player == null)
            {
                await ReplyAsync(Strings.PlayerNotFound);
                return;
            }
            await player.GetPlayerStats();
            await ReplyAsync(embed: EmbedHelper.GetStats($"{player.Name}'s Solo FPP", player.SoloStats, Color.Red));
            await ReplyAsync(embed: EmbedHelper.GetStats($"{player.Name}'s Duo FPP", player.DuoStats, Color.Blue));
            await ReplyAsync(embed: EmbedHelper.GetStats($"{player.Name}'s Squad FPP", player.SquadStats, Color.Teal));
        }
        [Command("solo")]
        [Alias("soloStats", "mysolo")]
        public async Task SoloStats([Remainder] string playersName = null)
        {
            if (playersName == null)
            {
                await ReplyAsync(Strings.PlayerNameRequired);
                return;
            }

            var player = await PubgHelper.GetPlayerByName(playersName);
            if (player == null)
            {
                await ReplyAsync(Strings.PlayerNotFound);
                return;
            }
            await player.GetPlayerStats();
            await ReplyAsync(embed: EmbedHelper.GetStats($"{player.Name}'s Solo FPP", player.SoloStats, Color.Red));
        }

        [Command("duo")]
        [Alias("duoStats", "myDuo")]
        public async Task DuoStats([Remainder] string playersName = null)
        {
            if (playersName == null)
            {
                await ReplyAsync(Strings.PlayerNameRequired);
                return;
            }

            var player = await PubgHelper.GetPlayerByName(playersName);
            if (player == null)
            {
                await ReplyAsync(Strings.PlayerNotFound);
                return;
            }
            await player.GetPlayerStats();
            await ReplyAsync(embed: EmbedHelper.GetStats($"{player.Name}'s Duo FPP", player.DuoStats, Color.Blue));
        }

        [Command("Squad")]
        [Alias("SquadStats", "MySquad")]
        public async Task SquadStats([Remainder] string playersName = null)
        {
            if (playersName == null)
            {
                await ReplyAsync(Strings.PlayerNameRequired);
                return;
            }

            var player = await PubgHelper.GetPlayerByName(playersName);
            if (player == null)
            {
                await ReplyAsync(Strings.PlayerNotFound);
                return;
            }
            await player.GetPlayerStats();
            await ReplyAsync(embed: EmbedHelper.GetStats($"{player.Name}'s Squad FPP", player.SquadStats, Color.Teal));
        }

        [Command("Compare")]
        public async Task SquadStats(params string[] Players)
        {
            if (Players == null)
            {
                await ReplyAsync(Strings.PlayersNamesRequired);
                return;
            }
            var players = await PubgHelper.GetPlayersByName(Players);

            if (players.Count() == 0)
            {
                await ReplyAsync(Strings.PlayerNotFound);
                return;
            }
            else
            {
                foreach (var player in players)
                {
                    await player.GetPlayerStats();
                }
                await ReplyAsync(embed: EmbedHelper.GetCompare($"Compare", players, Color.Red));
            }
        }


        [Command("recents")]
        [Alias("recent", "matches", "last", "match")]

        public async Task Recents([Remainder] string playersName = null)
        {
            if (playersName == null)
            {
                await ReplyAsync(Strings.PlayerNameRequired);
                return;
            }

            var player = await PubgHelper.GetPlayerByName(playersName);
            if (player == null)
            {
                await ReplyAsync(Strings.PlayerNotFound);
                return;
            }
            await player.GetMatches();
            await ReplyAsync(embed: EmbedHelper.GetMatches($"{player.Name}'s Recent matches", player, Color.Red));

        }

        [Command("addwatch")]

        public async Task AddWatch([Remainder] string playersName = null)
        {
            if (playersName == null)
            {
                await ReplyAsync(Strings.PlayerNameRequired);
                return;
            }

            var player = await PubgHelper.GetPlayerByName(playersName);
            if (player == null)
            {
                await ReplyAsync(Strings.PlayerNotFound);
                return;
            }


            var user = await BotDBContext.Instance.Users.FirstOrDefaultAsync(u => u.DiscordId == Context.User.Id);
            if (user == null) //It is a new User
            {

                var newUsersPlayer = new Model.UsersPlayers()
                {
                    User = new Model.User() { DiscordId = Context.User.Id, Name = Context.User.Username },
                    Players = new List<Model.Player>
            {
                new PubgStatsBot.Model.Player() { PubgID = player.Id }
            }
                };

                await BotDBContext.Instance.UsersPlayers.AddAsync(newUsersPlayer);
                await BotDBContext.Instance.SaveChangesAsync();
                await ReplyAsync(Strings.PlayerAddedToWatch);
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
                        await ReplyAsync(Strings.PlayerAddedToWatch);
                    }
                    else
                    {
                        await ReplyAsync(Strings.PlayerIsAlreadySaved);
                    }
                }
            }

        }

        [Command("removewatch")]

        public async Task RemoveWatch([Remainder] string playersName = null)
        {
            if (playersName == null)
            {
                await ReplyAsync(Strings.PlayerNameRequired);
                return;
            }

            var player = await PubgHelper.GetPlayerByName(playersName);
            if (player == null)
            {
                await ReplyAsync(Strings.PlayerNotFound);
                return;
            }


            var user = await BotDBContext.Instance.Users.FirstOrDefaultAsync(u => u.DiscordId == Context.User.Id);
            if (user == null) //It is a new User
            {
                await ReplyAsync(Strings.YouHaveNoWatches);
                return;
            }
            else
            {
                var usersPlayers = await BotDBContext.Instance.UsersPlayers.Where(u => u.UserID == user.ID).Include(p => p.Players).FirstOrDefaultAsync();
                if (usersPlayers == null)
                {
                    await ReplyAsync(Strings.PlayerIsNotInYourWatch);
                    return;
                }
                var players = usersPlayers.Players.ToList();
                var playerInList = players.FirstOrDefault(p => p.PubgID == player.Id);
                if (playerInList != null)
                    players.Remove(playerInList);

                usersPlayers.Players = players;
                BotDBContext.Instance.UsersPlayers.Update(usersPlayers);
                await BotDBContext.Instance.SaveChangesAsync();
                await ReplyAsync(Strings.WatchRemovedSuccess);
            }

        }
        [Command("mywatch")]
        public async Task MyWatch()
        {
            var discordUser = Context.User;
            var user = await BotDBContext.Instance.Users.FirstOrDefaultAsync(u => u.DiscordId == discordUser.Id);
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
                    await ReplyAsync(builder.ToString());
                }
                else
                {
                    await ReplyAsync(Strings.NoWatchFound);
                }
            }
            else
            {
                await ReplyAsync(Strings.NoWatchFound);
            }
        }
    }

}

