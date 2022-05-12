using FileHelpers;
using IdentityModel;
using IdentityModel.Client;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChatroomBot.Robot
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            //if (args.Length > 0 && args.ToList().Contains("--stock"))
            //{
            var client = new HttpClient();

            //Task.Delay(5000).Wait();
            //var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            //{
            //    Address = "https://localhost:44390/connect/token",
            //    ClientId = "m2m.client",
            //    ClientSecret = "secret",
            //    Scope = "Chatroom.API"
            //});


            //if (tokenResponse.IsError)
            //{
            //    Console.WriteLine(tokenResponse.Error);
            //    return;
            //}

            //Console.WriteLine(tokenResponse.Json);


            client.BaseAddress = new Uri("https://stooq.com/q/l/");

            string stockCode = "appl.us";

            var response = await client.GetAsync($"?s={stockCode}&f=sd2t2ohlcv&h&e=csv");

            var file = await response.Content.ReadAsStringAsync();

            var fileHelperEngine = new FileHelperEngine<Record>();
            var records = fileHelperEngine.ReadString(file);

            foreach (var record in records)
            {
                Console.WriteLine(record);
            }
        }
    }

    [DelimitedRecord(",")]
    [IgnoreFirst(1)]
    internal class Record 
    {
        public string Symbol { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Open { get; set; }
        public string High { get; set; }
        public string Low { get; set; }
        public string Close { get; set; }
        public string Volume { get; set; }
    }
}
