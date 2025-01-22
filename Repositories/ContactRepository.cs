using Microsoft.EntityFrameworkCore;
using phonebookApi.Models;
using phonebookApi.Repositories.Interfaces;
using PhonebookApp.API.Data;

namespace phonebookApi.Repositories
{

    public class ContactRepository : IContactRepository
    {
        private readonly PhonebookContext _context;

        public ContactRepository(PhonebookContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contact>> GetAllContactsAsync()
        {
            return await _context.Contacts.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<Contact> GetContactByIdAsync(int id)
        {
            return await _context.Contacts.FindAsync(id);
        }

        public async Task<IEnumerable<Contact>> SearchContactsAsync(string searchTerm)
        {
            return await _context.Contacts
                .Where(c => c.Name.Contains(searchTerm) ||
                            c.PhoneNumber.Contains(searchTerm))
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Contact> AddContactAsync(Contact contact)
        {
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return contact;
        }

        public async Task UpdateContactAsync(Contact contact)
        {
            _context.Entry(contact).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteContactAsync(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact != null)
            {
                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> PhoneNumberExistsAsync(string phoneNumber, int? excludeId = null)
        {
            return await _context.Contacts
                .AnyAsync(c => c.PhoneNumber == phoneNumber &&
                                (!excludeId.HasValue || c.Id != excludeId.Value));
        }
    }
    
}
