using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ReservationManagementSystem.Application.Repositories.Interface;
using ReservationManagementSystem.Core.Objects;
using ReservationManagementSystem.Models.Entities;
using ReservationManagementSystem.Services.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ReservationManagementSystem.Core.Entities;

namespace ReservationManagementSystem.Application.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(DataContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task CreateUser(SignUpViewModel signUp)
        {

            CreatePasswordHash(signUp.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User userinfo = new User
            {
                FirstName = signUp.FirstName,
                LastName = signUp.LastName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Email = signUp.Email,
                Address = signUp.Address,
                PhoneNumber = signUp.PhoneNumber,
                DateCreated = DateTime.Now,
                VerificationToken = signUp.VerificationToken
            };

            await _context.Users.AddAsync(userinfo);
            _context.SaveChanges();

        }

        public async Task<object> VerifyEmail(string email, string token)
        {
           ResponseViewModel response = new ResponseViewModel();

           var user = await  _context.Users.Where(x => x.Email == email && x.VerificationToken == token).FirstAsync();
           
           if(user is null)
           {
                return null;
           }

           user.IsVerified = true;
           user.DateUpdated = DateTime.Now;
           await _context.SaveChangesAsync();

           response.message = "User has been verified successfully";
           response.status = true;
           response.data = "no data available";
           return response;
        }

        public async Task<User> GetUserByEmail(string email, string SqlConn)
        {
            try
            {
                string sql = $"Select UserId, FirstName, LastName, Username, Email, PasswordHash, PasswordSalt, IsVerified, PasswordResetToken, ResetTokenExpires from users where email = email";

                using (IDbConnection con = new SqlConnection(SqlConn))
                {
                    var data = await con.QueryAsync<User>(sql, new { email = email });

                    return data.First();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);
                return null;
            }
        }

        public async Task<User> GetUserById(int Id, string SqlConn)
        {
            try
            {
                string sql = $"Select UserId, FirstName, LastName, Username, Email, PasswordHash, PasswordSalt, PasswordResetToken, ResetTokenExpires from Users where UserId = @UserId and IsVerified = 1";

                using (IDbConnection con = new SqlConnection(SqlConn))
                {
                    var data = await con.QueryAsync<User>(sql, new { UserId = Id });

                    return data.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);
                return new User();
            }
        }

        public async Task<ResponseViewModel> ForgotPasswordToken(ForgotPasswordEmailViewModel forgotPassword, string SqlConn)
        {
            try
            {
                string sql = $"update users set PasswordResetToken = @PasswordResetToken, ResetTokenExpires = @ResetTokenExpires where UserId = @UserId";
                using (IDbConnection con = new SqlConnection(SqlConn))
                {
                    var data = await con.ExecuteAsync(sql, 
                        new { PasswordResetToken = forgotPassword.PasswordResetToken, ResetTokenExpires = forgotPassword.ResetTokenExpires, UserId = forgotPassword.Id });

                    return new ResponseViewModel { message = "Token saved", status = true, data = "no data available" };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);
                return new ResponseViewModel { message = "Token not saved", status = false, data = "no data available" };
            }
        }

        public async Task<ResponseViewModel> UpdateUserPassword(HashForgottenPassword hash, string SqlConn)
        {
            try
            {
                string sql = $"update users set PasswordHash = @PasswordHash, PasswordSalt = @PasswordSalt where UserId = @UserId";
                using (IDbConnection con = new SqlConnection(SqlConn))
                {
                    var data = await con.ExecuteAsync(sql,
                        new { PasswordHash = hash.PasswordHash, PasswordSalt = hash.PasswordSalt, UserId = hash.Id });

                    return new ResponseViewModel { message = "Password changed", status = true, data = "no data available" };
                }


                //if (obj is null)
                //    return new ResponseViewModel { message = "object doesn't have data", status = false, data = "no data available" };

                //User user = obj as User;

                //if (user is null)
                //    return new ResponseViewModel { message = "object doesn't have data", status = false, data = "no data available" };

                //await _context.Users.AddAsync(user);
                //_context.SaveChanges();

                //_logger.LogInformation("user details updated, TimeStamp: " + DateTime.Now);
                //return new ResponseViewModel { message = "user updated", status = true, data = "no data available" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);
                return new ResponseViewModel();
            }
        }
    }
}
