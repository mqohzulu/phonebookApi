using ContactManagement.Application.Common.Interfaces;
using ContactManagement.Application.Interfaces;
using ContactManagement.Domain.Common;
using ContactManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Contacts.Commands.AddContactNote
{

    public class AddContactNoteCommandHandler : IRequestHandler<AddContactNoteCommand, Result<Guid>>
    {
        private readonly IContactRepository _contactRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public AddContactNoteCommandHandler(IContactRepository contactRepository, ICurrentUserService currentUserService,IUnitOfWork unitOfWork)
        {
            _contactRepository = contactRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(AddContactNoteCommand command, CancellationToken cancellationToken)
        {
            // Get the contact
            var contact = await _contactRepository.GetByIdAsync(command.ContactId);

            if (contact == null)
                return Result<Guid>.Failure(Error.NotFound("Contact.NotFound", "Contact not found"));

            // Verify ownership
            if (contact.UserId != _currentUserService.UserId)
                return Result<Guid>.Failure(Error.Forbidden("Contact.Unauthorized", "You are not authorized to add notes to this contact"));

            // Create and add the note
            var noteResult = ContactNote.Create(
                command.Content,
                command.ContactId,
                _currentUserService.UserId.Value);

            if (noteResult.IsFailure)
                return Result<Guid>.Failure(noteResult.Error);

            await _contactRepository.AddNoteAsync(noteResult.Value);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(noteResult.Value.Id);
        }
    }
}
