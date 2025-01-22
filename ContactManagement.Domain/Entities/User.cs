using ContactManagement.Domain.Common;
using ContactManagement.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Domain.Entities
{

    public class User : AggregateRoot
    {
        private readonly List<RefreshToken> _refreshTokens = new();
        public IReadOnlyCollection<RefreshToken> RefreshTokens => _refreshTokens.AsReadOnly();

        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Email Email { get; private set; }
        public Password Password { get; private set; }
        public bool IsActive { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? LastLogin { get; private set; }
        private readonly List<UserRole> _roles = new();
        public IReadOnlyCollection<UserRole> Roles => _roles.AsReadOnly();

        protected User() { }

        private User(string firstName, string lastName, Email email, Password password)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
        }

        public static Result<User> Create(string firstName, string lastName, string email, string password)
        {
            var emailResult = Email.Create(email);
            if (!emailResult.IsSuccess)
                return Result<User>.Failure(emailResult.Error);

            var passwordResult = Password.Create(password);
            if (!passwordResult.IsSuccess)
                return Result<User>.Failure(passwordResult.Error);

            return Result<User>.Success(new User(firstName, lastName, emailResult.Value, passwordResult.Value));
        }
        public void AddRefreshToken(RefreshToken refreshToken)
        {
            _refreshTokens.Add(refreshToken);
        }

        public void RemoveOldRefreshTokens(int daysToKeep)
        {
            var oldTokens = _refreshTokens
                .Where(r => !r.IsActive &&
                           r.CreatedAt.AddDays(daysToKeep) <= DateTime.UtcNow)
                .ToList();

            foreach (var token in oldTokens)
            {
                _refreshTokens.Remove(token);
            }
        }

        public void UpdateLastLogin()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public object Update(string firstName, string lastName, Email value)
        {
            throw new NotImplementedException();
        }
    }
}
