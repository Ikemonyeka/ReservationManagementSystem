using Dapper;
using Microsoft.Data.SqlClient;
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
    public class ReservationRepository : IReservationRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<ReservationRepository> _logger;

        public ReservationRepository(DataContext context, ILogger<ReservationRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Reservation>> GetReservationByTableId(int Id, DateTime ReservationDate, string SqlConn)
        {
            try
            {
                string sql = $"Select ReservationId, TableId, ReservationDate, StartTime, EndTime from Reservations where ReservationId = @Id and ReservationDate = @ReservationDate";

                using (IDbConnection con = new SqlConnection(SqlConn))
                {
                    var data = await con.QueryAsync<Reservation>(sql, new { Id = Id, ReservationDate = ReservationDate });

                    return data.ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);
                return new List<Reservation>();
            }
        }

        public async Task<ResponseViewModel> NewReservation(Reservation reservation)
        {
            try
            {
                var data = await _context.Reservations.AddAsync(reservation);
                _context.SaveChanges();

                return new ResponseViewModel { message = "Reservation successful", status = true, data = "No data available" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);
                return new ResponseViewModel();
            }
        }
    }
}
