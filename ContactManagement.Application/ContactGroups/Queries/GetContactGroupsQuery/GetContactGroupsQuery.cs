using ContactManagement.Application.DTOs;
using ContactManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.ContactGroups.Queries.GetContactGroupsQuery
{
    public record GetContactGroupsQuery : IRequest<Result<List<ContactGroupDto>>>;
}
