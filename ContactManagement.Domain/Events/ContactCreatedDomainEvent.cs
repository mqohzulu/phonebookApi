using ContactManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Domain.Events
{
    public sealed class ContactCreatedDomainEvent : DomainEvent
    {
        public Guid ContactId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public Guid CreatedBy { get; }

        public ContactCreatedDomainEvent(
            Guid contactId,
            string firstName,
            string lastName,
            string email,
            Guid createdBy)
        {
            ContactId = contactId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            CreatedBy = createdBy;
        }
    }
}
