using ContactManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Authentication.Common.Interfaces
{

    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user, IList<string> roles);
        RefreshToken GenerateRefreshToken(Guid userId);
    }
}
