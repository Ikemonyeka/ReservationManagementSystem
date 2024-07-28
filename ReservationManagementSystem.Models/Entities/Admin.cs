using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Core.Entities
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public int Role { get; set; }
        public DateTime DateCreadted { get; set; }
        public DateTime? LastLogin { get; set; }
        public bool IsVerified { get; set; }
        public string? VerificationToken { get; set; }
        public int? RestuarantId { get; set; }
        public virtual Restuarant Restuarant { get; set; }
    }
}
