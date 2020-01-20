using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirwaysWithEF
{
    public class PassengersInFlightModel
    {
        public int Id { get; set; }
        public string PassengerName { get; set; }
        public string Date { get; set; }
        public string SeatNumber { get; set; }
    }
}
