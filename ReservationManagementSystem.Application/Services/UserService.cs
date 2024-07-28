using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ReservationManagementSystem.Application.Repositories.Interface;
using ReservationManagementSystem.Application.Services.Interface;
using ReservationManagementSystem.Core.Objects;
using ReservationManagementSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserService> _logger;
        private readonly string SqlConn;


        public UserService(IEmailService emailService, IUserRepository userRepository, IConfiguration configuration, ILogger<UserService> logger)
        {
            _emailService = emailService;
            _userRepository = userRepository;
            _configuration = configuration;
            _logger = logger;
            SqlConn = _configuration.GetConnectionString("DefaultConnection");
        }

        private static string VerficationGen()
        {
            const string src = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            int length = 40;
            var sb = new StringBuilder();
            Random RNG = new Random();
            for (var i = 0; i < length; i++)
            {
                var c = src[RNG.Next(0, src.Length)];
                sb.Append(c);
            }
            return sb.ToString();
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                 new Claim(ClaimTypes.SerialNumber, user.UserId.ToString("")),
                new Claim(ClaimTypes.Name, user.Email),
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<ResponseViewModel> SignUp(SignUpViewModel signUp)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                if(string.IsNullOrWhiteSpace(signUp.FirstName) || string.IsNullOrWhiteSpace(signUp.LastName) || string.IsNullOrWhiteSpace(signUp.Email) || string.IsNullOrWhiteSpace(signUp.Address))
                {
                    response.message = "Some fields are still empty";
                    response.status = false;
                    response.data = "No data available";
                    return response;
                }

                if (!signUp.Password.Equals(signUp.ConfirmPassword))
                {
                    response.message = "Passwords do not match";
                    response.status = false;
                    response.data = "No data available";
                    return response;
                }

                signUp.VerificationToken = VerficationGen();
                await _userRepository.CreateUser(signUp);
                await _emailService.UserVerifcationEmail(signUp.Email, signUp.VerificationToken);

                response.message = "User Created Successfully";
                response.status = true;
                response.data = "No data available";
                return response;
            }
            catch (Exception ex) 
            {

                return response;
            }
        }

        public async Task<ResponseViewModel> VerifyUser(string email, string token)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                if(string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
                {
                    response.message = "All fields are required";
                    response.status = false;
                    response.data = "no data available";

                    return response;
                }

                var verificationResponse = await _userRepository.VerifyEmail(email, token);
                
                if(verificationResponse is false)
                {
                    response.message = "Verification Unsuccessful";
                    response.status = false;
                    response.data = "no data available";

                    return response;
                }

                response.message = "Verification Successful";
                response.status = true;
                response.data = "no data available";
                return response;
            }
            catch(Exception ex)
            {
                return response;
            }
        }

        public async Task<ResponseViewModel> SignIn(SignInViewModel signIn)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                if (string.IsNullOrWhiteSpace(signIn.Email))
                    return new ResponseViewModel { message = "email is incorrect", status = false, data = "no data available" };

                var user = await _userRepository.GetUserByEmail(signIn.Email, SqlConn);

                if(user is null)
                {
                    response.message = "User doesn't exist";
                    response.status = false;
                    response.data = "no data available";

                    return response;
                }

                if(user.IsVerified is false)
                {
                    response.message = "User is not verified, check your email";
                    response.status = false;
                    response.data = "no data available";

                    return response;
                }

                if(!VerifyPasswordHash(signIn.Password, user.PasswordHash, user.PasswordSalt))
                {
                    response.message = "Incorrect Username or Password";
                    response.status = false;
                    response.data = "no data available";

                    return response;
                }

                var token = CreateToken(user);

                response.message = "Login Successful";
                response.status = true;
                response.data = token;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);

                response.message = ex.Message;
                response.status = false;
                response.data = "No data available";

                return response;
            }
        }

        public async Task<ResponseViewModel> ForgotPasswordEmail(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                    return new ResponseViewModel { message = "email is incorrect", status = false, data = "no data available" };

                var userInfo = await _userRepository.GetUserByEmail(email, SqlConn);
                if(userInfo is null)
                    return new ResponseViewModel { message = "email not found", status = false, data = "no data available" };

                var passwordForgotten = new ForgotPasswordEmailViewModel
                {
                    Id = userInfo.UserId,
                    PasswordResetToken = VerficationGen(),
                    ResetTokenExpires = DateTime.Now.AddHours(24),
                };

                var tokenResponse = await _userRepository.ForgotPasswordToken(passwordForgotten, SqlConn);
                if(tokenResponse is null)
                    return new ResponseViewModel { message = "token not saved, try again", status = false, data = "no data available" };

                var emailResponse = await _emailService.ForgotPasswordEmail(userInfo.Email, passwordForgotten.PasswordResetToken);

                if(emailResponse.status is false)
                    return new ResponseViewModel { message = "email not sent, try again", status = false, data = "no data available" };

                return new ResponseViewModel { message = "Token generated and email sent", status = true, data = "no data available" };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<ResponseViewModel> ChangeUserPassword(UpdateForgottenPassword update)
        {
            try
            {
                if(update.Email is null || update.UToken is null || update.Password is null || update.ConfirmPassword is null)
                    return new ResponseViewModel { message = "All fields are required", status = false, data = "no data available" };

                if(!update.Password.Equals(update.ConfirmPassword))
                    return new ResponseViewModel { message = "password and confirm password must match", status = false, data = "no data available" };

                var userInfo = await _userRepository.GetUserByEmail(update.Email, SqlConn);
                if(userInfo is null)
                    return new ResponseViewModel { message = "user could not be found", status = false, data = "no data available" };

                if(!update.UToken.Equals(userInfo.PasswordResetToken) || userInfo.ResetTokenExpires < DateTime.Now)
                    return new ResponseViewModel { message = "Token not valid, try again or call us", status = false, data = "no data available" };

                CreatePasswordHash(update.Password, out byte[] passwordHash, out byte[] passwordSalt);

                var hashedDetails = new HashForgottenPassword
                {
                    Id = userInfo.UserId,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };

                var updatedUser = await _userRepository.UpdateUserPassword(hashedDetails, SqlConn);
                if(updatedUser.status is false)
                    return new ResponseViewModel { message = "Could not update password, try again", status = false, data = "no data available" };

                return new ResponseViewModel { message = "Password changed successfully", status = true, data = "no data available" };


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " + ex.StackTrace);
                return new ResponseViewModel();
            }
        }
    }
}
