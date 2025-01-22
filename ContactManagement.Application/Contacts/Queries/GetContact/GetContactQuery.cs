using ContactManagement.Application.DTOs;
using ContactManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Contacts.Queries.GetContact
{
    public record GetContactQuery(Guid Id) : IRequest<Result<ContactDto>>;
}
