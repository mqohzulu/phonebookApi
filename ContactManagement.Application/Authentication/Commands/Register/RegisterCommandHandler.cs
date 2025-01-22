using ContactManagement.Application.Authentication.Common.Interfaces;
using ContactManagement.Application.Authentication.Common;
using ContactManagement.Application.Common.Interfaces;
using ContactManagement.Application.Interfaces;
using ContactManagement.Application.Users.Commands.CreateUser;
using ContactManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthResponse>>
    {
        private readonly IMediator _mediator;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;
        private readonly IDateTime _dateTime;

        public RegisterCommandHandler(
            IMediator mediator,
            IJwtTokenGenerator jwtTokenGenerator,
            IUserRepository userRepository,
            IDateTime dateTime)
        {
            _mediator = mediator;
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
            _dateTime = dateTime;
        }

        public async Task<Result<AuthResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Check if email is already in use
            if (await _userRepository.ExistsByEmailAsync(request.Email))
                return Result<AuthResponse>.Failure("Email is already registered");

            // Create user using existing CreateUserCommand
            var createUserCommand = new CreateUserCommand
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = request.Password,
                Roles = new List<string> { "User" }  // Assign default role
            };

            var createUserResult = await _mediator.Send(createUserCommand, cancellationToken);

            if (createUserResult.IsFailure)
                return Result<AuthResponse>.Failure(createUserResult.Error);

            // Get the created user
            var user = await _userRepository.GetByIdAsync(createUserResult.Value);

            if (user == null)
                return Result<AuthResponse>.Failure("Failed to retrieve created user");

            // Generate tokens
            var accessToken = _jwtTokenGenerator.GenerateToken(user, user.Roles.Select(r => r.Name).ToList());
            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(user.Id);

            // Add refresh token to user
            user.AddRefreshToken(refreshToken);
            _userRepository.Update(user);

            // Create auth response
            var response = new AuthResponse(
                accessToken,
                refreshToken.Token,
                _dateTime.UtcNow.AddMinutes(TokenSettings.ExpiryMinutes),
                new User(
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email.Value,
                    user.Roles.Select(r => r.Role).ToList()
                ));

            return Result<AuthResponse>.Success(response);
        }
    }
}
