using ContactManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.ContactGroups.Commands.DeleteContactGroup
{
    public record DeleteContactGroupCommand(Guid Id) : IRequest<Result<Unit>>;
}
