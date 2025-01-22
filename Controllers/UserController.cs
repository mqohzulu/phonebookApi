using ContactManagement.Application.Authentication.Comnands.RefreshToken;
using ContactManagement.Application.Users.Commands.CreateUser;
using ContactManagement.Application.Users.Commands.DeleteUser;
using ContactManagement.Application.Users.Commands.UpdateUser;
using ContactManagement.Application.Users.DTOs;
using ContactManagement.Application.Users.Queries.GetUser;
using ContactManagement.Application.Users.Queries.GetUsers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace phonebookApi.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetUsers([FromQuery] GetUsersQuery query)
        {
            return Ok(await Mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(Guid id)
        {
            return Ok(await Mediator.Send(new GetUserQuery(id)));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserCommand command)
        {
            if (id != command.Id)
                return BadRequest();

            return Ok(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await Mediator.Send(new DeleteUserCommand(id));
            return NoContent();
        }

 
    }
}
