using ContactManagement.Application.Interfaces;
using ContactManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactManagement.Infrastructure.Persistance.Repositories
{
    public class ContactGroupRepository : IContactGroupRepository
    {
        private readonly PhonebookContext _context;
        private readonly ILogger<ContactGroupRepository> _logger;

        public ContactGroupRepository(PhonebookContext context,ILogger<ContactGroupRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ContactGroup?> GetByIdAsync(Guid id)
        {
            return await _context.Set<ContactGroup>()
                .Include(g => g.Contacts)  // Since we have a private field
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<List<ContactGroup>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Set<ContactGroup>()
                .Include(g => g.Contacts)  // Since we have a private field
                .Where(g => g.UserId == userId)
                .OrderBy(g => g.Name)
                .ToListAsync();
        }

        public async Task AddAsync(ContactGroup group)
        {
            try
            {
                await _context.Set<ContactGroup>().AddAsync(group);
                _logger.LogInformation("Contact group created: {GroupId}", group.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating contact group");
                throw;
            }
        }

        public void Update(ContactGroup group)
        {
            try
            {
                _context.Set<ContactGroup>().Update(group);
                _logger.LogInformation("Contact group updated: {GroupId}", group.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating contact group {GroupId}", group.Id);
                throw;
            }
        }

        public async Task<bool> ExistsByNameAsync(string name, Guid userId)
        {
            return await _context.Set<ContactGroup>()
                .AnyAsync(g =>
                    g.Name.ToLower() == name.ToLower() &&
                    g.UserId == userId);
        }

        public async Task<int> GetContactCountAsync(Guid groupId)
        {
            return await _context.Set<ContactGroup>()
                .Where(g => g.Id == groupId)
                .Select(g => g.Contacts.Count)
                .FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(ContactGroup group)
        {
            try
            {
                _context.Set<ContactGroup>().Remove(group);
                _logger.LogInformation("Contact group deleted: {GroupId}", group.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting contact group {GroupId}", group.Id);
                throw;
            }
        }

        public async Task<bool> RemoveContactFromGroupAsync(Guid groupId, Guid contactId)
        {
            try
            {
                var group = await _context.Set<ContactGroup>()
                    .Include(g => g.Contacts)
                    .FirstOrDefaultAsync(g => g.Id == groupId);

                if (group == null) return false;

                var contact = group.Contacts.FirstOrDefault(c => c.Id == contactId);
                if (contact == null) return false;

                var entry = _context.Entry(group);
                entry.Collection("_contacts").Load();
                ((List<Contact>)entry.Property("_contacts").CurrentValue).Remove(contact);

                _logger.LogInformation(
                    "Contact {ContactId} removed from group {GroupId}",
                    contactId, groupId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error removing contact {ContactId} from group {GroupId}",
                    contactId, groupId);
                throw;
            }
        }
    }

}
