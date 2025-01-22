using ContactManagement.Application.Common.Interfaces;
using ContactManagement.EmailService.Services;
using ContactManagement.Infrastructure.Services;
using ContactManagement.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.EmailService.Worker
{


    public class EmailWorker : BackgroundService
    {
        private readonly ILogger<EmailWorker> _logger;
        private readonly IModel _channel;
        private readonly IConnection _connection;
        //private readonly EmailService _emailService;
        private const string ExchangeName = "contact_management";

        public EmailWorker(IOptions<RabbitMQSettings> rabbitMQSettings, ILogger<EmailWorker> logger)
        {
            //_emailService = emailService;
            //_logger = logger;

            //var factory = new ConnectionFactory
            //{
            //    HostName = rabbitMQSettings.Value.HostName,
            //    UserName = rabbitMQSettings.Value.UserName,
            //    Password = rabbitMQSettings.Value.Password,
            //    VirtualHost = rabbitMQSettings.Value.VirtualHost
            //};

            //_connection = factory.CreateConnection();
            //_channel = _connection.CreateModel();

            //SetupRabbitMQ();
        }

        private void SetupRabbitMQ()
        {
            //_channel.ExchangeDeclareAsycn(ExchangeName, ExchangeType.Topic, true);

            //// Declare Queues
            //_channel.QueueDeclare(
            //    queue: "email.welcome",
            //    durable: true,
            //    exclusive: false,
            //    autoDelete: false);

            //_channel.QueueDeclare(
            //    queue: "email.contact.created",
            //    durable: true,
            //    exclusive: false,
            //    autoDelete: false);

            //// Bind queues to exchange
            //_channel.QueueBind(
            //    queue: "email.welcome",
            //    exchange: ExchangeName,
            //    routingKey: "email.welcome");

            //_channel.QueueBind(
            //    queue: "email.contact.created",
            //    exchange: ExchangeName,
            //    routingKey: "email.contact.created");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //try
            //{
            //    // Welcome Email Consumer
            //    var welcomeConsumer = new AsyncEventingBasicConsumer(_channel);
            //    welcomeConsumer.Received += async (_, ea) =>
            //    {
            //        try
            //        {
            //            var message = JsonSerializer.Deserialize<WelcomeEmailMessage>(
            //                Encoding.UTF8.GetString(ea.Body.ToArray()));

            //            await _emailService.SendEmailAsync(
            //                message.To,
            //                "Welcome to Contact Management System",
            //                GetWelcomeEmailTemplate(message.UserName),
            //                true);

            //            _channel.BasicAck(ea.DeliveryTag, false);
            //            _logger.LogInformation("Welcome email sent to {Email}", message.To);
            //        }
            //        catch (Exception ex)
            //        {
            //            _logger.LogError(ex, "Error processing welcome email");
            //            _channel.BasicNack(ea.DeliveryTag, false, true);
            //        }
            //    };

            //    // Contact Created Email Consumer
            //    var contactConsumer = new AsyncEventingBasicConsumer(_channel);
            //    contactConsumer.Received += async (_, ea) =>
            //    {
            //        try
            //        {
            //            var message = JsonSerializer.Deserialize<ContactCreatedEmailMessage>(
            //                Encoding.UTF8.GetString(ea.Body.ToArray()));

            //            await _emailService.SendEmailAsync(
            //                message.To,
            //                "New Contact Created",
            //                GetContactCreatedEmailTemplate(message.ContactName),
            //                true);

            //            _channel.BasicAck(ea.DeliveryTag, false);
            //            _logger.LogInformation("Contact created email sent to {Email}", message.To);
            //        }
            //        catch (Exception ex)
            //        {
            //            _logger.LogError(ex, "Error processing contact created email");
            //            _channel.BasicNack(ea.DeliveryTag, false, true);
            //        }
            //    };

            //    var contactUpdatedConsumer = new AsyncEventingBasicConsumer(_channel);
            //    contactUpdatedConsumer.Received += async (_, ea) =>
            //    {
            //        try
            //        {
            //            var message = JsonSerializer.Deserialize<ContactUpdatedEmailMessage>(
            //                Encoding.UTF8.GetString(ea.Body.ToArray()));

            //            await _emailService.SendEmailAsync(
            //                message.To,
            //                "Contact Information Updated",
            //                ContactUpdatedEmailTemplate.GetTemplate(message.ContactName),
            //                true);

            //            _channel.BasicAck(ea.DeliveryTag, false);
            //            _logger.LogInformation("Contact updated email sent to {Email}", message.To);
            //        }
            //        catch (Exception ex)
            //        {
            //            _logger.LogError(ex, "Error processing contact updated email");
            //            _channel.BasicNack(ea.DeliveryTag, false, true);
            //        }
            //    };

            //    // Start consuming
            //    _channel.BasicConsume(
            //        queue: "email.welcome",
            //        autoAck: false,
            //        consumer: welcomeConsumer);

            //    _channel.BasicConsume(
            //        queue: "email.contact.created",
            //        autoAck: false,
            //        consumer: contactConsumer);

            //    _channel.BasicConsume(
            //        queue: "email.contact.updated",
            //        autoAck: false,
            //        consumer: contactUpdatedConsumer);


            //    while (!stoppingToken.IsCancellationRequested)
            //    {
            //        await Task.Delay(1000, stoppingToken);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Error in email worker");
            //}
        }

        private string GetWelcomeEmailTemplate(string userName)
        {
            return $@"
            <html>
                <body>
                    <h2>Welcome to Contact Management System!</h2>
                    <p>Dear {userName},</p>
                    <p>Thank you for joining our Contact Management System. We're excited to have you on board!</p>
                    <p>Best regards,<br>The Contact Management Team</p>
                </body>
            </html>";
        }

        private string GetContactCreatedEmailTemplate(string contactName)
        {
            return $@"
            <html>
                <body>
                    <h2>New Contact Created</h2>
                    <p>A new contact has been created in your system:</p>
                    <p><strong>{contactName}</strong></p>
                    <p>Best regards,<br>The Contact Management Team</p>
                </body>
            </html>";
        }

        public override void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
            base.Dispose();
        }
    }

}
