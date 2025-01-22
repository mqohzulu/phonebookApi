using ContactManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Common.Interfaces
{
    public interface IContactGroupRepository
    {
        Task<ContactGroup?> GetByIdAsync(Guid id);
        Task<List<ContactGroup>> GetByUserIdAsync(Guid userId);
        Task AddAsync(ContactGroup group);
        void Update(ContactGroup group);
        Task<bool> ExistsByNameAsync(string name, Guid userId);
        Task<int> GetContactCountAsync(Guid groupId);
        Task DeleteAsync(ContactGroup group);
        Task<bool> RemoveContactFromGroupAsync(Guid groupId, Guid contactId);
    }
}
