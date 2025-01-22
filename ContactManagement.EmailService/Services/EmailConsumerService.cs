using ContactManagement.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.EmailService.Services
{

    public class EmailConsumerService : BackgroundService
    {
        private readonly ILogger<EmailConsumerService> _logger;
        private readonly EmailService _emailService;
        private readonly RabbitMQSettings _rabbitMQSettings;
        private IConnection _connection;
        private IModel _channel;

        public EmailConsumerService(
            IOptions<RabbitMQSettings> rabbitMQSettings,
            EmailService emailService,
            ILogger<EmailConsumerService> logger)
        {
            _rabbitMQSettings = rabbitMQSettings.Value;
            _emailService = emailService;
            _logger = logger;
            InitializeRabbitMQ();
        }

        private void InitializeRabbitMQ()
        {
            //var factory = new ConnectionFactory
            //{
            //    HostName = _rabbitMQSettings.HostName,
            //    UserName = _rabbitMQSettings.UserName,
            //    Password = _rabbitMQSettings.Password,
            //    VirtualHost = _rabbitMQSettings.VirtualHost
            //};

            //_connection = factory.CreateConnection();
            //_channel = _connection.CreateModel();

            //_channel.ExchangeDeclare("email_exchange", ExchangeType.Direct);
            //_channel.QueueDeclare("email_queue", true, false, false, null);
            //_channel.QueueBind("email_queue", "email_exchange", "email_routing_key");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //var consumer = new EventingBasicConsumer(_channel);

            //consumer.Received += async (model, ea) =>
            //{
            //    var body = ea.Body.ToArray();
            //    var message = JsonSerializer.Deserialize<EmailMessage>(
            //        Encoding.UTF8.GetString(body));

            //    try
            //    {
            //        await _emailService.SendEmailAsync(
            //            message.To,
            //            message.Subject,
            //            message.Body,
            //            message.IsHtml);

            //        _channel.BasicAck(ea.DeliveryTag, false);
            //    }
            //    catch (Exception ex)
            //    {
            //        _logger.LogError(ex, "Error processing email message");
            //        _channel.BasicNack(ea.DeliveryTag, false, true);
            //    }
            //};

            //_channel.BasicConsume(queue: "email_queue",
            //                    autoAck: false,
            //                    consumer: consumer);

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            //_channel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }
    }
}
