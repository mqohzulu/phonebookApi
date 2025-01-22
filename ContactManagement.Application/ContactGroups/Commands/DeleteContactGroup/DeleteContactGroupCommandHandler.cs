using ContactManagement.Application.Common.Interfaces;
using ContactManagement.Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.ContactGroups.Commands.DeleteContactGroup
{
    public class DeleteContactGroupCommandHandler : IRequestHandler<DeleteContactGroupCommand, Result<Unit>>
    {
        private readonly IContactGroupRepository _groupRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteContactGroupCommandHandler> _logger;

        public DeleteContactGroupCommandHandler(
            IContactGroupRepository groupRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork,
            ILogger<DeleteContactGroupCommandHandler> logger)
        {
            _groupRepository = groupRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<Unit>> Handle(DeleteContactGroupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var group = await _groupRepository.GetByIdAsync(request.Id);

                if (group == null)
                    return Result<Unit>.Failure(Error.NotFound("ContactGroup.NotFound", "Contact group not found"));

                if (group.UserId != _currentUserService.UserId)
                    return Result<Unit>.Failure(Error.Forbidden("ContactGroup.Forbidden", "You don't have permission to delete this group"));

                // Check if group has contacts
                var contactCount = await _groupRepository.GetContactCountAsync(group.Id);
                if (contactCount > 0)
                {
                    return Result<Unit>.Failure(Error.Conflict(
                        "ContactGroup.HasContacts",
                        "Cannot delete group that contains contacts. Remove all contacts first."));
                }

                await _groupRepository.DeleteAsync(group);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Contact group {GroupId} deleted by user {UserId}",
                    request.Id, _currentUserService.UserId);

                return Result<Unit>.Success(Unit.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting contact group {GroupId}", request.Id);
                throw;
            }
        }
    }
}
