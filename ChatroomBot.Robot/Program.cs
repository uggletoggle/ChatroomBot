using Microsoft.Extensions.Configuration;
using System;

namespace ChatroomBot.Robot
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json")
             .Build();

            using (var listener = new MessageBusClient(config))
            {
                Console.WriteLine("Robot Started listening...");
                Console.ReadKey();
            }
        }
    }
}
