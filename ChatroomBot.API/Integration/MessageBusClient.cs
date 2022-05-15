using ChatroomBot.API.Models;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace ChatroomBot.API.Integration
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

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

                _channel.QueueDeclare("robot",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                Console.WriteLine("--> Connected to Rabbit MQ");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Connection to Rabbit MQ failed with error: {ex.Message}");
            }
        }

        public void PublishAskForStockMessage(AskForStockMessage message)
        {
            var stockCodeMessage = message.stock;

            if (_connection.IsOpen)
            {
                SendMessage(stockCodeMessage);
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish("", "robot", null, body);
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