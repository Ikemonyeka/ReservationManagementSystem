using ReservationManagementSystem.Application.Repositories.Interface;
using ReservationManagementSystem.Application.Services.Interface;
using ReservationManagementSystem.Core.Objects;
using ReservationManagementSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public UserService(IEmailService emailService, IUserRepository userRepository)
        {
            _emailService = emailService;
            _userRepository = userRepository;
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

                await _userRepository.CreateUser(signUp);

                await _emailService.VerifcationEmail(signUp.Email);

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
    }
}
