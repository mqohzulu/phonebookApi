using ContactManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Domain.Entities
{

    public class ContactGroup : Entity
    {
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public Guid UserId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        private readonly List<Contact> _contacts = new();
        public IReadOnlyCollection<Contact> Contacts => _contacts.AsReadOnly();

        protected ContactGroup() { }

        private ContactGroup(string name, string? description, Guid userId)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            UserId = userId;
            CreatedAt = DateTime.UtcNow;
        }

        public static Result<ContactGroup> Create(string name, string? description, Guid userId)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Result<ContactGroup>.Failure("Group name is required");

            return Result<ContactGroup>.Success(new ContactGroup(name, description, userId));
        }

        public Result<Unit> Update(string name, string? description)
        {
            throw new NotImplementedException();
        }
    }
}
