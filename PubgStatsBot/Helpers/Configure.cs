using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using PubgSDK.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace PubgStatsBot.Helpers
{
    public class Configure
    {
        public Configure()
        {
            using (var con = new BotDBContext())
            {
                try
                {
                    con.Database.Migrate();

                }
                catch (Exception)
                {

                }
            }
            new PubgSDK.Helpers.Configure();

        }
    }
}
