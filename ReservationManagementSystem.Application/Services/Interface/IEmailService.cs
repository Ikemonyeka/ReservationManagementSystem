using ReservationManagementSystem.Core.Objects;
using ReservationManagementSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Application.Services.Interface
{
    public interface IEmailService
    {
        Task UserVerifcationEmail(string email, string token);
        Task AdminVerifcationEmail(string email, string token);
        Task<ResponseViewModel> ReservationComplete(ReservationCompleteViewModel model);
        Task<ResponseViewModel> ForgotPasswordEmail(string email, string token);
    }
}
