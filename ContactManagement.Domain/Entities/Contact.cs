using ContactManagement.Domain.Common;
using ContactManagement.Domain.Enums;
using ContactManagement.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Domain.Entities
{

    public class Contact : AggregateRoot
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Email Email { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public Address Address { get; private set; }
        public ContactStatus Status { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        protected Contact() { }

        private Contact(string firstName, string lastName, Email email, PhoneNumber phoneNumber, Address address, Guid userId)
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PhoneNumber = phoneNumber;
            Address = address;
            UserId = userId;
            Status = ContactStatus.Active;
            CreatedAt = DateTime.UtcNow;
        }

        public static Result<Contact> Create(string firstName, string lastName, string email, string phoneNumber, Address address, Guid userId)
        {
            var emailResult = Email.Create(email);
            if (!emailResult.IsSuccess)
                return Result<Contact>.Failure(emailResult.Error);

            var phoneNumberResult = PhoneNumber.Create(phoneNumber);
            if (!phoneNumberResult.IsSuccess)
                return Result<Contact>.Failure(phoneNumberResult.Error);

            return Result<Contact>.Success(new Contact(firstName, lastName, emailResult.Value, phoneNumberResult.Value, address, userId));
        }

        public void Update(string firstName, string lastName, Email value1, PhoneNumber value2, Address value3)
        {
            throw new NotImplementedException();
        }
    }
}

