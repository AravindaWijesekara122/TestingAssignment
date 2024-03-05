using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class GuestListAttendee
    {
        public int GuestListID { get; set; }
        public GuestList GuestList { get; set; }
        public int AttendeeID { get; set; }
        public Attendee Attendee { get; set; }
    }
}
