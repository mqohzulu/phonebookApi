using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Domain.Common
{
    public abstract class DomainEvent : INotification
    {
        protected DomainEvent()
        {
            EventId = Guid.NewGuid();
            OccurredOn = DateTime.UtcNow;
        }

        public Guid EventId { get; }
        public DateTime OccurredOn { get; }
    }
}
