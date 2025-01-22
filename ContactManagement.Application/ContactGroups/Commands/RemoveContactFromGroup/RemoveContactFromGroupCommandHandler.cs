using ContactManagement.Application.Common.Interfaces;
using ContactManagement.Application.Interfaces;
using ContactManagement.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.ContactGroups.Commands.RemoveContactFromGroup
{
    public class RemoveContactFromGroupCommandHandler : IRequestHandler<RemoveContactFromGroupCommand, Result<Unit>>
    {
        private readonly IContactGroupRepository _groupRepository;
        private readonly IContactRepository _contactRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveContactFromGroupCommandHandler> _logger;

        public RemoveContactFromGroupCommandHandler(
            IContactGroupRepository groupRepository,
            IContactRepository contactRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            ILogger<RemoveContactFromGroupCommandHandler> logger)
        {
            _groupRepository = groupRepository;
            _contactRepository = contactRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(RemoveContactFromGroupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var group = await _groupRepository.GetByIdAsync(request.GroupId);
                if (group == null)
                    return Result<Unit>.Failure(Error.NotFound("ContactGroup.NotFound", "Contact group not found"));

                if (group.UserId != _currentUserService.UserId)
                    return Result<Unit>.Failure(Error.Forbidden("ContactGroup.Forbidden", "You don't have permission to modify this group"));

                var contact = await _contactRepository.GetByIdAsync(request.ContactId);
                if (contact == null)
                    return Result<Unit>.Failure(Error.NotFound("Contact.NotFound", "Contact not found"));

                if (contact.UserId != _currentUserService.UserId)
                    return Result<Unit>.Failure(Error.Forbidden("Contact.Forbidden", "You don't have permission to modify this contact"));

                var result = await _groupRepository.RemoveContactFromGroupAsync(request.GroupId, request.ContactId);
                if (!result)
                    return Result<Unit>.Failure(Error.NotFound("ContactGroup.ContactNotInGroup", "Contact is not in the specified group"));

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation(
                    "Contact {ContactId} removed from group {GroupId} by user {UserId}",
                    request.ContactId, request.GroupId, _currentUserService.UserId);

                return Result<Unit>.Success(Unit.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error removing contact {ContactId} from group {GroupId}",
                    request.ContactId, request.GroupId);
                throw;
            }
        }
    }
}
