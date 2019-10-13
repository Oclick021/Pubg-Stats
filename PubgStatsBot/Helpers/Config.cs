using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PubgSDK.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PubgStatsBot.Helpers
{
  public class Config
    {
        public static Config Instance { get; set; }

        public string PubgToken { get; set; }
        public string DiscordToken { get; set; }
        public int PlayerRefreshTime { get; set; } = 10;
        public int WatcherDelay { get; set; } = 3;
        public int NumberOfRecentMatches { get; set; } = 10;
        public int WinPlace { get; set; } = 15;
        public int WatchRecentHours { get; set; } = 3;
        public Config()
        {
         
        }
        public static bool Validate()
        {
            Console.WriteLine("Validating...");
            string location = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

            new Configure();
            if (File.Exists(location))
            {
                Instance = JsonConvert.DeserializeObject<Config>(File.ReadAllText(location));
                Credentials.DiscordToken = Instance.DiscordToken;
                PubgSDK.Helpers.Config.PubgToken = Instance.PubgToken;
                PubgSDK.Helpers.Config.PlayerRefreshTime = Instance.PlayerRefreshTime;
                PubgSDK.Helpers.Config.WatcherDelay = Instance.WatcherDelay;
                PubgSDK.Helpers.Config.NumberOfRecentMatches = Instance.NumberOfRecentMatches;
                Console.WriteLine("Validated");
                return true;
            }
            else
            {
                Console.WriteLine("ConfigFile is not found.");
                Console.WriteLine("It will be created by the app.");
                Console.WriteLine("Please fill in the tokens.");
                Instance = new Config();
                File.WriteAllText(location, JsonConvert.SerializeObject(Instance));
                return false;
            }





        }
    }
}
