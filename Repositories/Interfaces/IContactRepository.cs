using phonebookApi.Models;

namespace phonebookApi.Repositories.Interfaces
{
    public interface IContactRepository
    {
        Task<IEnumerable<Contact>> GetAllContactsAsync();
        Task<Contact> GetContactByIdAsync(int id);
        Task<IEnumerable<Contact>> SearchContactsAsync(string searchTerm);
        Task<Contact> AddContactAsync(Contact contact);
        Task UpdateContactAsync(Contact contact);
        Task DeleteContactAsync(int id);
        Task<bool> PhoneNumberExistsAsync(string phoneNumber, int? excludeId = null);
    }
}
