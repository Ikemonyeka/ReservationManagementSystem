﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Application.Services.Interface
{
    public interface IEmailService
    {
        Task VerifcationEmail(string email, string token);
    }
}
