using System;
using System.Collections.Generic;
using System.Text;

namespace PubgSDK.Helpers
{
    public class Config
    {
        public static string PubgToken { get; set; }
        public static int PlayerRefreshTime { get; set; } = 10;
        public static int WatcherDelay { get; set; } = 3;
        public static int NumberOfRecentMatches { get; set; } = 10;


    }
}
