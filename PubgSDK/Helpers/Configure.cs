using Pubg.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace PubgSDK.Helpers
{
    class Configure
    {
        public Configure()
        {
            PubgApiConfiguration.Configure(opt =>
            {
                opt.ApiKey = Credentials.PubgToken;
            });
        }
    }
}
