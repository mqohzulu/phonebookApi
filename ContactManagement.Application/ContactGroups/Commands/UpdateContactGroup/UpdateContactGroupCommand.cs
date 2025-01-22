using ContactManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.ContactGroups.Commands.UpdateContactGroup
{
    public record UpdateContactGroupCommand : IRequest<Result<Unit>>
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string? Description { get; init; }
    }

}
