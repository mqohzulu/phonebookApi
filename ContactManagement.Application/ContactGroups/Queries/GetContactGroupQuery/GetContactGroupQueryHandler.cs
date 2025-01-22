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

namespace ContactManagement.Application.ContactGroups.Queries.GetContactGroupQuery
{
    public class GetContactGroupQueryHandler : IRequestHandler<GetContactGroupQuery, Result<ContactGroupDto>>
    {
        private readonly IContactGroupRepository _groupRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetContactGroupQueryHandler(
            IContactGroupRepository groupRepository,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _groupRepository = groupRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<ContactGroupDto>> Handle(GetContactGroupQuery request, CancellationToken cancellationToken)
        {
            var group = await _groupRepository.GetByIdAsync(request.Id);

            if (group == null)
                return Result<ContactGroupDto>.Failure(Error.NotFound("ContactGroup.NotFound", "Contact group not found"));

            if (group.UserId != _currentUserService.UserId)
                return Result<ContactGroupDto>.Failure(Error.Forbidden("ContactGroup.Forbidden", "You don't have permission to view this group"));

            var dto = _mapper.Map<ContactGroupDto>(group);
            dto.ContactCount = await _groupRepository.GetContactCountAsync(group.Id);

            return Result<ContactGroupDto>.Success(dto);
        }
    }

}
