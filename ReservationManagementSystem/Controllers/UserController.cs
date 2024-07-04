using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservationManagementSystem.Application.Services.Interface;
using ReservationManagementSystem.Core.Objects;

namespace ReservationManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("SignUp")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> SignUpUser(SignUpViewModel signUp)
        {
            var response = await _userService.SignUp(signUp);

            return Ok(response);
        }
    }
}
