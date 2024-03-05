using BusinessLogicLayer.DTOs;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.IServices
{
    public interface ICustomerService
    {
        IEnumerable<Event> GetUpcomingEvents();
        //void RegisterForEvent(int eventId, AttendeeDTO attendeeDTO);
        SuccessMessageDTO RegisterForEvent(int eventId, int attendeeId);
        IEnumerable<Event> GetRegisteredEvents(int attendeeId);
        void CancelRegistration(int eventId, int attendeeId);
    }
}
