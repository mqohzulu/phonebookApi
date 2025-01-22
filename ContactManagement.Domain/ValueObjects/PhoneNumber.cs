using ContactManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Domain.ValueObjects
{
    public class PhoneNumber : ValueObject
    {
        public string Value { get; private set; }

        private PhoneNumber(string value)
        {
            Value = value;
        }

        public static Result<PhoneNumber> Create(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return Result<PhoneNumber>.Failure("Phone number is required");

            // Add phone number validation logic here
            var normalized = NormalizePhoneNumber(phoneNumber);
            if (!IsValidPhoneNumber(normalized))
                return Result<PhoneNumber>.Failure("Invalid phone number format");

            return Result<PhoneNumber>.Success(new PhoneNumber(normalized));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        private static string NormalizePhoneNumber(string phoneNumber)
        {
            return string.Join("", phoneNumber.Where(char.IsDigit));
        }

        private static bool IsValidPhoneNumber(string phoneNumber)
        {
            return phoneNumber.Length >= 10 && phoneNumber.Length <= 15;
        }
    }
}
