using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using ReservationManagementSystem.Application.Repositories.Interface;
using ReservationManagementSystem.Core.Entities;
using ReservationManagementSystem.Core.Objects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Application.Repositories
{
    public class RestuarantRepository : IRestuarantRepository
    {
        private readonly ILogger<RestuarantRepository> _logger;

        public RestuarantRepository(ILogger<RestuarantRepository> logger)
        {
            _logger = logger;
        }
        public async Task<Restuarant> GetRestuarantById(int Id, string SqlConn)
        {
            try
            {
                string sql = $"Select Id, Name, OpenTime, CloseTime from Restuarants where Id = @Id";

                using (IDbConnection con = new SqlConnection(SqlConn))
                {
                    var data = await con.QueryAsync<dynamic>(sql, new { Id = Id });

                    var response = data.FirstOrDefault();
                    if (response is null)
                        return new Restuarant();

                    return new Restuarant { Id = response.Id, Name = response.Name, OpenTime = TimeOnly.FromTimeSpan(response.OpenTime), CloseTime = TimeOnly.FromTimeSpan(response.CloseTime) };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);
                return new Restuarant();
            }
        }
    }
}
