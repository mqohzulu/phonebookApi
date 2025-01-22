using ContactManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Application.Interfaces
{
    public interface IContactRepository
    {
        Task<Contact?> GetByIdAsync(Guid id);
        Task<List<Contact>> GetAllAsync();
        Task<List<Contact>> GetByUserIdAsync(Guid userId);
        Task AddAsync(Contact contact);
        void Update(Contact contact);
        void Delete(Contact contact);
        Task AddNoteAsync(ContactNote value);
    }
}
