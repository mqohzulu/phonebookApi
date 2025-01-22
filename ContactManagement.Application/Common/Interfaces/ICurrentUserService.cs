using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Common.Interfaces
{

    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? UserEmail { get; }
        bool IsAuthenticated { get; }
        IEnumerable<string> Roles { get; }
    }
}
