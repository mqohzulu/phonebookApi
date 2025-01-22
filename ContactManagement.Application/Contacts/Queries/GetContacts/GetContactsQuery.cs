using ContactManagement.Application.DTOs;
using ContactManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Contacts.Queries.GetContacts
{
    public record GetContactsQuery : IRequest<Result<List<ContactDto>>>
    {
        public string? SearchTerm { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }

}
