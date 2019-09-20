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

        public Config()
        {
         
        }
        public static bool Validate()
        {
            string location = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.json");

            new Configure();
            if (File.Exists(location))
            {
                Instance = JsonConvert.DeserializeObject<Config>(File.ReadAllText(location));
                Credentials.DiscordToken = Instance.DiscordToken;
                PubgSDK.Helpers.Credentials.PubgToken = Instance.PubgToken;
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
