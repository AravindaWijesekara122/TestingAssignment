using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IServices;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Route("api/organizer")]
    [ApiController]
    public class OrganizerController : Controller
    {
        private readonly IOrganizerService _organizerService;

        public OrganizerController(IOrganizerService organizerService)
        {
            _organizerService = organizerService;
        }

        [HttpPost("create-event")]
        public IActionResult CreateEvent([FromBody] EventDTO newEventDTO)
        {
            try
            {
                _organizerService.CreateEvent(newEventDTO);
                return Ok(new { Message = "Event created successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating event: {ex.Message}");
            }
        }

        [HttpGet("all-events")]
        public IActionResult GetEvents()
        {
            try
            {
                var events = _organizerService.GetEvents();
                return Ok(events);
            }
            catch (Exception ex) 
            {
                return BadRequest($"Error fetching event: {ex.Message}");
            }
        }

        [HttpGet("event/{eventId}")]
        public IActionResult GetEventById(int eventId)
        {
            try
            {
                var ev = _organizerService.GetEventById(eventId);
                return Ok(ev);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching event: {ex.Message}");
            }
        }

        [HttpPut("update-event/{eventId}")]
        public IActionResult UpdateEvent(int eventId, [FromBody] EventDTO updatedEventDTO)
        {
            try
            {
                _organizerService.UpdateEvent(eventId, updatedEventDTO);
                return Ok(new { Message = $"Event with ID {eventId} updated successfully."});   
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating event: {ex.Message}");
            }
        }

        [HttpDelete("delete-event/{eventId}")]
        public IActionResult CancelEvent(int eventId)
        {
            try
            {
                _organizerService.CancelEvent(eventId);
                return Ok(new {Message = $"Event with ID {eventId} canceled successfully."});
            }
            catch (Exception ex)
            {
                return BadRequest($"Error canceling event: {ex.Message}");
            }
        }

        [HttpGet("guestlist/{eventId}")]
        public IActionResult GetGuestList(int eventId)
        {
            try
            {
                var guestList = _organizerService.GetGuestList(eventId);
                return Ok(guestList);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving guest list: {ex.Message}");
            }
        }

    }
}
