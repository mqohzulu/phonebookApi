using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Domain.Events
{
    public sealed class UserLoggedInDomainEvent : DomainEvent
    {
        public Guid UserId { get; }
        public DateTime LoggedInAt { get; }
        public string IpAddress { get; }
        public string UserAgent { get; }

        public UserLoggedInDomainEvent(
            Guid userId,
            DateTime loggedInAt,
            string ipAddress,
            string userAgent)
        {
            UserId = userId;
            LoggedInAt = loggedInAt;
            IpAddress = ipAddress;
            UserAgent = userAgent;
        }
    }
}
