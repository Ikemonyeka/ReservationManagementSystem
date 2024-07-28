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
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("SignUp")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> SignUpUser(SignUpViewModel signUp)
        {
            var response = await _userService.SignUp(signUp);

            _logger.LogInformation(response.message);
            return Ok(response);
        }

        [HttpGet("VerifyUser")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> VerifySignUpUser(string Email, string UToken)
        {
            var response = await _userService.VerifyUser(Email, UToken);
                
            return Ok(response);
        }

        [HttpPost("SignIn")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> SignInUser(SignInViewModel signIn)
        {
            var response = await _userService.SignIn(signIn);

            return Ok(response);
        }

        [HttpPost("ForgotPasswordEmail")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> ForgotPasswordEmail(string email)
        {
            var response = await _userService.ForgotPasswordEmail(email);

            return Ok(response);
        }

        [HttpPut("UpdatePassword")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> UpdateUserPassword(UpdateForgottenPassword update)
        {
            var response = await _userService.ChangeUserPassword(update);

            return Ok(response);
        }
    }
}
