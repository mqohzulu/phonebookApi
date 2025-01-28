using ContactManagement.Application.Authentication.Common.Interfaces;
using ContactManagement.Application.Common.Interfaces;
using ContactManagement.Application.Common.Models;
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


namespace ContactManagement.Application.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<Guid>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRabbitMQService _rabbitMQService;

        public CreateUserCommandHandler(IUserRepository userRepository,IPasswordHasher passwordHasher,IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            if (await _userRepository.ExistsByEmailAsync(command.Email))
                return Result<Guid>.Failure(Error.Conflict("User.DuplicateEmail", "Email is already in use"));

            var emailResult = Email.Create(command.Email);
            if (emailResult.IsFailure)
                return Result<Guid>.Failure(emailResult.Error);

            var passwordHash = _passwordHasher.HashPassword(command.Password);
            var passwordResult = Password.Create(passwordHash);

            if (passwordResult.IsFailure)
                return Result<Guid>.Failure(passwordResult.Error);

            var user = User.Create(
                command.FirstName,
                command.LastName,
                emailResult.ToString(),
                passwordResult.ToString());

            if (user.IsFailure)
                return Result<Guid>.Failure(user.Error);

            await _userRepository.AddAsync(user.Value);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _rabbitMQService.PublishEmailMessageAsync(new WelcomeEmailMessage
            {
                To = command.Email,
                Subject = "Welcome to our system",
                Body = $"Welcome {command.FirstName}!",
                IsHtml = true,
                UserName = command.FirstName
            });

            return Result<Guid>.Success(user.Value.Id);
        }
    }
}
