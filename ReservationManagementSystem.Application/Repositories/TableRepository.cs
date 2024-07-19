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
    public class TableRepository : ITableRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<TableRepository> _logger;

        public TableRepository(DataContext context, ILogger<TableRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ResponseViewModel> AddRestuarantTable(Table table)
        {
            try
            {
                await _context.Tables.AddAsync(table);
                _context.SaveChanges();
                return new ResponseViewModel { message = "Table added", status = true, data = "no data available"};
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);
                return new ResponseViewModel { message = "Table not added", status = true, data = "no data available" };
            }
        }

        public async Task<Table> GetTableById(int Id, string SqlConn)
        {
            try
            {
                string sql = $"Select TableId, TableName, TableQuantity, RestuarantId, PartySize from Tables where IsActive = 1";

                using (IDbConnection con = new SqlConnection(SqlConn))
                {
                    var data = await con.QueryAsync<Table>(sql, new { UserId = Id });

                    return data.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);
                return new Table();
            }
        }
    }
}
