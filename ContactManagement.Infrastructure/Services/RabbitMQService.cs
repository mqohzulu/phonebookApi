using ContactManagement.Application.Common.Interfaces;
using ContactManagement.Application.Common.Models;
using ContactManagement.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using IModel = RabbitMQ.Client.IChannel;

namespace ContactManagement.Infrastructure.Services
{
    public class RabbitMQService : IRabbitMQService, IDisposable
    {
        private IConnection? _connection;
        private IModel? _channel;
        private readonly ILogger<RabbitMQService> _logger;
        private const string ExchangeName = "contact_management";

        public RabbitMQService(IOptions<RabbitMQSettings> settings,ILogger<RabbitMQService> logger)
        {
            _logger = logger;
            InitializeAsync(settings.Value).GetAwaiter().GetResult();
        }
        private async Task InitializeAsync(RabbitMQSettings settings)
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = settings.HostName,
                    UserName = settings.UserName,
                    Password = settings.Password,
                    VirtualHost = settings.VirtualHost
                };

                // Create connection and channel asynchronously
                _connection = await factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();

                // Declare exchange
                await _channel.ExchangeDeclareAsync(
                   exchange: ExchangeName,
                   type: ExchangeType.Topic,
                   durable: true,
                   autoDelete: false,
                   arguments: null);

                // Declare and bind queues
                _channel.QueueDeclareAsync("email.welcome", true, false, false);
                _channel.QueueDeclareAsync("email.contact.created", true, false, false);
                _channel.QueueDeclareAsync("email.contact.updated", true, false, false);

                _channel.QueueBindAsync("email.welcome", ExchangeName, "email.welcome");
                _channel.QueueBindAsync("email.contact.created", ExchangeName, "email.contact.created");
                _channel.QueueBindAsync("email.contact.updated", ExchangeName, "email.contact.updated");

                _logger.LogInformation("RabbitMQ connection established successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initializing RabbitMQ connection");
                throw;
            }
        }

        public async Task PublishAsync<T>(string routingKey, T message) where T : class
        {
            try
            {
                if (_channel == null)
                {
                    throw new InvalidOperationException("RabbitMQ channel is not initialized");
                }

                var json = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);

                // Publish the message
                await Task.Run(() =>
                {
                    //_channel.BasicPublishAsync(
                    //      exchange: ExchangeName,
                    //        routingKey: routingKey,
                    //        mandatory: true,
                    //        basicProperties: null,  // Use default properties (null)
                    //        body: body);
                });

                _logger.LogInformation("Message published to {RoutingKey}", routingKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing message to {RoutingKey}", routingKey);
                throw;
            }
        }


        public async Task PublishEmailMessageAsync<T>(T message) where T : EmailMessage
        {
            var routingKey = message switch
            {
                WelcomeEmailMessage => "email.welcome",
                ContactCreatedEmailMessage => "email.contact.created",
                _ => throw new ArgumentException($"Unknown message type: {message.GetType().Name}")
            };

            await PublishAsync(routingKey, message);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
