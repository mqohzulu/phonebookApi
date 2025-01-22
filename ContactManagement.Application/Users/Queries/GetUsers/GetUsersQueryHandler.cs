using AutoMapper;
using ContactManagement.Application.Interfaces;
using ContactManagement.Application.Users.DTOs;
using ContactManagement.Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Users.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<List<UserDto>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(
            IUserRepository userRepository,
            IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }

        public async Task<Result<List<UserDto>>> Handle(GetUsersQuery query, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetAllAsync(
                query.SearchTerm,
                query.Page,
                query.PageSize);

            return Result<List<UserDto>>.Success(_mapper.Map<List<UserDto>>(users));
        }
    }
}
