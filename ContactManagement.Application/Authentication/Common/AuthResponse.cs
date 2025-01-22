using ContactManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Authentication.Common
{
    public class AuthResponse
    {
        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
        public DateTime ExpiresAt { get; init; }
        public User User { get; init; }

        public AuthResponse(string accessToken, string refreshToken, DateTime expiresAt, User user)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpiresAt = expiresAt;
            User = user;
        }
    }
}
