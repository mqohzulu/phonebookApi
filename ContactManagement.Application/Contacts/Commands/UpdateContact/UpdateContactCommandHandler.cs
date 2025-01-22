using ContactManagement.Application.Common.Interfaces;
using ContactManagement.Application.Interfaces;
using ContactManagement.Domain.Common;
using ContactManagement.Domain.Entities;
using ContactManagement.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Contacts.Commands.UpdateContact
{

    public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, Result<Unit>>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRabbitMQService _rabbitMQService;
        private readonly ILogger<UpdateContactCommandHandler> _logger;

        public UpdateContactCommandHandler(
            IContactRepository contactRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IRabbitMQService rabbitMQService,
            ILogger<UpdateContactCommandHandler> logger)
        {
            _contactRepository = contactRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _rabbitMQService = rabbitMQService;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(UpdateContactCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var contact = await _contactRepository.GetByIdAsync(command.Id);

                if (contact == null)
                    return Result<Unit>.Failure(Error.NotFound("Contact.NotFound", "Contact not found"));

                if (contact.UserId != _currentUserService.UserId)
                    return Result<Unit>.Failure(Error.Forbidden("Contact.Forbidden", "You don't have permission to update this contact"));

                var emailResult = Email.Create(command.Email);
                if (emailResult.IsFailure)
                    return Result<Unit>.Failure(emailResult.Error);

                var phoneResult = PhoneNumber.Create(command.PhoneNumber);
                if (phoneResult.IsFailure)
                    return Result<Unit>.Failure(phoneResult.Error);

                var addressResult = Address.Create(
                    command.Address.Street,
                    command.Address.City,
                    command.Address.State,
                    command.Address.PostalCode,
                    command.Address.Country);

                if (addressResult.IsFailure)
                    return Result<Unit>.Failure(addressResult.Error);

                contact.Update(
                    command.FirstName,
                    command.LastName,
                    emailResult.Value,
                    phoneResult.Value,
                    addressResult.Value);

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                // Send email notification
                await SendUpdateNotificationAsync(contact);

                return Result<Unit>.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating contact {ContactId}", command.Id);
                throw;
            }
        }

        private async Task SendUpdateNotificationAsync(Contact contact)
        {
            var message = new ContactUpdatedEmailMessage
            {
                To = contact.Email.Value,
                Subject = "Contact Information Updated",
                ContactName = $"{contact.FirstName} {contact.LastName}",
                IsHtml = true
            };

            await _rabbitMQService.PublishEmailMessageAsync(message);
        }
    }

}
