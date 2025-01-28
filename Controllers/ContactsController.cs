using ContactManagement.Application.Contacts.Commands.AddContactNote;
using ContactManagement.Application.Contacts.Commands.CreateContact;
using ContactManagement.Application.Contacts.Commands.DeleteContact;
using ContactManagement.Application.Contacts.Commands.UpdateContact;
using ContactManagement.Application.Contacts.Queries.GetContact;
using ContactManagement.Application.Contacts.Queries.GetContacts;
using ContactManagement.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using phonebookApi.Controllers;

namespace PhonebookApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<ContactDto>>> GetContacts([FromQuery] GetContactsQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContact(Guid id)
        {
            return Ok(await Mediator.Send(new GetContactQuery(id)));
        }

        [HttpPost]
        public async Task<IActionResult> CreateContact(CreateContactCommand command)
        {
            var result = await Mediator.Send(command);
            return CreatedAtAction(nameof(GetContact), new { id = result.Value }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(Guid id, UpdateContactCommand command)
        {
            if (id != command.Id)
                return BadRequest();

            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            await Mediator.Send(new DeleteContactCommand(id));
            return NoContent();
        }

        [HttpPost("{id}/notes")]
        public async Task<IActionResult> AddNote(Guid id, AddContactNoteCommand command)
        {
            if (id != command.ContactId)
                return BadRequest();

            return Ok(await Mediator.Send(command));
        }
    }

    //public class ContactsController : ControllerBase
    //{
    //    private readonly IContactRepository _contactRepository;

    //    public ContactsController(IContactRepository contactRepository)
    //    {
    //        _contactRepository = contactRepository;
    //    }

    //    [HttpGet]
    //    public async Task<ActionResult<IEnumerable<Contact>>> GetContacts()
    //    {
    //        var contacts = await _contactRepository.GetAllContactsAsync();
    //        return Ok(contacts);
    //    }

    //    [HttpGet("search")]
    //    public async Task<ActionResult<IEnumerable<Contact>>> SearchContacts([FromQuery] string term)
    //    {
    //        if (string.IsNullOrWhiteSpace(term))
    //            return BadRequest("Search term is required");

    //        var contacts = await _contactRepository.SearchContactsAsync(term);
    //        return Ok(contacts);
    //    }

    //    [HttpGet("{id}")]
    //    public async Task<ActionResult<Contact>> GetContact(int id)
    //    {
    //        var contact = await _contactRepository.GetContactByIdAsync(id);
    //        if (contact == null)
    //            return NotFound();

    //        return Ok(contact);
    //    }

    //    [HttpPost]
    //    public async Task<ActionResult<Contact>> CreateContact(Contact contact)
    //    {
    //        if (await _contactRepository.PhoneNumberExistsAsync(contact.PhoneNumber))
    //            return BadRequest("Phone number already exists");

    //        var createdContact = await _contactRepository.AddContactAsync(contact);
    //        return CreatedAtAction(nameof(GetContact), new { id = createdContact.Id }, createdContact);
    //    }

    //    [HttpPut("{id}")]
    //    public async Task<IActionResult> UpdateContact(int id, Contact contact)
    //    {
    //        if (id != contact.Id)
    //            return BadRequest();

    //        if (await _contactRepository.PhoneNumberExistsAsync(contact.PhoneNumber, id))
    //            return BadRequest("Phone number already exists");

    //        await _contactRepository.UpdateContactAsync(contact);
    //        return NoContent();
    //    }

    //    [HttpDelete("{id}")]
    //    public async Task<IActionResult> DeleteContact(int id)
    //    {
    //        await _contactRepository.DeleteContactAsync(id);
    //        return NoContent();
    //    }
    //}
}