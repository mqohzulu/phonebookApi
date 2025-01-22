using ContactManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Authentication.Common.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthenticationResult>> AuthenticateAsync(string email, string password);
        Task<Result<AuthenticationResult>> RefreshTokenAsync(string refreshToken);
        Task<Result<string>> RevokeTokenAsync(string refreshToken);
        Task<Result<bool>> ValidateTokenAsync(string token);
    }
}
