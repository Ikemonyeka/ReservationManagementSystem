using ReservationManagementSystem.Core.Entities;
using ReservationManagementSystem.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Application.Repositories.Interface
{
    public interface IAdminRepository
    {
        Task<Admin> GetAdminByEmail(string email, string SqlConn);
        Task<ResponseViewModel> CreateAdmin(Admin admin);
        Task<Admin> GetAdminByUsername(string username, string SqlConn);
        Task<ResponseViewModel> VerifyAdminEmail(string email, string SqlConn);
        Task<ResponseViewModel> DeleteAdmin(int adminId, string SqlConn);
    }
}
