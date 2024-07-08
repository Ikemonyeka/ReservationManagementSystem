using ReservationManagementSystem.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Application.Services.Interface
{
    public interface IUserService
    {
        Task<ResponseViewModel> SignUp(SignUpViewModel signUp);
        Task<ResponseViewModel> VerifyUser(string email, string token);
        Task<ResponseViewModel> SignIn(SignInViewModel signIn);
    }
}
