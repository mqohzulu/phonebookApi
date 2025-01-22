using ContactManagement.Application.Authentication.Common;
using ContactManagement.Application.Authentication.Common.Interfaces;
using ContactManagement.Application.Common.Interfaces;
using ContactManagement.Application.Interfaces;
using ContactManagement.Domain.Common;
using ContactManagement.Infrastructure.Authentication;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Infrastructure.Services
{


    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserRepository userRepository,
            IJwtTokenGenerator jwtTokenGenerator,
            IPasswordHasher passwordHasher,
            IUnitOfWork unitOfWork,
            ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<AuthenticationResult>> AuthenticateAsync(string email, string password)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogWarning("Authentication failed: User not found for email {Email}", email);
                    return Result<AuthenticationResult>.Failure(
                        Error.Unauthorized("Auth.InvalidCredentials", "Invalid credentials"));
                }

                if (!user.IsActive)
                {
                    _logger.LogWarning("Authentication failed: Inactive user {UserId}", user.Id);
                    return Result<AuthenticationResult>.Failure(
                        Error.Unauthorized("Auth.InactiveUser", "User account is inactive"));
                }

                if (!_passwordHasher.VerifyPassword(password, user.Password.Hash))
                {
                    _logger.LogWarning("Authentication failed: Invalid password for user {UserId}", user.Id);
                    return Result<AuthenticationResult>.Failure(
                        Error.Unauthorized("Auth.InvalidCredentials", "Invalid credentials"));
                }

                var roles = user.Roles.Select(r => r.Role.Name).ToList();
                var token = _jwtTokenGenerator.GenerateToken(user, roles);
                var refreshToken = _jwtTokenGenerator.GenerateRefreshToken(user.Id);

                user.AddRefreshToken(refreshToken);
                user.UpdateLastLogin();

                await _unitOfWork.SaveChangesAsync();

                return Result<AuthenticationResult>.Success(new AuthenticationResult(
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email.Value,
                    token,
                    refreshToken.Token,
                    roles));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during authentication for email {Email}", email);
                return Result<AuthenticationResult>.Failure(
                    Error.Internal("Auth.Error", "An error occurred during authentication"));
            }
        }

        public async Task<Result<AuthenticationResult>> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);
                if (user == null)
                {
                    return Result<AuthenticationResult>.Failure(
                        Error.Unauthorized("Auth.InvalidToken", "Invalid refresh token"));
                }

                var existingRefreshToken = user.RefreshTokens
                    .FirstOrDefault(rt => rt.Token == refreshToken);

                if (existingRefreshToken == null || !existingRefreshToken.IsActive)
                {
                    return Result<AuthenticationResult>.Failure(
                        Error.Unauthorized("Auth.InvalidToken", "Invalid refresh token"));
                }

                var roles = user.Roles.Select(r => r.Role.Name).ToList();
                var newToken = _jwtTokenGenerator.GenerateToken(user, roles);
                var newRefreshToken = _jwtTokenGenerator.GenerateRefreshToken(user.Id);

                existingRefreshToken.Revoke("Refresh token rotation", newRefreshToken.Token);
                user.AddRefreshToken(newRefreshToken);

                await _unitOfWork.SaveChangesAsync();

                return Result<AuthenticationResult>.Success(new AuthenticationResult(
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.Email.Value,
                    newToken,
                    newRefreshToken.Token,
                    roles));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token");
                return Result<AuthenticationResult>.Failure(
                    Error.Internal("Auth.Error", "An error occurred while refreshing the token"));
            }
        }

        public async Task<Result<string>> RevokeTokenAsync(string refreshToken)
        {
            try
            {
                var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);
                if (user == null)
                {
                    return Result<string>.Failure(
                        Error.NotFound("Auth.TokenNotFound", "Token not found"));
                }

                var existingRefreshToken = user.RefreshTokens
                    .FirstOrDefault(rt => rt.Token == refreshToken);

                if (existingRefreshToken == null)
                {
                    return Result<string>.Failure(
                        Error.NotFound("Auth.TokenNotFound", "Token not found"));
                }

                existingRefreshToken.Revoke("Manually revoked");
                await _unitOfWork.SaveChangesAsync();

                return Result<string>.Success("Token revoke success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking token");
                return Result<string>.Failure(
                    Error.Internal("Auth.Error", "An error occurred while revoking the token"));
            }
        }

        public async Task<Result<bool>> ValidateTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                if (!tokenHandler.CanReadToken(token))
                {
                    return Result<bool>.Success(false);
                }

                var jwtToken = tokenHandler.ReadJwtToken(token);
                var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value;

                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
                {
                    return Result<bool>.Success(false);
                }

                var user = await _userRepository.GetByIdAsync(userGuid);
                return Result<bool>.Success(user != null && user.IsActive);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token");
                return Result<bool>.Failure(
                    Error.Internal("Auth.Error", "An error occurred while validating the token"));
            }
        }
    }
}
