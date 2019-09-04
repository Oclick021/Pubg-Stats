using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;

namespace PubgStatsBot.Helpers
{
   public static class UserHelper
    {
        public static bool ChannelHasUser(this SocketChannel server, SocketUser user)
        {
            if (server.GetUser(user.Id) != null)
            {
                return true;
            }    
            return false;
        }

    }
}
