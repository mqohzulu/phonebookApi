using ContactManagement.Application.Authentication.Common;
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

namespace ContactManagement.Application.Authentication.Comnands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthenticationResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenCommandHandler(
            IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<AuthenticationResult>> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
        {
            // Find user by refresh token
            var user = await _userRepository.GetByRefreshTokenAsync(command.RefreshToken);
            if (user == null)
                return Result<AuthenticationResult>.Failure(
                    Error.Unauthorized("Auth.InvalidToken", "Invalid refresh token"));

            // Validate refresh token
            var refreshToken = user.RefreshTokens.FirstOrDefault(rt => rt.Token == command.RefreshToken);
            if (refreshToken == null || !refreshToken.IsValid)
                return Result<AuthenticationResult>.Failure(
                    Error.Unauthorized("Auth.InvalidToken", "Invalid refresh token"));

            // Get user roles
            var roles = user.Roles.Select(r => r.Role.Name).ToList();

            // Generate new tokens
            var token = _jwtTokenGenerator.GenerateToken(user, roles);
            var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken(user.Id);

            // Revoke old refresh token and add new one
            refreshToken.Revoke();
            user.AddRefreshToken(newRefreshToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<AuthenticationResult>.Success(new AuthenticationResult(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email.Value,
                token,
                newRefreshToken.Token,
                roles));
        }
    }

}
