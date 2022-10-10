using Auto.Messages;
using EasyNetQ;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Auto.AuditLog
{

    class Program
    {
        private static readonly IConfigurationRoot config = ReadConfiguration();

        private const string SUBSCRIBER_ID = "Auto.AuditLog";

        static async Task Main(string[] args)
        {
            using var bus = RabbitHutch.CreateBus(config.GetConnectionString("AutoRabbitMQ"));
            Console.WriteLine("Connected! Listening for NewVehicleMessage messages.");
            await bus.PubSub.SubscribeAsync<NewVehicleMessage>(SUBSCRIBER_ID, HandleNewVehicleMessage);
            Console.ReadKey(true);
        }

        private static void HandleNewVehicleMessage(NewVehicleMessage message)
        {
            var csv =
                $"{message.Registration},{message.Manufacturer},{message.ModelName},{message.Color},{message.Year},{message.ListedAtUtc:O}";
            Console.WriteLine(csv);
        }

        private static IConfigurationRoot ReadConfiguration()
        {
            var basePath = Directory.GetParent(AppContext.BaseDirectory).FullName;
            return new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }
    }
}