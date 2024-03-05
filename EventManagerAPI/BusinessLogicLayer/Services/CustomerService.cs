using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IServices;
using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Services
{  
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _dbContext;
        public CustomerService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<Event> GetUpcomingEvents()
        {
            try
            {
                var upcomingEvents = _dbContext.Events
                .Where(e => e.Date >= DateTime.Now)
                //.Select(e => new EventDetailsDTO
                //{
                //    EventName = e.EventName,
                //    Date = e.Date,
                //    Time = e.Time,
                //    Location = e.Location,
                //    Description = e.Description,
                //})
                .ToList();

                return upcomingEvents;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public SuccessMessageDTO RegisterForEvent(int eventId, int attendeeId)
        {
            try
            {
                var existingGuestlist = _dbContext.GuestLists.SingleOrDefault(a => a.EventID == eventId);
                var existingEvent = _dbContext.Events.Find(eventId);
                var existingAttendee = _dbContext.Attendees.Find(attendeeId);

                existingGuestlist.GuestListAttendees.Add(new GuestListAttendee
                {
                    Attendee = existingAttendee
                });

                var isAlreadyRegistered = _dbContext.GuestListAttendees
                    .Any(gla => gla.GuestList.EventID == eventId && gla.AttendeeID == attendeeId);

                if (isAlreadyRegistered)
                {
                    throw new InvalidOperationException("Attendee is already registered for the event");
                }

                _dbContext.SaveChanges();

                return SuccessMessage(existingEvent, existingAttendee);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private SuccessMessageDTO SuccessMessage(Event excistingEvent, Attendee excistingAttendee)
        {
            var message = new SuccessMessageDTO()
            {
                Message = "Successfully Registered for event..",
                AttendeeName = excistingAttendee.Name,
                PhoneNumber = excistingAttendee.PhoneNumber,
                Email = excistingAttendee.Email,
                EventName = excistingEvent.EventName,
                Date = excistingEvent.Date,
                Time = excistingEvent.Time,
                Location = excistingEvent.Location,
                Description = excistingEvent.Description,
            };
            return message;
        }


        public IEnumerable<Event> GetRegisteredEvents(int attendeeId)
        {
            try
            {
                var attendee = _dbContext.Attendees.Find(attendeeId);
                var registeredEvents = _dbContext.GuestListAttendees
                .Where(gla => gla.AttendeeID == attendeeId)
                .Select(gla => gla.GuestList.Event)
                //.Select(a => new EventDetailsDTO()
                //{
                //    EventName = a.EventName,
                //    Date = a.Date,
                //    Time = a.Time,
                //    Location = a.Location,
                //    Description = a.Description
                //})
                .ToList();

                return registeredEvents;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void CancelRegistration(int eventId, int attendeeId)
        {
            try
            {
                var registrationToRemove = _dbContext.GuestListAttendees
                .SingleOrDefault(gla => gla.GuestList.EventID == eventId && gla.AttendeeID == attendeeId);

                if (registrationToRemove != null)
                {
                    _dbContext.GuestListAttendees.Remove(registrationToRemove);
                    _dbContext.SaveChanges();
                }
                else
                {
                    throw new Exception("Registration not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
