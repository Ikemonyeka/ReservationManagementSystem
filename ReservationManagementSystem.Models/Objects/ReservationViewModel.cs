using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Core.Objects
{
    public class ReservationViewModel
    {
        public DateTime ReservationDate{ get; set; }
        public string StartTime { get; set; }
        public int TimeId { get; set; }
        public string SpecialRequest {  get; set; }
        public int TableId { get; set; }
        public int PartySize { get; set; }
    }

    public class ReservationCompleteViewModel
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public DateTime ReservationDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string TableType { get; set; }
        public int PartySize { get; set; }
        public string SpecialRequests { get; set; }
        public string RestuarantName { get; set; }
    }
}
