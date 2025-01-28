using ContactManagement.Application.Authentication.Common;
using ContactManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Authentication.Comnands.Login
{
    public record LoginCommand : IRequest<Result<AuthenticationResult>>
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }

}
