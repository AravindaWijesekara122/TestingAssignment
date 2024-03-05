using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class BudgetTracking
    {
        public int BudgetID { get; set; }
        public int EventID { get; set; }
        public Event Event { get; set; } // One-to-One with Event
        public int Expenses { get; set; }
        public int Revenue { get; set; }
    }
}
