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

namespace ContactManagement.Application.ContactGroups.Commands
{
    public class CreateContactGroupCommandHandler : IRequestHandler<CreateContactGroupCommand, Result<Guid>>
    {
        private readonly IContactGroupRepository _groupRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateContactGroupCommandHandler(
            IContactGroupRepository groupRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateContactGroupCommand command, CancellationToken cancellationToken)
        {
            if (!_currentUserService.UserId.HasValue)
                return Result<Guid>.Failure(Error.Unauthorized("User.Unauthorized", "User is not authenticated"));

            var groupResult = ContactGroup.Create(
                command.Name,
                command.Description,
                _currentUserService.UserId.Value);

            if (groupResult.IsFailure)
                return Result<Guid>.Failure(groupResult.Error);

            await _groupRepository.AddAsync(groupResult.Value);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(groupResult.Value.Id);
        }
    }
}
