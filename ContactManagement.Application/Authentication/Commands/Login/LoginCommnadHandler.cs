﻿using ContactManagement.Application.Authentication.Common;
using ContactManagement.Application.Authentication.Common.Interfaces;
using ContactManagement.Application.Common.Interfaces;
using ContactManagement.Application.Interfaces;
using ContactManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Authentication.Comnands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthenticationResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;

        public LoginCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator,IPasswordHasher passwordHasher,IUnitOfWork unitOfWork)
            {
                _userRepository = userRepository;
                _jwtTokenGenerator = jwtTokenGenerator;
                _passwordHasher = passwordHasher;
                _unitOfWork = unitOfWork;
            }

        public async Task<Result<AuthenticationResult>> Handle(LoginCommand command, CancellationToken cancellationToken)
        {

            var user = await _userRepository.GetByEmailAsync(command.Email);
            if (user == null)
                return Result<AuthenticationResult>.Failure(
                    Error.Unauthorized("Auth.InvalidCredentials", "Invalid credentials"));

            if (!_passwordHasher.VerifyPassword(command.Password, user.Password.Hash))
                return Result<AuthenticationResult>.Failure(
                    Error.Unauthorized("Auth.InvalidCredentials", "Invalid credentials"));


            if (!user.IsActive)
                return Result<AuthenticationResult>.Failure(
                    Error.Unauthorized("Auth.InactiveUser", "User account is inactive"));


            var roles = user.Roles.Select(r => r.Role.Name).ToList();


            var token = _jwtTokenGenerator.GenerateToken(user, roles);

            var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(user.Id);
            user.AddRefreshToken(refreshToken);

            user.UpdateLastLogin();
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<AuthenticationResult>.Success(new AuthenticationResult(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email.Value,
                token,
                refreshToken.Token,
                roles));
        }
    }

}
