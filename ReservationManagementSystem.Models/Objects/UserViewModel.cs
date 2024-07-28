using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Core.Objects
{
    public class ForgotPasswordEmailViewModel
    {
        public int Id { get; set; }
        public string PasswordResetToken { get; set; }
        public DateTime ResetTokenExpires { get; set; }
    }

    public class UpdateForgottenPassword
    {
        public string Email { get; set; }
        public string UToken { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class HashForgottenPassword
    {
        public int Id { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}
