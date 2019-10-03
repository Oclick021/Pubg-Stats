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
                if (!con.Database.GetService<IRelationalDatabaseCreator>().Exists())
                {

                    // Create the Db if it doesn't exist and applies any pending migration.
                    con.Database.Migrate();
                }
            }
            new PubgSDK.Helpers.Configure();

        }
    }
}
