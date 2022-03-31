using account_microservice.Context;
using account_microservice.Models;
using account_microservice.Tools;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;

namespace account_microservice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                             .AddJsonFile($"appsettings.json", false);
            var config = builder.Build();
            var salt = config["Salt"];
            PasswordHasher hasher = new PasswordHasher(salt);
            Console.WriteLine("Worker:");
            ConnectionFactory cf = new ConnectionFactory();
            Options opts = ConnectionFactory.GetDefaultOptions();
            opts.Url = "nats://host.docker.internal:4222";
            opts.Timeout = 15500;
            IConnection c = cf.CreateConnection(opts);
            UserContext userContext = new UserContext();
            userContext.Database.EnsureCreated();

            EventHandler<MsgHandlerEventArgs> h = (sender, args) =>
            {
                string receivedMessage = System.Text.Encoding.UTF8.GetString(args.Message.Data);
                var deserializedMessage = (JObject)JsonConvert.DeserializeObject(receivedMessage);
                var decodedMessage = deserializedMessage.SelectToken("Message").ToString();
                Console.WriteLine($"Got message: {decodedMessage}");
                var reply = args.Message.Reply;
                var replyMessage = Encoding.UTF8.GetBytes("Replied");
                c.Publish(reply, replyMessage);
            };


            IAsyncSubscription s = c.SubscribeAsync("account", "load-balancing-queue-account", h);


            int firstRun = 1;
            while (true)
            {
                if (firstRun == 1)
                {
                    Console.WriteLine("Account Service Started");
                    firstRun = 0;
                }
            }
        }
    }
    
}
