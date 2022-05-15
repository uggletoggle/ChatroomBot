using ChatroomBot.Robot.Messages;
using FileHelpers;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace ChatroomBot.Robot
{
    public class MessageBusClient : IMessageBusClient, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly EventingBasicConsumer _consumer;
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

                    try
                    {
                        var response = await client.GetAsync($"?s={stockCode}&f=sd2t2ohlcv&h&e=csv");
                        var file = await response.Content.ReadAsStringAsync();
                        var fileHelperEngine = new FileHelperEngine<Record>();

                        var records = fileHelperEngine.ReadString(file);

                        foreach (var record in records)
                        {
                            if (double.TryParse(record.Close.Replace('.', ','), out double close))
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
                                    Symbol = record.Symbol
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        PublishRobotWorkerError(new RobotWorkerErrorMessage
                        {
                            ExceptionType = ex.GetType().Name,
                            Description = ex.Message
                        });
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

        private void PublishRobotWorkerError(RobotWorkerErrorMessage message)
        {
            var jsonMessage = JsonSerializer.Serialize(message);

            if (_connection.IsOpen)
            {
                SendMessage(jsonMessage);
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

            _channel.BasicPublish("", "chatroom", null, body);

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
