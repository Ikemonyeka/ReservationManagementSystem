using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Core.Objects
{
    public class ResponseViewModel
    {
        public string message { get; set; } = string.Empty;
        public string data { get; set; } = string.Empty;
        public bool status { get; set; }
    }
}
