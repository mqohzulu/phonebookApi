using ContactManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Domain.ValueObjects
{
    public class Password : ValueObject
    {
        public string Hash { get; private set; }

        private Password(string hash)
        {
            Hash = hash;
        }

        public static Result<Password> Create(string plainPassword)
        {
            if (string.IsNullOrWhiteSpace(plainPassword))
                return Result<Password>.Failure("Password cannot be empty");

            if (plainPassword.Length < 6)
                return Result<Password>.Failure("Password must be at least 6 characters");

            return Result<Password>.Success(new Password(plainPassword));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Hash;
        }
    }
}
