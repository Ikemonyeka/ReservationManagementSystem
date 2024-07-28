using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservationManagementSystem.Application.Services.Interface;
using ReservationManagementSystem.Core.Objects;

namespace ReservationManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationTimeSlotController : ControllerBase
    {
        private readonly IReservationTimeService _reservationTimeService;

        public ReservationTimeSlotController(IReservationTimeService reservationTimeService)
        {
            _reservationTimeService = reservationTimeService;
        }

        [HttpPost("CreateTimeSlot")]
        [ProducesResponseType(200), Authorize]
        public async Task<ActionResult> CreateTimeSlot(TimeViewModel model)
        {
            var response = await _reservationTimeService.AddTimeSlot(model);
            return Ok(response);
        }
    }
}
