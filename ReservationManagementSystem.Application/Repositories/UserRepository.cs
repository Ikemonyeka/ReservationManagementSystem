using ReservationManagementSystem.Application.Repositories.Interface;
using ReservationManagementSystem.Core.Objects;
using ReservationManagementSystem.Models.Entities;
using ReservationManagementSystem.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Application.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
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
            };

            await _context.Users.AddAsync(userinfo);
            _context.SaveChanges();

        }
    }
}
