using ContactManagement.Application.ContactGroups.Commands.AddContactToGroup;
using ContactManagement.Application.ContactGroups.Commands.DeleteContactGroup;
using ContactManagement.Application.ContactGroups.Queries.GetContactGroupQuery;
using ContactManagement.Application.ContactGroups.Queries.GetContactGroupsQuery;
using ContactManagement.Application.Contacts.Commands.CreateContact;
using ContactManagement.Application.Contacts.Commands.UpdateContact;
using ContactManagement.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace phonebookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactGroupsController : ApiControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(List<ContactGroupDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetGroups()
        {
            return HandleResult(await Mediator.Send(new GetContactGroupsQuery()));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContactGroupDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetGroup(Guid id)
        {
            return HandleResult(await Mediator.Send(new GetContactGroupQuery(id)));
        }

        [HttpPost]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateGroup(CreateContactCommand command)
        {
            var result = await Mediator.Send(command);
            if (!result.IsSuccess) return BadRequest(result.Error);

            return CreatedAtAction(
                nameof(GetGroup),
                new { id = result.Value },
                result.Value);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateGroup(Guid id, UpdateContactCommand command)
        {
            if (id != command.Id) return BadRequest("ID mismatch");

            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteGroup(Guid id)
        {
            var result = await Mediator.Send(new DeleteContactGroupCommand(id));
            if (!result.IsSuccess) return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/contacts")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddContactToGroup(Guid id, AddContactToGroupCommand command)
        {
            if (id != command.GroupId) return BadRequest("ID mismatch");

            var result = await Mediator.Send(command);
            return HandleResult(result);
        }

        [HttpDelete("{groupId}/contacts/{contactId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> RemoveContactFromGroup(Guid groupId, Guid contactId)
        {
            var command = new RemoveContactFromGroupCommand
            {
                GroupId = groupId,
                ContactId = contactId
            };

            var result = await Mediator.Send(command);

            if (result.IsFailure)
                return HandleResult(result.Error);

            return NoContent();
        }
    }
}
