using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IServices;
using DataAccessLayer;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services
{
    public class OrganizerService : IOrganizerService
    {
        private readonly ApplicationDbContext _dbContext;

        public OrganizerService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreateEvent(EventDTO newEventDTO)
        {
            try
            {
                Event newEvent = new Event()
                {
                    EventName = newEventDTO.EventName,
                    Date = newEventDTO.Date,
                    Time = newEventDTO.Time,
                    Location = newEventDTO.Location,
                    Description = newEventDTO.Description,
                    OrganizerID = newEventDTO.OrganizerID,
                    GuestList = new GuestList()
                    {
                        GuestListAttendees = new List<GuestListAttendee>()
                    },
                    BudgetTracking = new BudgetTracking(),
                };        

                _dbContext.Events.Add(newEvent);

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                new Exception(ex.Message);
            }  
        }

        public IEnumerable<Event> GetEvents()
        {
            try
            {
                var events = _dbContext.Events.ToList();

                return events;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public EventDetailsDTO GetEventById(int eventId)
        {
            try
            {
                var thisEvent = _dbContext.Events.Find(eventId);

                if (thisEvent == null)
                {
                    throw new Exception("No event found");
                }

                var eventDetails = new EventDetailsDTO()
                {
                    EventName = thisEvent.EventName,
                    Date = thisEvent.Date,
                    Time = thisEvent.Time,
                    Location = thisEvent.Location,
                    Description = thisEvent.Description,
                };
                return eventDetails;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }           
        }

        public void UpdateEvent(int eventId, EventDTO updatedEventDTO)
        {
            try
            {
                Event existingEvent = _dbContext.Events.Find(eventId);

                if (existingEvent == null)
                {
                    throw new InvalidOperationException($"Event with ID {eventId} not found.");
                }

                existingEvent.EventName = updatedEventDTO.EventName;
                existingEvent.Date = updatedEventDTO.Date;
                existingEvent.Time = updatedEventDTO.Time;
                existingEvent.Location = updatedEventDTO.Location;
                existingEvent.Description = updatedEventDTO.Description;

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void CancelEvent(int eventId)
        {
            try
            {
                var eventToCancel = _dbContext.Events.Find(eventId);
                if (eventToCancel == null)
                {
                    throw new InvalidOperationException($"Event with ID {eventId} not found.");
                }

                _dbContext.Events.Remove(eventToCancel);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }          
        }

        public IEnumerable<AttendeeDTO> GetGuestList(int eventId)
        {
            try
            {
                var attendees = _dbContext.GuestListAttendees
                    .Where(gla => gla.GuestList.EventID == eventId)
                    .Select(gla => gla.Attendee)
                    .Select(a => new AttendeeDTO()
                    {
                        Name = a.Name,
                        PhoneNumber = a.PhoneNumber,
                        Email = a.Email

                    })
                    .ToList();
                if (attendees == null)
                {
                    throw new Exception("No attendees found");
                }
                return attendees;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
    }
}
