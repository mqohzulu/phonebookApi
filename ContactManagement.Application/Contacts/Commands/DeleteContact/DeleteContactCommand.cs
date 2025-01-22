using ContactManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Contacts.Commands.DeleteContact
{
    public record DeleteContactCommand(Guid Id) : IRequest<Result<Unit>>;
}
