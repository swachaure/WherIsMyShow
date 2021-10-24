using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhereIsMyShow.Models
{
    public class BulkBookingUpdateViewModel : Booking
    {

        public bool isSelected { get; set; } = false;
    }
}
