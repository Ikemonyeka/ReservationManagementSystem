using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReservationManagementSystem.Application.Repositories.Interface;
using ReservationManagementSystem.Application.Services.Interface;
using ReservationManagementSystem.Core.Entities;
using ReservationManagementSystem.Core.Enums;
using ReservationManagementSystem.Core.Objects;
using ReservationManagementSystem.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Application.Services
{
    public class ReservationTimeService : IReservationTimeService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAdminRepository _adminRepository;
        private readonly IRestuarantRepository _restuarantRepository;
        private readonly IReservationTimeRepository _reservationTimeRepository;
        private readonly ILogger<ReservationTimeService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string SqlConn;

        public ReservationTimeService(IHttpContextAccessor httpContextAccessor, IAdminRepository adminRepository, IConfiguration configuration, IRestuarantRepository restuarantRepository, IReservationTimeRepository reservationTimeRepository, ILogger<ReservationTimeService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _adminRepository = adminRepository;
            _configuration = configuration;
            _restuarantRepository = restuarantRepository;
            _reservationTimeRepository = reservationTimeRepository;
            _logger = logger;
            SqlConn = _configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<ResponseViewModel> AddTimeSlot(TimeViewModel model)
        {
            try
            {
                TimeOnly.TryParse(model.Time, out TimeOnly time);
                TimeOnly.TryParse(model.Duration, out TimeOnly duration);

                string adminUsername = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.Name)?.Value;
                if (adminUsername is null)
                    return new ResponseViewModel { message = "Contact your admin to resolve this", status = false, data = "No data available" };

                int adminRole = Convert.ToInt32(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.Role)?.Value);
                if (adminRole is not (int)Roles.PrimaryAdmin && adminRole is not (int)Roles.SecoondaryAdmin)
                    return new ResponseViewModel { message = "Permission denied", status = false, data = "No data available" };

                var adminInfo = await _adminRepository.GetAdminByUsername(adminUsername, SqlConn);
                if (adminInfo.RestuarantId is null)
                    return new ResponseViewModel { message = "Admin must be profiled to a restuarant", status = false, data = "No data available" };

                var restuarantDetails = await _restuarantRepository.GetRestuarantById((int)adminInfo.RestuarantId, SqlConn);
                if (restuarantDetails is null)
                    return new ResponseViewModel { message = "Could not determine restuarant details", status = false, data = "No data available" };

                int hours = duration.Hour;
                int minutes =duration.Minute;
                int durationConverted = (hours * 60) + minutes;

                if (time < restuarantDetails.OpenTime || time.AddMinutes(durationConverted) > restuarantDetails.CloseTime)
                    return new ResponseViewModel { message = "The time provided does not fall within opening and clsoing time of the restuarant", status = false, data = "No data available" };

                ReservationTimeSlot reservationTime = new ReservationTimeSlot
                {
                    TimeSlot = time,
                    Duration = duration,
                    RestuarantId = (int)adminInfo.RestuarantId,
                };

                var data = await _reservationTimeRepository.CreateTimeSlot(reservationTime);
                if (data is null)
                    return new ResponseViewModel { message = "not able to add new time slot, please try again later.", status = false, data = "No data available" };

                return new ResponseViewModel { message = "Time slot created", status = true, data = "No data available" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);
                return new ResponseViewModel { message = $"Time slot not created...{ex.Message}", status = false, data = "No data available" };
            }
        }
    }
}
