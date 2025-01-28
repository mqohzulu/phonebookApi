using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Domain.Events
{
    public sealed class UserDeletedDomainEvent : DomainEvent
    {
        public Guid UserId { get; }
        public DateTime DeletedAt { get; }
        public Guid? DeletedBy { get; }

        public UserDeletedDomainEvent(
            Guid userId,
            DateTime deletedAt,
            Guid? deletedBy = null)
        {
            UserId = userId;
            DeletedAt = deletedAt;
            DeletedBy = deletedBy;
        }
    }
}
