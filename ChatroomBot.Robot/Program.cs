using FileHelpers;
using IdentityModel;
using IdentityModel.Client;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ChatroomBot.Robot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //if (args.Length > 0 && args.ToList().Contains("--stock"))
            //{
            IConfiguration config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();


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




            var messageBusClient = new MessageBusClient(config);
            //messageBusClient.PublishCurrentRecordValue( new CurrentRecordValueMessage 
            //    {
            //        Close = "$93.44",
            //        Symbol = "APPL.US"
            //    });


            Console.ReadKey();
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

    public interface IMessageBusClient
    {
        void PublishCurrentRecordValue(CurrentRecordValueMessage message); 
    }

    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly EventingBasicConsumer _consumer;
        private readonly string _queueName;
        private IModel _consumptionChannel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.QueueDeclare("chatroom",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                _consumptionChannel = _connection.CreateModel();

                _channel.QueueDeclare("robot",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                _consumer = new EventingBasicConsumer(_consumptionChannel);

                _consumer.Received += async (model, ea) =>
                {
                    var client = new HttpClient();

                    client.BaseAddress = new Uri("https://stooq.com/q/l/");

                    var body = ea.Body;
                    var stockCode = Encoding.UTF8.GetString(body.Span);

                    var response = await client.GetAsync($"?s={stockCode}&f=sd2t2ohlcv&h&e=csv");


                    var file = await response.Content.ReadAsStringAsync();

                    var fileHelperEngine = new FileHelperEngine<Record>();
                    var records = fileHelperEngine.ReadString(file);

                    foreach (var record in records)
                    {
                        if (double.TryParse(record.Close, out double close))
                        {
                            PublishCurrentRecordValue(new CurrentRecordValueMessage
                            {
                                Close = close,
                                Symbol = record.Symbol
                            });
                        } 
                        else
                        {
                            PublishNotFoundValue(new NotFoundValueMessage 
                            {
                                symbol = record.Symbol
                            });
                        }
                    }
                };

                _channel.BasicConsume(queue: "robot", autoAck: true, consumer: _consumer);

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutDown;

                Console.WriteLine("--> Connected to Rabbit MQ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Connection to Rabbit MQ failed with error: {ex.Message}");
            }
        }

        private void PublishNotFoundValue(NotFoundValueMessage message)
        {
            var jsonMessage = JsonSerializer.Serialize(message);

            if (_connection.IsOpen)
            {
                SendMessage(jsonMessage);
            }
        }

        public void PublishCurrentRecordValue(CurrentRecordValueMessage message)
        {
            var jsonMessage = JsonSerializer.Serialize(message);

            if (_connection.IsOpen)
            {
                SendMessage(jsonMessage);
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish("", "chatroom", null,body);

            Console.WriteLine($"--> New message sended to {"chatroom"}");
        }

        private void RabbitMQ_ConnectionShutDown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> Connection with Rabbit MQ shutdown");
        }

        public void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }

}
