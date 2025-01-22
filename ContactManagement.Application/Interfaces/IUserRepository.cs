using ContactManagement.Application.Users.DTOs;
using ContactManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByEmailAsync(string email);
        Task<bool> ExistsByEmailAsync(string email);
        Task AddAsync(User user);
        void Update(User user);
        Task<User?> GetByRefreshTokenAsync(string refreshToken);
        Task<List<UserDto>> GetAllAsync(string? searchTerm, int page, int pageSize);
    }
}
