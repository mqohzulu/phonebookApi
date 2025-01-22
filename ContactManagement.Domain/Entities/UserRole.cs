using ContactManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Domain.Entities
{

    public class UserRole : Entity
    {
        public Guid UserId { get; private set; }
        public User User { get; private set; }

        public Guid RoleId { get; private set; }
        public Role Role { get; private set; }

        public DateTime AssignedAt { get; private set; }
        public Guid AssignedBy { get; private set; }

        protected UserRole() { } // For EF Core

        public UserRole(Guid userId, Guid roleId, Guid assignedBy)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            RoleId = roleId;
            AssignedBy = assignedBy;
            AssignedAt = DateTime.UtcNow;
        }

        public static UserRole Create(Guid userId, Guid roleId, Guid assignedBy)
        {
            return new UserRole(userId, roleId, assignedBy);
        }
    }
}
