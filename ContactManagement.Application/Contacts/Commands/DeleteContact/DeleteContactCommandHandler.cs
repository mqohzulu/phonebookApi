using ContactManagement.Application.Common.Interfaces;
using ContactManagement.Application.Interfaces;
using ContactManagement.Domain.Common;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Contacts.Commands.DeleteContact
{
    public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, Result<Unit>>
    {
        private readonly IContactRepository _contactRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteContactCommandHandler(
            IContactRepository contactRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _contactRepository = contactRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(DeleteContactCommand command, CancellationToken cancellationToken)
        {
            // Get the contact
            var contact = await _contactRepository.GetByIdAsync(command.Id);

            if (contact == null)
                return Result<Unit>.Failure(Error.NotFound("Contact.NotFound", "Contact not found"));

            // Verify ownership
            if (contact.UserId != _currentUserService.UserId)
                return Result<Unit>.Failure(Error.Forbidden("Contact.Unauthorized", "You are not authorized to delete this contact"));

            _contactRepository.Delete(contact);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Unit>.Success(Unit.Value);
        }
    }

}
