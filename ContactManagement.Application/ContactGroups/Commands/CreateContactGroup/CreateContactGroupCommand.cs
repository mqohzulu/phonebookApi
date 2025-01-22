using ContactManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.ContactGroups.Commands
{
    public record CreateContactGroupCommand : IRequest<Result<Guid>>
    {
        public string Name { get; init; }
        public string? Description { get; init; }
    }
}
