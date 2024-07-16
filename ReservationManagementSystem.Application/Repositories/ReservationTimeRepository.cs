using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Logging;
using ReservationManagementSystem.Application.Repositories.Interface;
using ReservationManagementSystem.Core.Entities;
using ReservationManagementSystem.Core.Objects;
using ReservationManagementSystem.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Application.Repositories
{
    public class ReservationTimeRepository : IReservationTimeRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ReservationTimeRepository> _logger;

        public ReservationTimeRepository(DataContext context, ILogger<ReservationTimeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<ResponseViewModel> CreateTimeSlot(ReservationTimeSlot reservationTime)
        {
            try
            {
                var data = await _context.ReservationTimeSlots.AddAsync(reservationTime);
                _context.SaveChanges();

                return new ResponseViewModel { message = "Time slot has been added", status = true, data = "No data available" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);
                return new ResponseViewModel();
            }
        }
    }
}
