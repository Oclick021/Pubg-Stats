using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Discord;
using PubgSDK.Helpers;
using PubgStatsBot.Helpers;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;

namespace PubgStatsBot.Modules
{
    [IsOwner]
    public class AdminModule : ModuleBase<SocketCommandContext>
    {

        [Command("OnlineUsers")]
        [Alias("Online", "Users", "کاربران")]
        public async Task OnlineUsers()
        {
            var discordHelper = new DiscordHelper(Context.Client);
            var users = discordHelper.GetAllUsers();

            await ReplyAsync(
                $"Total: {users.Count()} \n" +
                $"Online:{users.Where(u => u.Status == UserStatus.Online).Count()}\n" +
                $"Offline: {users.Where(u => u.Status == UserStatus.Offline).Count()}\n" +
                $"Idle: {users.Where(u => u.Status == UserStatus.Idle).Count()}\n" +
                $"DoNotDistorb: {users.Where(u => u.Status == UserStatus.DoNotDisturb).Count()}\n");
        }

        [Command("Servers")]
        [Alias("AllServers", "سرور")]
        public async Task Servers()
        {

            string[] ServerList = (from guild in Context.Client.Guilds
                                   select $"{guild.Name} Owned by: {guild.Owner.Username}").ToArray();
            await ReplyAsync($"Total servers: {ServerList.Length} \n" + string.Join("\n", ServerList));
        }

        [Command("status")]
        [Alias("وضعیت")]
        public async Task Status()
        {

            string[] ServerList = (from guild in Context.Client.Guilds
                                   select $"{guild.Name} Owned by: {guild.Owner.Username}").ToArray();
            var discordHelper = new DiscordHelper(Context.Client);
            var users = discordHelper.GetAllUsers();
            int watchUsers = 0;
            int totalRequests = 0;
            using (var con = new BotDBContext())
            {
                watchUsers = con.Users.Count();
                totalRequests = con.Logs.Count();
            }

            string msg =
                $"Servers: {ServerList.Length} \n" +
                $"Users: {users.Count()}\n" +
                $"Watchers: {watchUsers}\n" +
                $"Requests: {totalRequests}\n";

            await ReplyAsync(msg);

        }

        [Command("log")]
        [Alias("گزارش")]
        public async Task log([Remainder] int recentsLogs)
        {

            using (var con = new BotDBContext())
            {
                var logs = con.Logs.Include(l => l.User).OrderByDescending(x => x.Date);
                int count = 0;
                StringBuilder builder = new StringBuilder();
                foreach (var log in logs)
                {
                    count++;
                    if (builder.Length > 1900)
                    {
                        await ReplyAsync(builder.ToString());
                        builder = new StringBuilder();
                    }
                    builder.AppendLine($"{count}. {log.Date}|   {log?.User?.Name}          | {log.Content}");
                    if (count > recentsLogs)
                        break;
                }
                await ReplyAsync(builder.ToString());

            }
        }



        [Command("EchoHelp")]
        public async Task EchoOnline()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("!EchoHelp");
            builder.AppendLine("!EchoOnline");
            builder.AppendLine("!EchoAll");
            builder.AppendLine("!EchoWatchers");
            await ReplyAsync(builder.ToString());

        }

        [Command("EchoOnline")]
        public async Task EchoOnline([Remainder] string message)
        {
            var discordHelper = new DiscordHelper(Context.Client);
            var onlineUsers = discordHelper.GetAllUsers().Where(u => u.Status == UserStatus.Online);
            await SendMessageToPlayers(onlineUsers, message);
        }

        [Command("EchoAll")]
        public async Task EchoAll(string message, [Remainder] string password)
        {
            if (password == "Prohumper" && message.Trim() != "")
            {
                var discordHelper = new DiscordHelper(Context.Client);
                var onlineUsers = discordHelper.GetAllUsers();
                await SendMessageToPlayers(onlineUsers, message);
            }
        }


        [Command("EchoWatchers")]
        public async Task EchoWatchers([Remainder] string message)
        {
            using (var con = new BotDBContext())
            {
                var watchers = new List<IUser>();
                var discordHelper = new DiscordHelper(Context.Client);
                var onlineUsers = discordHelper.GetAllUsers();
                foreach (var user in onlineUsers)
                {
                    var userInDb = con.UsersPlayers.Include(u => u.User).ToList();
                    if (userInDb.Any(u => u.User.DiscordId == user.Id))
                    {
                        watchers.Add(user);
                    }
                }
                await SendMessageToPlayers(watchers, message);
            }
        }

        async Task SendMessageToPlayers(IEnumerable<IUser> users, string message)
        {
            foreach (var user in users)
            {
                await Discord.UserExtensions.SendMessageAsync(user, message);
            }
            var us = await Context.Client.Rest.GetUserAsync(Credentials.UserID);
            await ReplyAsync(Strings.EchoSent + $" {users.Count()} are notified!");
        }
    }
}
