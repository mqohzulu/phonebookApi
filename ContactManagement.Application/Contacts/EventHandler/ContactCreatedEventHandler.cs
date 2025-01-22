using ContactManagement.Application.Common.Interfaces;
using ContactManagement.Application.Common.Models;
using ContactManagement.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Contacts.EventHandler
{
    public class ContactCreatedEventHandler: INotificationHandler<ContactCreatedDomainEvent>
    {
        private readonly IRabbitMQService _rabbitMQService;
        private readonly ILogger<ContactCreatedEventHandler> _logger;

        public ContactCreatedEventHandler(IRabbitMQService rabbitMQService, ILogger<ContactCreatedEventHandler> logger)
        {
            _rabbitMQService = rabbitMQService;
            _logger = logger;
        }

        public async Task Handle(ContactCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            try
            {
                var message = new ContactCreatedEmailMessage
                {
                    To = notification.Email,
                    Subject = "New Contact Created",
                    ContactName = $"{notification.FirstName} {notification.LastName}",
                    IsHtml = true
                };

                await _rabbitMQService.PublishEmailMessageAsync(message);

                _logger.LogInformation(
                    "Contact created email message published for {Email}",
                    notification.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error publishing contact created email for {Email}",
                    notification.Email);
                throw;
            }
        }
    }
}
