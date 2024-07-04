using ReservationManagementSystem.Core.Objects;
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
    }
}
