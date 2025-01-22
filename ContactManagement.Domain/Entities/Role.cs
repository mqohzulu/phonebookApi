using ContactManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Domain.Entities
{
    public class Role : Entity
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public bool IsDefault { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private readonly List<UserRole> _userRoles = new();
        public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

        protected Role() { } // For EF Core

        private Role(string name, string? description, bool isDefault = false)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            IsDefault = isDefault;
            CreatedAt = DateTime.UtcNow;
        }

        public static Result<Role> Create(string name, string? description = null, bool isDefault = false)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<Role>.Failure(Error.Validation("Role.EmptyName", "Role name cannot be empty"));

            name = name.Trim();

            if (name.Length > 50)
                return Result<Role>.Failure(Error.Validation("Role.NameTooLong", "Role name cannot exceed 50 characters"));

            return Result<Role>.Success(new Role(name, description, isDefault));
        }

        public Result<string> Update(string name, string? description)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<string>.Failure(Error.Validation("Role.EmptyName", "Role name cannot be empty"));

            name = name.Trim();

            if (name.Length > 50)
                return Result<string>.Failure(Error.Validation("Role.NameTooLong", "Role name cannot exceed 50 characters"));

            Name = name;
            Description = description;
            UpdatedAt = DateTime.UtcNow;

            return Result<string>.Success("Update Successful");
        }

        public void AddUserRole(UserRole userRole)
        {
            _userRoles.Add(userRole);
        }

        public void RemoveUserRole(UserRole userRole)
        {
            _userRoles.Remove(userRole);
        }
    }
}
