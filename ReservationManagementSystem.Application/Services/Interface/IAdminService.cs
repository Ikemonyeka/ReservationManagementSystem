using ReservationManagementSystem.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Application.Services.Interface
{
    public interface IAdminService
    {
        Task<ResponseViewModel> CreateAdmin(AdminViewModel admin);
        Task<ResponseViewModel> LoginAdmin(AdminLoginViewModel adminLogin);
        Task<ResponseViewModel> VerifyAdmin(string email, string token);
    }
}
