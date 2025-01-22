using ContactManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Domain.Events
{
    public sealed class ContactUpdatedDomainEvent : DomainEvent
    {
        public Guid ContactId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public Guid UpdatedBy { get; }

        public ContactUpdatedDomainEvent(
            Guid contactId,
            string firstName,
            string lastName,
            string email,
            Guid updatedBy)
        {
            ContactId = contactId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UpdatedBy = updatedBy;
        }
    }

}
