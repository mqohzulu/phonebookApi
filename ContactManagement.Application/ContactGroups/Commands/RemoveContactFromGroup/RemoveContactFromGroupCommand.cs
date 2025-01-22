﻿using ContactManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.ContactGroups.Commands.RemoveContactFromGroup
{
    public record RemoveContactFromGroupCommand : IRequest<Result<Unit>>
    {
        public Guid GroupId { get; init; }
        public Guid ContactId { get; init; }
    }
}
