using ContactManagement.Application.Interfaces;
using ContactManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Infrastructure.Persistance.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Contact?> GetByIdAsync(Guid id)
        {
            return await _context.Contacts
                .Include(c => c.Address)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Contact>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Contacts
                .Include(c => c.Address)
                .Where(c => c.UserId == userId)
                .ToListAsync();
        }

        public async Task AddAsync(Contact contact)
        {
            await _context.Contacts.AddAsync(contact);
        }

        public void Update(Contact contact)
        {
            _context.Entry(contact).State = EntityState.Modified;
        }

        public void Delete(Contact contact)
        {
            _context.Contacts.Remove(contact);
        }

        public Task<List<Contact>> GetAllAsync()
        {
            throw new NotImplementedException();
        }
    }
}
