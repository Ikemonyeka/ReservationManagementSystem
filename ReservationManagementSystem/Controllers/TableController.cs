using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReservationManagementSystem.Application.Services.Interface;
using ReservationManagementSystem.Core.Objects;

namespace ReservationManagementSystem.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableController : ControllerBase
    {
        private readonly ITableService _tableService;

        public TableController(ITableService tableService)
        {
            _tableService = tableService;
        }

        [HttpPost("NewTable"), Authorize]
        [ProducesResponseType(200)]
        public async Task<ActionResult> AddNewTable(TableViewModel table)
        {
            var response = await _tableService.CreateRestaurantTable(table);
            return Ok(response);
        }
    }
}
