using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PubgStatsBot.Helpers
{
    public static class Watcher
    {
        public static Task StartWatch(SocketMessage message)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await message.Channel.SendMessageAsync(message.Author.Username);
                    await Task.Delay(3000);
                }
            });
            return null;
        }
    }
}
