using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Core.Entities
{
    public class Table
    {
        public int TableId { get; set; }
        public string TableName { get; set; } = string.Empty;
        public int TableQuantity { get; set; }
        public bool IsActive { get; set; }
        public int RestuarantId { get; set; }
        public int PartySize { get; set; }
        public virtual Restuarant Restuarant { get; set; }
        public virtual List<Reservation> Reservation { get; set; }
    }
}
