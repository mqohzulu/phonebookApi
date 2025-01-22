using ContactManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Domain.Events
{
    public sealed class UserCreatedDomainEvent : DomainEvent
    {
        public Guid UserId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public IReadOnlyList<string> AssignedRoles { get; }

        public UserCreatedDomainEvent(
            Guid userId,
            string firstName,
            string lastName,
            string email,
            IReadOnlyList<string> assignedRoles)
        {
            UserId = userId;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            AssignedRoles = assignedRoles;
        }
    }
}
