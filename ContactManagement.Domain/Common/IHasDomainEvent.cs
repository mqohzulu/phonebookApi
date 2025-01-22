﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Domain.Common
{
    public interface IHasDomainEvents
    {
        IReadOnlyCollection<DomainEvent> DomainEvents { get; }
        void ClearDomainEvents();
    }
}
