using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Core.Objects
{
    public class SignUpViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}
