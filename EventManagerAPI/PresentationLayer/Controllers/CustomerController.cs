using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IServices;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("upcoming")]
        public IActionResult GetUpcomingEvents()
        {
            try
            {
                var upcomingEvents = _customerService.GetUpcomingEvents();
                return Ok(upcomingEvents);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving upcoming events: {ex.Message}");
            }
        }

        [HttpPost("register/{eventId}/{attendeeId}")]
        public IActionResult RegisterForEvent(int eventId, int attendeeId)
        {
            try
            {
                SuccessMessageDTO successMessage = _customerService.RegisterForEvent(eventId, attendeeId);
                return Ok(successMessage);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error registering for the event: {ex.Message}");
            }
        }

        [HttpGet("registered-events/{attendeeId}")]
        public IActionResult GetRegisteredEvents(int attendeeId)
        {
            try
            {
                var registeredEvents = _customerService.GetRegisteredEvents(attendeeId);
                return Ok(registeredEvents);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving registered events: {ex.Message}");
            }
        }

        [HttpDelete("unregister/{eventId}/{attendeeId}")]
        public IActionResult CancelRegistration(int attendeeId, int eventId)
        {
            try
            {
                _customerService.CancelRegistration(eventId, attendeeId);
                return Ok(new { Message = "Registration canceled successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error canceling registration: {ex.Message}");
            }
        }
    }
}
