using NATS.Client;
using System;
using System.Text;

namespace Requester
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConnectionFactory cf = new ConnectionFactory();
            Options opts = ConnectionFactory.GetDefaultOptions();
            opts.Url = "nats://host.docker.internal:4222";
            opts.Timeout = 15500;
            IConnection c = cf.CreateConnection(opts);
            while (true)
            {

                Console.WriteLine("Channel: ");
                string channel = Console.ReadLine();
                Console.WriteLine("Message: ");
                string message = Console.ReadLine();
                Console.WriteLine("Times to be sent: ");
                string timesString = Console.ReadLine();
                int times;
                while (true)
                {

                    try
                    {
                        times = Convert.ToInt32(timesString);
                        break;

                    }
                    catch (Exception)
                    {
                        Console.WriteLine("Please specify a number.");
                        timesString = Console.ReadLine();
                    }
                }
                RequestDTO request = new RequestDTO("RequestConsole", message, channel);
                Console.WriteLine($"Sent {message} ");
                var responseData = c.Request(channel, Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(request)));
                var receivedOrder = Encoding.UTF8.GetString(responseData.Data);
                Console.WriteLine(receivedOrder);


            }
        }
    }
}
