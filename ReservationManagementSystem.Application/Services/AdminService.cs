using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ReservationManagementSystem.Application.Repositories;
using ReservationManagementSystem.Application.Repositories.Interface;
using ReservationManagementSystem.Application.Services.Interface;
using ReservationManagementSystem.Core.Entities;
using ReservationManagementSystem.Core.Enums;
using ReservationManagementSystem.Core.Objects;
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
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AdminService> _logger;
        private readonly string SqlConn;


        public AdminService(IAdminRepository adminRepository, IConfiguration configuration, IEmailService emailService, IHttpContextAccessor httpContextAccessor, ILogger<AdminService> logger)
        {
            _adminRepository = adminRepository;
            _configuration = configuration;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            SqlConn = _configuration.GetConnectionString("DefaultConnection");
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
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

        private string CreateToken(AdminClaims adminClaims)
        {
            List<Claim> claims = new List<Claim>
            {
                 new Claim(ClaimTypes.SerialNumber, adminClaims.userId.ToString("")),
                new Claim(ClaimTypes.Name, adminClaims.Username),
                new Claim(ClaimTypes.Role, adminClaims.Role)

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

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        public async Task<ResponseViewModel> CreateAdmin(AdminViewModel admin)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                if (string.IsNullOrWhiteSpace(admin.FirstName) || string.IsNullOrWhiteSpace(admin.LastName) || string.IsNullOrWhiteSpace(admin.Username) || string.IsNullOrWhiteSpace(admin.Email) || string.IsNullOrWhiteSpace(admin.PhoneNumber))
                {
                    response.message = "Some fields are still empty";
                    response.status = false;
                    response.data = "No data available";
                    return response;
                }

                if (!admin.Password.Equals(admin.ConfirmPassword))
                {
                    response.message = "Passwords do not match";
                    response.status = false;
                    response.data = "No data available";
                    return response;
                }

                if (_httpContextAccessor.HttpContext is null)
                {
                    response.message = "User can't be identified";
                    response.status = false;
                    response.data = "no data available";

                    return response;
                }

                int adminClaim = Convert.ToInt32(_httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.Role)?.Value);

                var adminEmail = await _adminRepository.GetAdminByEmail(admin.Email, SqlConn);
                var adminUsername=await _adminRepository.GetAdminByUsername(admin.Username, SqlConn);
                if(adminEmail is not null || adminEmail is not null)
                {
                    response.message = "Email or Username already exists";
                    response.status = false;
                    response.data = "No data available";
                    return response;
                }

                CreatePasswordHash(admin.Password, out byte[] passwordHash, out byte[] passwordSalt);

                var createAdmin = new Admin();

                if (adminClaim is (int)Roles.SuperAdmin)
                {
                    createAdmin = new Admin
                    {
                        Firstname = admin.FirstName,
                        Lastname = admin.LastName,
                        Username = admin.Username,
                        Email = admin.Email,
                        PhoneNumber = admin.PhoneNumber,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        DateCreadted = DateTime.Now,
                        IsVerified = false,
                        VerificationToken = VerficationGen(),
                        Role = (int)Roles.PrimaryAdmin,
                        RestuarantId = admin.RestuarantId
                    };
                }
                else if (adminClaim is (int)Roles.PrimaryAdmin)
                {
                    string primaryAdmin = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.Name)?.Value;
                    var primaryAdminInfo = await _adminRepository.GetAdminByUsername(primaryAdmin, SqlConn);

                    createAdmin = new Admin
                    {
                        Firstname = admin.FirstName,
                        Lastname = admin.LastName,
                        Username = admin.Username,
                        Email = admin.Email,
                        PhoneNumber = admin.PhoneNumber,
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        DateCreadted = DateTime.Now,
                        IsVerified = false,
                        VerificationToken = VerficationGen(),
                        Role = (int)Roles.SecoondaryAdmin,
                        RestuarantId = primaryAdminInfo.RestuarantId
                    };
                }
                else if (adminClaim is (int)Roles.SecoondaryAdmin)
                {
                    response.message = "Admin was not created, access denied";
                    response.status = false;
                    response.data = "no data available";

                    return response;
                   
                }
                var data = await _adminRepository.CreateAdmin(createAdmin);

                if (data is null)
                {
                    response.message = "Admin was not created, please try again later";
                    response.status = false;
                    response.data = "no data available";

                    return response;
                }

                await _emailService.AdminVerifcationEmail(createAdmin.Email, createAdmin.VerificationToken);

                response.message = "Admin Created Successfully";
                response.status = true;
                response.data = "No data available";
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

        public async Task<ResponseViewModel> LoginAdmin(AdminLoginViewModel adminLogin)
        {
            AdminClaims claims = new AdminClaims();
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                if (string.IsNullOrWhiteSpace(adminLogin.Username))
                {
                    response.message = "All fields are required";
                    response.status = false;
                    response.data = "no data available";
                    return response;
                }

                var adminUser = await _adminRepository.GetAdminByUsername(adminLogin.Username, SqlConn);

                if (adminUser is null)
                {
                    response.message = "Have you verified your account? Username or Password is wrong";
                    response.status = false;
                    response.data = "No data available";
                    return response;
                }

                if(!VerifyPasswordHash(adminLogin.Password, adminUser.PasswordHash, adminUser.PasswordSalt))
                {
                    response.message = "Incorrect Username or Password";
                    response.status = false;
                    response.data = "no data available";
                    return response;
                }

                claims.userId = adminUser.AdminId;
                claims.Username = adminUser.Username;
                claims.Role = adminUser.Role.ToString();
                
                var token = CreateToken(claims);

                response.message = "Login Successful";
                response.status = true;
                response.data = token;

                return response;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message + " " +  ex.StackTrace);

                response.message = ex.Message;
                response.status = false;
                response.data = "No data available";

                return response;
            }
        }

        public async Task<ResponseViewModel> VerifyAdmin(string email, string token)
        {
            ResponseViewModel response = new ResponseViewModel();
            try
            {
                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(token))
                {
                    response.message = "All fields are required";
                    response.status = false;
                    response.data = "no data available";
                    return response;
                }

                var adminUser = await _adminRepository.GetAdminByEmail(email, SqlConn);

                if (adminUser is null)
                {
                    response.message = "Username does not exist";
                    response.status = false;
                    response.data = "No data available";
                    return response;
                }

                if (adminUser.VerificationToken is null)
                {
                    response.message = "Admin has not received token to verify";
                    response.status = false;
                    response.data = "No data available";
                    return response;
                }

                if (!adminUser.VerificationToken.Equals(token))
                {
                    response.message = "Token not found, please contact your admin";
                    response.status = false;
                    response.data = "No data available";
                    return response;
                }
                var verify = await _adminRepository.VerifyAdminEmail(email, SqlConn);

                if (verify is null)
                {
                    response.message = "Verification unsuccessful, try again later";
                    response.status = true;
                    response.data = "no data available";
                    return response;
                }

                response.message = "Verification Successful";
                response.status = true;
                response.data = "no data available";
                return response;
            }
            catch (Exception ex)
            {
                return response;
            }
        }
    }
}