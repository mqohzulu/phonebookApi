using ContactManagement.Application.ContactGroups.Commands.AddContactToGroup;
using ContactManagement.Application.ContactGroups.Commands.DeleteContactGroup;
using ContactManagement.Application.ContactGroups.Commands.RemoveContactFromGroup;
using ContactManagement.Application.ContactGroups.Queries.GetContactGroupQuery;
using ContactManagement.Application.ContactGroups.Queries.GetContactGroupsQuery;
using ContactManagement.Application.Contacts.Commands.CreateContact;
using ContactManagement.Application.Contacts.Commands.UpdateContact;
using ContactManagement.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace phonebookApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ContactGroupsController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<ContactGroupDto>>> GetGroups()
        {
            return Ok(await Mediator.Send(new GetContactGroupsQuery()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContactGroupDto>> GetGroup(Guid id)
        {
            return Ok(await Mediator.Send(new GetContactGroupQuery(id)));
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroup(CreateContactCommand command)
        {
            var result = await Mediator.Send(command);
            return CreatedAtAction(
                nameof(GetGroup),
                new { id = result.Value },
                result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroup(Guid id, UpdateContactCommand command)
        {
            if (id != command.Id)
                return BadRequest();

            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(Guid id)
        {
            await Mediator.Send(new DeleteContactGroupCommand(id));
            return NoContent();
        }

        [HttpPost("{id}/contacts")]
        public async Task<IActionResult> AddContactToGroup(Guid id, AddContactToGroupCommand command)
        {
            if (id != command.GroupId)
                return BadRequest();

            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{groupId}/contacts/{contactId}")]
        public async Task<IActionResult> RemoveContactFromGroup(Guid groupId, Guid contactId)
        {
            var command = new RemoveContactFromGroupCommand
            {
                GroupId = groupId,
                ContactId = contactId
            };

            await Mediator.Send(command);
            return NoContent();
        }
    }
}
