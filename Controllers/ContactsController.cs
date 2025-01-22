using ContactManagement.Application.Contacts.Commands.CreateContact;
using ContactManagement.Application.Contacts.Queries.GetContact;
using ContactManagement.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using phonebookApi.Controllers;
using phonebookApi.Models;
using phonebookApi.Repositories.Interfaces;

namespace PhonebookApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ContactsController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(List<ContactDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetContacts([FromQuery] GetContactQuery query)
        {
            return HandleResult(await Mediator.Send(query));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetContact(Guid id)
        {
            return HandleResult(await Mediator.Send(new GetContactQuery(id)));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateContact(CreateContactCommand command)
        {
            var result = await Mediator.Send(command);
            if (!result.IsSuccess) return BadRequest(result.Error);

            return CreatedAtAction(
                nameof(GetContact),
                new { id = result.Value },
                result.Value);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateContact(Guid id, UpdateContactCommand command)
        {
            if (id != command.Id) return BadRequest("ID mismatch");

            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteContact(Guid id)
        {
            var result = await Mediator.Send(new DeleteContactCommand(id));
            if (!result.IsSuccess) return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/notes")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddNote(Guid id, AddContactNoteCommand command)
        {
            if (id != command.ContactId) return BadRequest("ID mismatch");

            var result = await Mediator.Send(command);
            return HandleResult(result);
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