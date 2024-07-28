using Azure;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Logging;
using ReservationManagementSystem.Application.Repositories.Interface;
using ReservationManagementSystem.Core.Entities;
using ReservationManagementSystem.Core.Objects;
using ReservationManagementSystem.Services.Data;
using System;
using System.Collections.Generic;
using System.Data;
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

        public async Task<TimeByIdViewModel> GetTimeSlotById(int Id, string SqlConn)
        {
            try
            {
                string sql = $"Select Id, TimeSlot, Duration from ReservationTimeSlots where Id = @Id";

                using (IDbConnection con = new SqlConnection(SqlConn))
                {
                    var data = await con.QueryAsync<dynamic>(sql, new { Id = Id });

                    var response = data.FirstOrDefault();
                    if (response is null)
                        return new TimeByIdViewModel();

                    return new TimeByIdViewModel { Id = response.Id, Time = TimeOnly.FromTimeSpan(response.TimeSlot), Duration = TimeOnly.FromTimeSpan(response.Duration) };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);
                return new TimeByIdViewModel();
            }
        }
    }
}
