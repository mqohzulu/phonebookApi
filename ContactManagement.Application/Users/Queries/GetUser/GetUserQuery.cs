
using ContactManagement.Application.DTOs;
using ContactManagement.Application.Users.DTOs;
using ContactManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Users.Queries.GetUser
{
    public record GetUserQuery(Guid Id) : IRequest<Result<UserDto>>;
}
