using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Core.Objects
{
    public class ReservationTimeViewModel
    {
        public TimeOnly Time {  get; set; }
        public TimeOnly Duration { get; set; }

    }

    public class TimeViewModel
    {
        public string Time { get; set; }
        public string Duration { get; set; }
    }
}
