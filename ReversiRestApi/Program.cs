using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReversiRestApi.Controllers;
using ReversiRestApi.DAL;
using ReversiRestApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReversiRestApi
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Spel sp2 = new Spel
            {
                ID = 5,
                Token = Guid.NewGuid().ToString(),
                Speler1Token = Guid.NewGuid().ToString(),
                Omschrijving = "Geen Speler 2",
                AandeBeurt = Kleur.Wit,
            };
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
