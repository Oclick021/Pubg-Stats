using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using PubgSDK.Helpers;
using System.Linq;

namespace PubgStatsBot.Services
{
    public class CommandHandlingService
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;

        public CommandHandlingService(IServiceProvider services)
        {
            _commands = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            // Hook CommandExecuted to handle post-command-execution logic.
            _commands.CommandExecuted += CommandExecutedAsync;
            // Hook MessageReceived so we can process each message to see
            // if it qualifies as a command.
            _discord.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            // Register modules that are public and inherit ModuleBase<T>.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // Ignore system messages, or messages from other bots
            if (!(rawMessage is SocketUserMessage message))
                return;
            if (message.Source != MessageSource.User)
                return;

            // This value holds the offset where the prefix ends
            var argPos = 0;
            // Perform prefix check. You may want to replace this with
            if (!message.HasCharPrefix('!', ref argPos))
                return;


            // for a more traditional command format like !help.
            //if (!message.HasMentionPrefix(_discord.CurrentUser, ref argPos))
            //    return;

            var context = new SocketCommandContext(_discord, message);
            // Perform the execution of the command. In this method,
            // the command service will perform precondition and parsing check
            // then execute the command if one is matched.
            await _commands.ExecuteAsync(context, argPos, _services);
            // Note that normally a result will be returned by this format, but here
            // we will handle the result in CommandExecutedAsync,
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            await LogAction(context);

            // command is unspecified when there was a search failure (command not found); we don't care about these errors
            if (!command.IsSpecified)
                return;

            // the command was successful, we don't care about this result, unless we want to log that a command succeeded.
            if (result.IsSuccess)
            {
                return;
            }


            // the command failed, let's notify the user that something happened.
            await context.Channel.SendMessageAsync($"error: {result}");
        }
        async Task LogAction(ICommandContext context)
        {
            using (var con = new BotDBContext())
            {
                var userInDb = con.Users.FirstOrDefault(u => u.DiscordId == context.User.Id);
                if (userInDb == null)
                {
                    userInDb = new Model.User() { DiscordId = context.User.Id, Name = context.User.Username, UserTag = context.User.Discriminator };
                }
                Console.WriteLine($"User {context.User.Username} requested {context.Message.Content} ***** at {DateTime.Now.ToShortDateString()}");

                await con.Logs.AddAsync(new Model.Log() { Date = DateTime.Now, Content = context.Message.Content, User = userInDb });
                await con.SaveChangesAsync(); 
            }
        }
    }
}
