using ContactManagement.Application.Interfaces;
using ContactManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Users.Commands.DeleteUser
{
    public record DeleteUserCommand(Guid Id) : IRequest<Result<Unit>>;

}
