using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservationManagementSystem.Application.Repositories.Interface;
using ReservationManagementSystem.Application.Services;
using ReservationManagementSystem.Application.Services.Interface;
using ReservationManagementSystem.Core.Objects;

namespace ReservationManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("CreateAdmin"), Authorize]
        [ProducesResponseType(200)]
        public async Task<ActionResult> CreateAdmin(AdminViewModel admin)
        {
            var response = await _adminService.CreateAdmin(admin);
            return Ok(response);
        }

        [HttpPost("AdminLogin")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> LoginAdmin(AdminLoginViewModel adminLogin)
        {
            var response = await _adminService.LoginAdmin(adminLogin);
            return Ok(response);
        }

        [HttpGet("VerifyAdmin")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> VerifySignUpUser(string Email, string UToken)
        {
            var response = await _adminService.VerifyAdmin(Email, UToken);
            return Ok(response);
        }

        [HttpDelete("DeleteAdmin")]
        [ProducesResponseType(200), Authorize]
        public async Task<ActionResult> DeleteAdmin(int adminId)
        {
            var response = _adminService.DeleteAdmin(adminId);
            return Ok(response);
        }
    }
}
