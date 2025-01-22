using ContactManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Domain.ValueObjects
{
    public class Address : ValueObject
    {
        public string Street { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string PostalCode { get; private set; }
        public string Country { get; private set; }

        private Address(string street, string city, string state, string postalCode, string country)
        {
            Street = street;
            City = city;
            State = state;
            PostalCode = postalCode;
            Country = country;
        }

        public static Result<Address> Create(string street, string city, string state, string postalCode, string country)
        {
            if (string.IsNullOrWhiteSpace(city))
                return Result<Address>.Failure("City is required");

            if (string.IsNullOrWhiteSpace(country))
                return Result<Address>.Failure("Country is required");

            return Result<Address>.Success(new Address(street, city, state, postalCode, country));
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Street.ToLower();
            yield return City.ToLower();
            yield return State.ToLower();
            yield return PostalCode.ToLower();
            yield return Country.ToLower();
        }
    }
}
