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

namespace ContactManagement.Application.Contacts.Queries.GetContact
{
    public class GetContactQueryHandler : IRequestHandler<GetContactQuery, Result<ContactDto>>
    {
        private readonly IContactRepository _contactRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public GetContactQueryHandler(
            IContactRepository contactRepository,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _contactRepository = contactRepository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result<ContactDto>> Handle(GetContactQuery request, CancellationToken cancellationToken)
        {
            var contact = await _contactRepository.GetByIdAsync(request.Id);

            if (contact is null)
                return Result<ContactDto>.Failure(Error.NotFound("Contact.NotFound", "Contact not found"));

            // Check if the current user has access to this contact
            if (contact. != _currentUserService.UserId)
                return Result<ContactDto>.Failure(Error.Forbidden("Contact.Forbidden", "You don't have access to this contact"));

            var dto = _mapper.Map<ContactDto>(contact);
            return Result<ContactDto>.Success(dto);
        }
    }

}
