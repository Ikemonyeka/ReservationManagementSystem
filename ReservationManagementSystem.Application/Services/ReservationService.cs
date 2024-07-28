using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ReservationManagementSystem.Application.Repositories.Interface;
using ReservationManagementSystem.Application.Services.Interface;
using ReservationManagementSystem.Core.Entities;
using ReservationManagementSystem.Core.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Application.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IReservationTimeRepository _reservationTimeRepository;
        private readonly IConfiguration _configuration;
        private readonly IReservationRepository _reservationRepository;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly ITableRepository _tableRepository;
        private readonly IRestuarantRepository _restuarantRepository;
        private readonly ILogger<ReservationService> _logger;
        private readonly string SqlConn;

        public ReservationService(IHttpContextAccessor httpContextAccessor, IReservationTimeRepository reservationTimeRepository, IConfiguration configuration, IReservationRepository reservationRepository, IEmailService emailService, IUserRepository userRepository, ITableRepository tableRepository, IRestuarantRepository restuarantRepository, ILogger<ReservationService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _reservationTimeRepository = reservationTimeRepository;
            _configuration = configuration;
            _reservationRepository = reservationRepository;
            _emailService = emailService;
            _userRepository = userRepository;
            _tableRepository = tableRepository;
            _restuarantRepository = restuarantRepository;
            _logger = logger;
            SqlConn = _configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<ResponseViewModel> NewReservation(ReservationViewModel model)
        {
            try
            {
                if (model.TableId is 0)
                    return new ResponseViewModel { message = "All fields are required", status = false, data = "No data available" };

                int UserId = Convert.ToInt32(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.SerialNumber).Value);
                if (UserId is 0)
                    return new ResponseViewModel { message = "Could not authenticate user", status = false, data = "No data available" };

                var TableInfo = await _tableRepository.GetTableById(model.TableId, SqlConn);
                var ReservationInfo = await _reservationRepository.GetReservationByTableId(model.TableId, model.ReservationDate, SqlConn);

                if (model.PartySize > TableInfo.PartySize)
                    return new ResponseViewModel { message = "Select a table that will fit your party size", status = false, data = "No data available" };

                if(TableInfo.TableQuantity <= ReservationInfo.Count())
                    return new ResponseViewModel { message = "Table not available", status = false, data = "No data available" };

                TimeOnly.TryParse(model.StartTime, out TimeOnly startTime);
                var timeSlotInfo = await _reservationTimeRepository.GetTimeSlotById(model.TimeId, SqlConn);

                int hours = timeSlotInfo.Duration.Hour;
                int minutes = timeSlotInfo.Duration.Minute;
                int durationConverted = (hours * 60) + minutes;

                Reservation reservation = new Reservation
                {
                    UserId = UserId,
                    TableId = model.TableId,
                    ReservationDate = model.ReservationDate.Date,
                    StartTime = startTime,
                    EndTime = startTime.AddMinutes(durationConverted),
                    SpecialRequest = model.SpecialRequest,
                    PartySize = model.PartySize
                };

                var createdReservation = await _reservationRepository.NewReservation(reservation);
                if (createdReservation.status is false)
                    return new ResponseViewModel { message = $"Reservation not added...{createdReservation.message}", status = false, data = "No data available" };

                var userInfo = await _userRepository.GetUserById(UserId, SqlConn);
                var restuarantInfo = await _restuarantRepository.GetRestuarantById(TableInfo.RestuarantId, SqlConn);

                ReservationCompleteViewModel reservationComplete = new ReservationCompleteViewModel
                {
                    Email = userInfo.Email,
                    Name = userInfo.FirstName + " " + userInfo.LastName,
                    ReservationDate = model.ReservationDate.Date,
                    StartTime = startTime,
                    EndTime = startTime.AddMinutes(durationConverted),
                    TableType = TableInfo.TableName,
                    PartySize = model.PartySize,
                    SpecialRequests = model.SpecialRequest,
                    RestuarantName = restuarantInfo.Name
                };

                var reservationEmail = await _emailService.ReservationComplete(reservationComplete);
                if (reservationEmail.status is false)
                    return new ResponseViewModel { message = $"Email not sent...{createdReservation.message}", status = false, data = "No data available" };

                return new ResponseViewModel { message = $"Reservation added successfully", status = true, data = "No data available" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);
                return new ResponseViewModel();
            }
        }
    }
}
