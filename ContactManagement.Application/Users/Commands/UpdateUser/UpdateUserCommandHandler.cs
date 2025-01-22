using ContactManagement.Application.Common.Interfaces;
using ContactManagement.Application.Interfaces;
using ContactManagement.Domain.Common;
using ContactManagement.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<Unit>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(command.Id);

            if (user is null)
            {
                return Result<Unit>.Failure(Error.NotFound("User.NotFound", "User not found"));
            }

            var emailResult = Email.Create(command.Email);
            if (emailResult.IsFailure)
            {
                return Result<Unit>.Failure(emailResult.Error);
            }

            // Check if email is already in use by another user
            if (await _userRepository.ExistsByEmailAsync(command.Email) &&
                user.Email.Value != command.Email)
            {
                return Result<Unit>.Failure(Error.Conflict("User.DuplicateEmail", "Email is already in use"));
            }

            var result = user.Update(
                command.FirstName,
                command.LastName,
                emailResult.Value);

            if (result.IsFailure)
            {
                return Result<Unit>.Failure(result.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Unit>.Success(Unit.Value);
        }
    }


}
