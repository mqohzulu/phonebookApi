using ContactManagement.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Domain.Entities
{
    public class ContactNote : Entity
    {
        public string Content { get; private set; }
        public Guid ContactId { get; private set; }
        public Guid CreatedBy { get; private set; }
        public DateTime CreatedAt { get; private set; }

        protected ContactNote() { }

        private ContactNote(string content, Guid contactId, Guid createdBy)
        {
            Id = Guid.NewGuid();
            Content = content;
            ContactId = contactId;
            CreatedBy = createdBy;
            CreatedAt = DateTime.UtcNow;
        }

        public static Result<ContactNote> Create(string content, Guid contactId, Guid createdBy)
        {
            if (string.IsNullOrWhiteSpace(content))
                return Result<ContactNote>.Failure("Note content is required");

            return Result<ContactNote>.Success(new ContactNote(content, contactId, createdBy));
        }
    }
}
