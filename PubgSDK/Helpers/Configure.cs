using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Pubg.Net;
using System;
using System.Collections.Generic;
using System.Text;

namespace PubgSDK.Helpers
{
    public class Configure
    {
        public Configure()
        {
            PubgApiConfiguration.Configure(opt =>
            {
                opt.ApiKey = Credentials.PubgToken;
            });

            using (var con = new PubgDB())
            {

                if (!con.Database.GetService<IRelationalDatabaseCreator>().Exists())
                {

                    // Create the Db if it doesn't exist and applies any pending migration.
                    con.Database.Migrate();
                } 
            }
        }
    }
}