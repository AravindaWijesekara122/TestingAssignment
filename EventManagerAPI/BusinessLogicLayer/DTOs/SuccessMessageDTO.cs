using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.DTOs
{
    public class SuccessMessageDTO
    {
        public string Message { get; set; }
        public string AttendeeName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string EventName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
    }
}
