using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Core.Entities
{
    public class ReservationTimeSlot
    {
        public int Id { get; set; }
        public TimeOnly TimeSlot { get; set; }
        public TimeOnly Duration { get; set; }
        public int RestuarantId { get; set; }
        public virtual Restuarant Restuarant { get; set; }
    }
}
