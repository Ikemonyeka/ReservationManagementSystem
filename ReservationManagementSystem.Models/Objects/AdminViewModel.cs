using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Core.Objects
{
    public class AdminViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int RestuarantId { get; set; }   
    }

    public class AdminLoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class AdminClaims
    {
        public int userId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
