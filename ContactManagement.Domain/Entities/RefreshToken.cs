using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Domain.Entities
{
    public class RefreshToken : Entity
    {
        public string Token { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string? RevokedBy { get; private set; }
        public DateTime? RevokedAt { get; private set; }
        public string? ReplacedByToken { get; private set; }
        public string? ReasonRevoked { get; private set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsRevoked => RevokedAt != null;
        public bool IsActive => !IsRevoked && !IsExpired;

        protected RefreshToken() { } // For EF Core

        private RefreshToken(
            Guid userId,
            DateTime expiresAt)
        {
            Id = Guid.NewGuid();
            Token = GenerateRefreshToken();
            UserId = userId;
            ExpiresAt = expiresAt;
            CreatedAt = DateTime.UtcNow;
        }

        public static RefreshToken Create(Guid userId, DateTime expiresAt)
        {
            return new RefreshToken(userId, expiresAt);
        }

        public void Revoke(string? revokedBy = null, string? replacedByToken = null, string? reason = null)
        {
            RevokedBy = revokedBy;
            RevokedAt = DateTime.UtcNow;
            ReplacedByToken = replacedByToken;
            ReasonRevoked = reason;
        }

        private static string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
