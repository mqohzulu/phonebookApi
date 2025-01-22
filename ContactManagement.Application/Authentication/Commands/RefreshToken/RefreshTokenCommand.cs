using ContactManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Authentication.Comnands.RefreshToken
{
    public record RefreshTokenCommand : IRequest<Result<AuthenticationResult>>
    {
        public string RefreshToken { get; init; }
    }
}
