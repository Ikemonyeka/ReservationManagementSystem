using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Core.Entities
{
    public class ReservationAvailability
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public virtual Restuarant Restuarant { get; set; }
    }
}
