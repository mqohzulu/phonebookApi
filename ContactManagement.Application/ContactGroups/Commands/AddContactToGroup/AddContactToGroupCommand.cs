using ContactManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.ContactGroups.Commands.AddContactToGroup
{
    public record AddContactToGroupCommand : IRequest<Result<Unit>>
    {
        public Guid GroupId { get; init; }
        public Guid ContactId { get; init; }
    }
}
