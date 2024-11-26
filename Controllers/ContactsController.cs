using Microsoft.AspNetCore.Mvc;
using phonebookApi.Models;
using phonebookApi.Repositories.Interfaces;

namespace PhonebookApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IContactRepository _contactRepository;

        public ContactsController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
        {
            var contacts = await _contactRepository.GetAllContactsAsync();
            return Ok(contacts);
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Contact>>> SearchContacts([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return BadRequest("Search term is required");

            var contacts = await _contactRepository.SearchContactsAsync(term);
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Contact>> GetContact(int id)
        {
            var contact = await _contactRepository.GetContactByIdAsync(id);
            if (contact == null)
                return NotFound();

            return Ok(contact);
        }

        [HttpPost]
        public async Task<ActionResult<Contact>> CreateContact(Contact contact)
        {
            if (await _contactRepository.PhoneNumberExistsAsync(contact.PhoneNumber))
                return BadRequest("Phone number already exists");

            var createdContact = await _contactRepository.AddContactAsync(contact);
            return CreatedAtAction(nameof(GetContact), new { id = createdContact.Id }, createdContact);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, Contact contact)
        {
            if (id != contact.Id)
                return BadRequest();

            if (await _contactRepository.PhoneNumberExistsAsync(contact.PhoneNumber, id))
                return BadRequest("Phone number already exists");

            await _contactRepository.UpdateContactAsync(contact);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            await _contactRepository.DeleteContactAsync(id);
            return NoContent();
        }
    }
}