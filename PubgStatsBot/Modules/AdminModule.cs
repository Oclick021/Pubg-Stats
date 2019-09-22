using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Discord;

namespace PubgStatsBot.Modules
{
    [IsOwner]
  public  class AdminModule : ModuleBase<SocketCommandContext>
    {


        [Command("OnlineUsers")]
        [Alias("Online", "Users", "کاربران")]
        public async Task OnlineUsers()
        {
            List<ulong> usersList = new List<ulong>();
            int online = 0;
            int offline = 0;
            int doNotDistrub = 0;
            int idle = 0;
            foreach (var user in from guild in Context.Client.Guilds
                                 from user in guild.Users
                                 where !usersList.Contains(user.Id)
                                 select user)
            {
                usersList.Add(user.Id);
                switch (user.Status)
                {
                    case Discord.UserStatus.Offline:
                        offline++;
                        break;
                    case Discord.UserStatus.Online:
                        online++;
                        break;
                    case Discord.UserStatus.Idle:
                        idle++;
                        break;
                    case Discord.UserStatus.DoNotDisturb:
                        doNotDistrub++;
                        break;
                    default:
                        break;
                }
            }
            await ReplyAsync(
                $"Total: {usersList.Count} \n" +
                $"Online: {online}\n" +
                $"Offline: {offline}\n" +
                $"Idle: {idle}\n" +
                $"DoNotDistorb: {doNotDistrub}\n");

        }

        [Command("Servers")]
        [Alias("AllServers", "سرور")]
        public async Task Servers()
        {

            string[] ServerList = (from guild in Context.Client.Guilds
                                   select $"{guild.Name} Owned by: {guild.Owner.Username}").ToArray();
                   await ReplyAsync($"Total servers: {ServerList.Length} \n" + string.Join("\n", ServerList));
        }

    }
}
