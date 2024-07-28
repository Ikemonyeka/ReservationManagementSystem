using Azure;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
    public class AdminRepository : IAdminRepository
    {
        private readonly DataContext _context;
        private readonly ILogger _logger;

        public AdminRepository(DataContext context, ILogger<AdminRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ResponseViewModel> CreateAdmin(Admin admin)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                var data = await _context.Admins.AddAsync(admin);
                _context.SaveChanges();

                response.message = "User has been created successfully";
                response.status = true;
                response.data = "no data available";
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);
                return null;
            }
        }

        public async Task<Admin> GetAdminByEmail(string email, string SqlConn)
        {
            try
            {
                string sql = $"Select AdminId, FirstName, LastName, Username, Email, PhoneNumber, Role, IsVerified, VerificationToken, RestuarantId from Admins where email = @email";
                
                using(IDbConnection con = new SqlConnection(SqlConn))
                {
                    var data = await con.QueryAsync<Admin>(sql, new { email = email});

                    return data.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);
                return new Admin();
            }
        }

        public async Task<Admin> GetAdminByUsername(string username, string SqlConn)
        {
            try
            {
                string sql = $"Select AdminId, FirstName, LastName, Username, Email, PasswordHash, PasswordSalt, Role, RestuarantId from Admins where username = @username and IsVerified = 1";

                using (IDbConnection con = new SqlConnection(SqlConn))
                {
                    var data = await con.QueryAsync<Admin>(sql, new { username = username });

                    return data.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);
                return new Admin();
            }
        }

        public async Task<ResponseViewModel> VerifyAdminEmail(string email, string SqlConn)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                string sql = $"update Admins set IsVerified = 1  where email = @email";

                using(IDbConnection con = new SqlConnection(SqlConn))
                {
                    var data = await con.ExecuteAsync(sql, new { email = email });

                    response.message = "Verified Successfully";
                    response.status = true;
                    response.data = "no data available";
                    return response;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);
                return new ResponseViewModel();
            }
        }

        public async Task<ResponseViewModel> DeleteAdmin(int adminId, string SqlConn)
        {
            try
            {
                string sql = $"delete from Admins where adminId = @adminId";

                using (IDbConnection con = new SqlConnection(SqlConn))
                {
                    var data = await con.ExecuteAsync(sql, new { adminId = adminId });

                    return new ResponseViewModel { message = "This admin has been deleted successfully", status = true, data = "no data available" };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);
                return new ResponseViewModel();
            }
        }
    }
}
