using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Attendee
    {
        public int AttendeeID { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "Attendee";
        //public int? GuestListID { get; set; }
        //public GuestList? GuestList { get; set; } // one-to-many with GuestList

        public ICollection<GuestListAttendee> GuestListAttendees { get; set; }
        
    }
}
