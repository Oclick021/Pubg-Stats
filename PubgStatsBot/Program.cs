﻿using Discord;
using Discord.WebSocket;
using PubgSDK.Extentions;
using PubgStatsBot.Helpers;
using System;
using System.Threading.Tasks;

namespace PubgStatsBot
{
  public  class Program
    {
        private readonly DiscordSocketClient _client;

        // Discord.Net heavily utilizes TAP for async, so we create
        // an asynchronous context from the beginning.
        static void Main(string[] args)
        {
            new Program()
                .MainAsync()
                .GetAwaiter()
                .GetResult();
        }



        public Program()
        {

            // It is recommended to Dispose of a client when you are finished
            // using it, at the end of your app's lifetime.
            _client = new DiscordSocketClient();

            _client.Log += LogAsync;
            _client.Ready += ReadyAsync;
            _client.MessageReceived += MessageReceivedAsync;
        }


        public async Task MainAsync()
        {
            // Tokens should be considered secret data, and never hard-coded.
            await _client.LoginAsync(TokenType.Bot, Credentials.DiscordToken);
            await _client.StartAsync();

            // Block the program until it is closed.
            await Task.Delay(-1);
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        // The Ready event indicates that the client has opened a
        // connection and it is now safe to access the cache.
        private Task ReadyAsync()
        {
            Console.WriteLine($"{_client.CurrentUser} is connected!");

            return Task.CompletedTask;
        }

        // This is not the recommended way to write a bot - consider
        // reading over the Commands Framework sample.
        private async Task MessageReceivedAsync(SocketMessage message)
        {
            if (message.Content.ToLower().StartsWith("!ping"))
            {
                await message.Channel.SendMessageAsync("pong!");
            }
            var messageHelper = new MessageHelper(_client, message);
            // The bot should never respond to itself.
            if (message.Author.Id == _client.CurrentUser.Id)
                return;
            Console.WriteLine($"{message.Author.Username} Requested!");

            if (await messageHelper.IsAuthorized())
            {
                if (message.Content.EqualsAnyOf("!help", "!Help", "!HELP"))
                {
                    await message.Channel.SendMessageAsync(embed: EmbedHelper.GetHelp());
                }


                if (message.Content.ToLower().StartsWith("!stats"))
                {
                    await messageHelper.GetStats(message);
                }

                if (message.Content.ToLower().StartsWith("!compare"))
                {
                    await messageHelper.GetStatsCompare(message);
                }
                if (message.Content.ToLower().StartsWith("!recents"))
                {
                    await messageHelper.GetMatchStats(message);
                }
            }
        }



    }
}