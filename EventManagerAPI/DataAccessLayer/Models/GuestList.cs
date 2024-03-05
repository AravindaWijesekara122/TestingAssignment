using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class GuestList
    {
        public int GuestListID { get; set; }
        public int EventID { get; set; }
        public Event Event { get; set; } // one-to-One with Event
        //public int? AttendeeID { get; set; }
        //public ICollection<Attendee>? Attendee { get; set; } // One-to-Many with Attendee
        public ICollection<GuestListAttendee> GuestListAttendees { get; set; } = new List<GuestListAttendee>();
    }
}

