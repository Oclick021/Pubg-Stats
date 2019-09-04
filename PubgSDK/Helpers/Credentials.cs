using System;
using System.Collections.Generic;
using System.Text;

namespace PubgSDK.Helpers
{
    public class Credentials
    {
        private static string pubgToken;
        public static string PubgToken
        {
            get
            {
                if (pubgToken == null)
                {
                    pubgToken = Environment.GetEnvironmentVariable("pubgtoken", EnvironmentVariableTarget.User);
                }
                return pubgToken;
            }
            set => pubgToken = value;
        }
  


    }
}
