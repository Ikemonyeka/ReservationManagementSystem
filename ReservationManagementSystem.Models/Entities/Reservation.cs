using ReservationManagementSystem.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Core.Entities
{
    public class Reservation
    {
        public int ReservationId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int TableId { get; set; }
        public virtual Table Table { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? SpecialRequest { get; set; }
    }
}
