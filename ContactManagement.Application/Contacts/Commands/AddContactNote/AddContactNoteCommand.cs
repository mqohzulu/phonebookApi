using ContactManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Contacts.Commands.AddContactNote
{
    public record AddContactNoteCommand : IRequest<Result<Guid>>
    {
        public Guid ContactId { get; init; }
        public string Content { get; init; }
    }
}
