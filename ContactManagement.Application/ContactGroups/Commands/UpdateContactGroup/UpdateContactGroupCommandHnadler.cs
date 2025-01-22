using ContactManagement.Application.Common.Interfaces;
using ContactManagement.Application.Interfaces;
using ContactManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.ContactGroups.Commands.UpdateContactGroup
{
    public class UpdateContactGroupCommandHandler : IRequestHandler<UpdateContactGroupCommand, Result<Unit>>
    {
        private readonly IContactGroupRepository _groupRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateContactGroupCommandHandler(
            IContactGroupRepository groupRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(UpdateContactGroupCommand command, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetByIdAsync(command.Id);

            if (group == null)
                return Result<Unit>.Failure(Error.NotFound("ContactGroup.NotFound", "Contact group not found"));

            if (group.UserId != _currentUserService.UserId)
                return Result<Unit>.Failure(Error.Forbidden("ContactGroup.Forbidden", "You don't have permission to update this group"));

            var updateResult = group.Update(command.Name, command.Description);
            if (updateResult.IsFailure)
                return updateResult;

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result<Unit>.Success(Unit.Value);
        }
    }
}
