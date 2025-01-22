using ContactManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Domain.Events
{
    public sealed class UserUpdatedDomainEvent : DomainEvent
    {
        public Guid UserId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public DateTime UpdatedAt { get; }
        public Guid? UpdatedBy { get; }

        public UserUpdatedDomainEvent(
            Guid userId,
            string firstName,
            string lastName,
            string email,
            DateTime updatedAt,
            Guid? updatedBy = null)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            UpdatedAt = updatedAt;
            UpdatedBy = updatedBy;
        }
    }

}
