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

namespace ContactManagement.Application.Contacts.Queries.GetContacts
{
    public class GetContactsQueryHandler
      : IRequestHandler<GetContactsQuery, Result<List<ContactDto>>>
    {
        private readonly IContactRepository _contactRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public GetContactsQueryHandler(
            IContactRepository contactRepository,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _contactRepository = contactRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<Result<List<ContactDto>>> Handle(
            GetContactsQuery request,
            CancellationToken cancellationToken)
        {
            var contacts = await _contactRepository.GetByUserIdAsync(_currentUserService.UserId.Value);

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                contacts = contacts.Where(c =>
                    c.FirstName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    c.LastName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    c.Email.Value.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase)
                ).ToList();
            }

            var pagedContacts = contacts
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            return Result<List<ContactDto>>.Success(
                _mapper.Map<List<ContactDto>>(pagedContacts));
        }
    }

}
