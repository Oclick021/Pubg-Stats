using Discord;
using Discord.Commands;
using Microsoft.EntityFrameworkCore;
using PubgSDK.Helpers;
using PubgSDK.Repository;
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
        private readonly PubgDB pubgDB;
        private readonly BotDBContext botDB;

        public PubgModule()
        {
            pubgDB = new PubgDB();
            botDB = new BotDBContext();
        }


        [Command("stats")]
        [Alias("allStats", "myStats")]
        public async Task AllStats([Remainder] string playersName = null)
        {
            if (playersName == null)
            {
                await ReplyAsync(Strings.PlayerNameRequired);
                return;
            }
            var playerRep = new PlayerRepository();
            await playerRep.GetPlayerByName(playersName);

            if (playerRep.Player == null)
            {
                await ReplyAsync(Strings.PlayerNotFound);
                return;
            }
            await playerRep.GetPlayerStats();
            await ReplyAsync(embed: EmbedHelper.GetStats($"{playerRep.Player.Name}'s Solo FPP", playerRep.Player.SoloStats, Color.Red));
            await ReplyAsync(embed: EmbedHelper.GetStats($"{playerRep.Player.Name}'s Duo FPP", playerRep.Player.DuoStats, Color.Blue));
            await ReplyAsync(embed: EmbedHelper.GetStats($"{playerRep.Player.Name}'s Squad FPP", playerRep.Player.SquadStats, Color.Teal));
        }
        [Command("solo")]
        [Alias("soloStats", "mysolo")]
        public async Task SoloStats([Remainder] string playersName = null)
        {
            try
            {
                if (playersName == null)
                {
                    await ReplyAsync(Strings.PlayerNameRequired);
                    return;
                }
                var playerRep = new PlayerRepository();
                await playerRep.GetPlayerByName(playersName);

                if (playerRep.Player == null)
                {
                    await ReplyAsync(Strings.PlayerNotFound);
                    return;
                }
                await playerRep.GetPlayerStats();
                await ReplyAsync(embed: EmbedHelper.GetStats($"{playerRep.Player.Name}'s Solo FPP", playerRep.Player.SoloStats, Color.Red));
            }
            catch (Exception e )
            {
                Console.WriteLine(e.ToString());
                throw;
            }
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
            var playerRep = new PlayerRepository();
            await playerRep.GetPlayerByName(playersName);

            if (playerRep.Player == null)
            {
                await ReplyAsync(Strings.PlayerNotFound);
                return;
            }
            await playerRep.GetPlayerStats();
            await ReplyAsync(embed: EmbedHelper.GetStats($"{playerRep.Player.Name}'s Duo FPP", playerRep.Player.DuoStats, Color.Blue));
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
            var playerRep = new PlayerRepository();
            await playerRep.GetPlayerByName(playersName);

            if (playerRep.Player == null)
            {
                await ReplyAsync(Strings.PlayerNotFound);
                return;
            }
            await playerRep.GetPlayerStats();
            await ReplyAsync(embed: EmbedHelper.GetStats($"{playerRep.Player.Name}'s Squad FPP", playerRep.Player.SquadStats, Color.Teal));
        }

        [Command("Compare")]
        public async Task CompareStats(params string[] Players)
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
                    var playerRep = new PlayerRepository(player);
                    await playerRep.GetPlayerStats();
                    player.DuoStats = playerRep.Player.DuoStats;
                    player.SquadStats = playerRep.Player.SquadStats;
                    player.SoloStats = playerRep.Player.SoloStats;
                }
                await ReplyAsync(embed: EmbedHelper.GetCompare($"Compare", players, Color.Red));
            }
        }


        [Command("recents")]
        [Alias("recent", "matches", "last", "match")]

        public async Task Recents([Remainder] string playersName = null)
        {
            try
            {
                if (playersName == null)
                {
                    await ReplyAsync(Strings.PlayerNameRequired);
                    return;
                }

                var playerRep = new PlayerRepository();
                await playerRep.GetPlayerByName(playersName);
                if (playerRep.Player == null)
                {
                    await ReplyAsync(Strings.PlayerNotFound);
                    return;
                }
                await ReplyAsync(embed: EmbedHelper.GetMatches($"{playerRep.Player.Name}'s Recent matches", playerRep.Player, Color.Red));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }


        }

        [Command("addwatch")]

        public async Task AddWatch([Remainder] string playersName = null)
        {
            if (playersName == null)
            {
                await ReplyAsync(Strings.PlayerNameRequired);
                return;
            }
            var playerRep = new PlayerRepository();
            await playerRep.GetPlayerByName(playersName);
            if (playerRep.Player == null)
            {
                await ReplyAsync(Strings.PlayerNotFound);
                return;
            }

            using (var context = new BotDBContext())
            {

                var user = await context.Users.FirstOrDefaultAsync(u => u.DiscordId == Context.User.Id);
                if (user == null) //It is a new User
                {

                    var newUsersPlayer = new Model.UsersPlayers()
                    {
                        User = new Model.User() { DiscordId = Context.User.Id, Name = Context.User.Username },
                        Players = new List<Model.Player>
                    {
                        new Model.Player() { PubgID = playerRep.Player.Id }
                    }
                    };

                    await context.UsersPlayers.AddAsync(newUsersPlayer);
                    await context.SaveChangesAsync();
                    await ReplyAsync(Strings.PlayerAddedToWatch);
                }
                else
                {
                    var usersPlayers = await context.UsersPlayers.Where(u => u.UserID == user.ID).Include(p => p.Players).FirstOrDefaultAsync();
                    if (usersPlayers == null)
                    {
                        usersPlayers = new Model.UsersPlayers() { User = user, Players = new List<Model.Player>() };
                    }
                    if (usersPlayers != null)
                    {

                        var playersList = usersPlayers.Players.ToList();
                        if (playersList.FirstOrDefault(a => a.PubgID == playerRep.Player.Id) == null)
                        {
                            playersList.Add(new Model.Player() { PubgID = playerRep.Player.Id });
                            usersPlayers.Players = playersList;
                            context.Update(usersPlayers);
                            await context.SaveChangesAsync();
                            await ReplyAsync(Strings.PlayerAddedToWatch);
                        }
                        else
                        {
                            await ReplyAsync(Strings.PlayerIsAlreadySaved);
                        }
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
            var playerRep = new PlayerRepository();
            await playerRep.GetPlayerByName(playersName);
            if (playerRep.Player == null)
            {
                await ReplyAsync(Strings.PlayerNotFound);
                return;
            }


            using (var context = new BotDBContext())
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.DiscordId == Context.User.Id);
                if (user == null) //It is a new User
                {
                    await ReplyAsync(Strings.YouHaveNoWatches);
                    return;
                }
                else
                {
                    var usersPlayers = await context.UsersPlayers.Where(u => u.UserID == user.ID).Include(p => p.Players).FirstOrDefaultAsync();
                    if (usersPlayers == null)
                    {
                        await ReplyAsync(Strings.PlayerIsNotInYourWatch);
                        return;
                    }
                    var players = usersPlayers.Players.ToList();
                    var playerInList = players.FirstOrDefault(p => p.PubgID == playerRep.Player.Id);
                    if (playerInList != null)
                        players.Remove(playerInList);

                    usersPlayers.Players = players;
                    context.UsersPlayers.Update(usersPlayers);
                    await context.SaveChangesAsync();
                    await ReplyAsync(Strings.WatchRemovedSuccess);
                }
            }

        }
        [Command("mywatch")]
        public async Task MyWatch()
        {
            var discordUser = Context.User;
            using (var context = new BotDBContext())
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.DiscordId == discordUser.Id);
                if (user != null)
                {
                    var usersPlayers = await context.UsersPlayers.Where(u => u.UserID == user.ID).Include(p => p.Players).FirstOrDefaultAsync();
                    if (usersPlayers != null)
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.AppendLine(Strings.YourPlayers);
                        foreach (var player in usersPlayers.Players)
                        {

                            using (var con = new PubgDB())
                            {
                                var playerInDB = await con.Players.FirstOrDefaultAsync(p => p.Id == player.PubgID);
                                builder.AppendLine(playerInDB.Name);
                            }
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

        [Command("lastMatch")]
        public async Task LastMatch()
        {
            var discordUser = Context.User;
            using (var context = new BotDBContext())
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.DiscordId == discordUser.Id);
                if (user != null)
                {
                    var usersPlayers = await context.UsersPlayers.Where(u => u.UserID == user.ID).Include(p => p.Players).FirstOrDefaultAsync();
                    if (usersPlayers != null)
                    {
                        StringBuilder builder = new StringBuilder();
                        builder.AppendLine(Strings.YourPlayers);
                        foreach (var player in usersPlayers.Players)
                        {
                            using (var con = new PubgDB())
                            {
                                var playerInDB = await con.Players.FirstOrDefaultAsync(p => p.Id == player.PubgID);
                                builder.AppendLine(playerInDB.Name);
                            }
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

}

