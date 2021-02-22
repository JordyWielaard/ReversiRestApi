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

            Spel sp = new Spel
            {
                ID = 3,
                Token = Guid.NewGuid().ToString(),
                Speler1Token = Guid.NewGuid().ToString(),
                Speler2Token = Guid.NewGuid().ToString(),
                Omschrijving = "Test",
                AandeBeurt = Kleur.Wit,
            };

            SpelAccessLayer sal = new SpelAccessLayer();
            //sal.AddSpel(sp);
            Console.WriteLine(sal.GetSpel("DC645EB6-72FC-4298-9961-189014B80608").Omschrijving);
            for (int i = 0; i < sp.Bord.GetLength(0); i++)
            {
                for (int j = 0; j < sp.Bord.GetLength(1); j++)
                {
                    Console.WriteLine(sal.GetSpel("DC645EB6-72FC-4298-9961-189014B80608").Bord[i,j]);
                }
            }


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
