using ReservationManagementSystem.Core.Objects;
using ReservationManagementSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Application.Repositories.Interface
{
    public interface IUserRepository
    {
        Task CreateUser(SignUpViewModel signUp);
        Task<object> VerifyEmail(string email, string token);
        Task<User> GetUserByEmail(string email, string SqlConn);
        Task<User> GetUserById(int Id, string SqlConn);
        Task<ResponseViewModel> ForgotPasswordToken(ForgotPasswordEmailViewModel forgotPassword, string SqlConn);
        Task<ResponseViewModel> UpdateUserPassword(HashForgottenPassword hash, string SqlConn);
    }
}
