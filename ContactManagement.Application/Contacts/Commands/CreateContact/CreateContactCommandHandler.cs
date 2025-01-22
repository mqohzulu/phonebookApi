using ContactManagement.Application.Common.Interfaces;
using ContactManagement.Application.Interfaces;
using ContactManagement.Domain.Common;
using ContactManagement.Domain.Entities;
using ContactManagement.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Contacts.Commands.CreateContact
{
    public class CreateContactCommandHandler: IRequestHandler<CreateContactCommand, Result<Guid>>
    {
        private readonly IContactRepository _contactRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateContactCommandHandler(
            IContactRepository contactRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _contactRepository = contactRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(
            CreateContactCommand request,
            CancellationToken cancellationToken)
        {
            var address = Address.Create(
                request.Address.Street,
                request.Address.City,
                request.Address.State,
                request.Address.PostalCode,
                request.Address.Country);

            if (address.IsFailure)
                return Result<Guid>.Failure(address.Error);

            var contact = Contact.Create(
                request.FirstName,
                request.LastName,
                request.Email,
                request.PhoneNumber,
                address.Value,
                _currentUserService.UserId.Value);

            if (contact.IsFailure)
                return Result<Guid>.Failure(contact.Error);

            await _contactRepository.AddAsync(contact.Value);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(contact.Value.Id);
        }
    }
}
