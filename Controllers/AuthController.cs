using ContactManagement.Application.Authentication.Commands.Register;
using ContactManagement.Application.Authentication.Common;
using ContactManagement.Application.Authentication.Common.Interfaces;
using ContactManagement.Application.Authentication.Comnands.Login;
using ContactManagement.Application.Users.Commands.CreateUser;
using ContactManagement.Domain.Common;
using ContactManagement.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace phonebookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand request)
        {
            var result = await _authService.AuthenticateAsync(request.Email, request.Password);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand request)
        {
            var command = new CreateUserCommand
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = request.Password
            };

            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _authService.RefreshTokenAsync(Guid.Parse(userId).ToString());
            return Ok(result);
        }
    }

}
