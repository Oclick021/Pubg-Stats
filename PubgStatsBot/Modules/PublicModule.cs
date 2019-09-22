using Discord;
using Discord.Commands;
using PubgStatsBot.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PubgStatsBot.Modules
{
    public class PublicModule : ModuleBase<SocketCommandContext>
    {

        [Command("help")]
        [Alias("hi", "help", "راهنما")]
        public async Task Help() => await ReplyAsync(embed: EmbedHelper.GetHelp());

        [Command("invite")]
        [Alias("link", "share", "دعوت")]
        public async Task Invite() => await ReplyAsync(@"https://discordapp.com/api/oauth2/authorize?client_id=605829109335064593&permissions=0&scope=bot");
    }
}
