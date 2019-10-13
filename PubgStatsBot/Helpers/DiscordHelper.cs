using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PubgStatsBot.Helpers
{
    public class DiscordHelper
    {
        public DiscordSocketClient Client { get; set; }
        public DiscordHelper(DiscordSocketClient client)
        {
            Client = client;
        }


        public IEnumerable<SocketGuildUser> GetAllUsers()
        {
            var users = new List<SocketGuildUser>();
            foreach (var guild in Client.Guilds)
            {
                users.AddRange(guild.Users);
            }
            return users.GroupBy(x => x.Id).Select(x => x.First());
        }

        public void UpdateUsersStatus()
        {

        }

    }
}
