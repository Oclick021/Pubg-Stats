﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using PubgSDK.Extentions;
using PubgStatsBot.Helpers;
using PubgStatsBot.Services;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PubgStatsBot
{
    class Program
    {
        // There is no need to implement IDisposable like before as we are
        // using dependency injection, which handles calling Dispose for us.
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            // You should dispose a service provider created using ASP.NET
            // when you are finished using it, at the end of your app's lifetime.
            // If you use another dependency injection framework, you should inspect
            // its documentation for the best way to do this.


            if (Config.Validate())
            {
                using (var services = ConfigureServices())
                {
                    var client = services.GetRequiredService<DiscordSocketClient>();

                    client.Log += LogAsync;
                    services.GetRequiredService<CommandService>().Log += LogAsync;

                    // Tokens should be considered secret data and never hard-coded.
                    // We can read from the environment variable to avoid hardcoding.
                    await client.LoginAsync(TokenType.Bot, Credentials.DiscordToken);

                    client.Connected += async () =>
                    {
                        //send message to client asynchronously
                        var guilds = await client.Rest.GetGuildsAsync();
                        await client.DownloadUsersAsync(guilds);
                        Console.WriteLine("Caching client... Please wait");
                        await Task.Delay(10000);
                        Console.WriteLine("Client Cached.");
                        Console.WriteLine("Watchers Starting...");
                        var watch = new Watcher(client);
                    };

                    await client.StartAsync();

                    var us = await client.Rest.GetUserAsync(Credentials.UserID);
                    await Discord.UserExtensions.SendMessageAsync(us, "bot is online");

                    // Here we initialize the logic required to register our commands.
                    await services.GetRequiredService<CommandHandlingService>().InitializeAsync();

                    await Task.Delay(-1);
                }
            }
        }

        private Task LogAsync(LogMessage log)
        {
            Console.WriteLine(log.ToString());
            return Task.CompletedTask;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandlingService>()
                .AddSingleton<HttpClient>()
                .BuildServiceProvider();
        }
    }
}
