using BusinessLogicLayer.DTOs;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.IServices
{
    public interface IOrganizerService
    {
        void CreateEvent(EventDTO newEventDTO);
        IEnumerable<Event> GetEvents();
        EventDetailsDTO GetEventById(int eventId);
        void UpdateEvent(int eventId, EventDTO updatedEventDTO);
        void CancelEvent(int eventId);
        IEnumerable<AttendeeDTO> GetGuestList(int eventId);
    }
}
