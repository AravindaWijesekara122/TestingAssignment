using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Event
    {
        public int EventID { get; set; }
        public string EventName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public GuestList GuestList { get; set; } // One-to-One with GuestList
        public BudgetTracking BudgetTracking { get; set; } // One-to-One with BudgetTracking
        public int OrganizerID { get; set; }
        public Organizer Organizer { get; set; } // One-to-One with Organizer
        
    }
}
