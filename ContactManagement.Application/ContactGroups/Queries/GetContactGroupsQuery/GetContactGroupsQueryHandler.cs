using AutoMapper;
using ContactManagement.Application.Common.Interfaces;
using ContactManagement.Application.DTOs;
using ContactManagement.Application.Interfaces;
using ContactManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.ContactGroups.Queries.GetContactGroupsQuery
{
    public class GetContactGroupsQueryHandler : IRequestHandler<GetContactGroupsQuery, Result<List<ContactGroupDto>>>
    {
        private readonly IContactGroupRepository _groupRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetContactGroupsQueryHandler(
            IContactGroupRepository groupRepository,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _groupRepository = groupRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<List<ContactGroupDto>>> Handle(GetContactGroupsQuery request, CancellationToken cancellationToken)
        {
            var groups = await _groupRepository.GetByUserIdAsync(_currentUserService.UserId.Value);
            return Result<List<ContactGroupDto>>.Success(_mapper.Map<List<ContactGroupDto>>(groups));
        }
    }
}
