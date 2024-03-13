using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IServices;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PresentationLayer.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace EventManagerAPITests.PresentationLayerTest
{
    public class OrganizerControllerTest
    {
        private Mock<IOrganizerService> _mockOrganizerService;
        private OrganizerController _controller;

        public OrganizerControllerTest()
        {
            _mockOrganizerService = new Mock<IOrganizerService>();
            _controller = new OrganizerController(_mockOrganizerService.Object);
        }

        // Create event contoller Test

        [Fact]
        public void CreateEvent_ValidEvent_ReturnsOk()
        {
            // Arrange
            var newEventDTO = new EventDTO { EventName = "Test Event", Description = "This is a test event." };

            _mockOrganizerService.Setup(m => m.CreateEvent(newEventDTO)).Verifiable();

            // Act
            var result = _controller.CreateEvent(newEventDTO);

            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            _mockOrganizerService.Verify(m => m.CreateEvent(newEventDTO), Times.Once);
        }

        [Fact]
        public void CreateEvent_InvalidModelState_ReturnsBadRequest()
        {
            // Arrange
            var controller = new OrganizerController(null); // Simulate invalid model state by not injecting service
            controller.ModelState.AddModelError("Name", "Name is required");
            var newEventDTO = new EventDTO { Description = "This is a test event." };

            // Act
            var result = controller.CreateEvent(newEventDTO);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);

            // No need to verify mock service call as one wasn't injected
        }

        [Fact]
        public void CreateEvent_ThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var newEventDTO = new EventDTO { EventName = "Test Event", Description = "This is a test event." };
            var expectedException = new Exception("Test Exception");

            _mockOrganizerService.Setup(m => m.CreateEvent(newEventDTO)).Throws(expectedException);

            // Act
            var result = _controller.CreateEvent(newEventDTO);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.StartsWith("Error creating event:", badRequestResult.Value.ToString());
            _mockOrganizerService.Verify(m => m.CreateEvent(newEventDTO), Times.Once);
        }

        // ====================================================================================

        // Get all events controller test

        [Fact]
        public void GetEvents_ReturnsEvents_ReturnsOk()
        {
            // Arrange
            var expectedEvents = new List<Event>()
            {
                new Event { EventID = 1, EventName = "Event 1" },
                new Event { EventID = 2, EventName = "Event 2" }
            };

            _mockOrganizerService.Setup(m => m.GetEvents()).Returns(expectedEvents);

            // Act
            var result = _controller.GetEvents();

            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.IsAssignableFrom<IEnumerable<Event>>(okObjectResult.Value); // Ensure returned value is IEnumerable<Event>
            Assert.Equal(expectedEvents, okObjectResult.Value as IEnumerable<Event>); // Assert on specific events returned
        }

        [Fact]
        public void GetEvents_ThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var expectedException = new Exception("Test Exception");

            _mockOrganizerService.Setup(m => m.GetEvents()).Throws(expectedException);

            // Act
            var result = _controller.GetEvents();

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.StartsWith("Error fetching event:", badRequestResult.Value.ToString());
            _mockOrganizerService.Verify(m => m.GetEvents(), Times.Once);
        }

        // ===================================================================================

        // Get event by id controller test

        [Fact]
        public void GetEventById_ExistingEvent_ReturnsOk()
        {
            // Arrange
            var eventId = 1;
            var expectedEvent = new Event { EventID = eventId, EventName = "Test Event" };
            var expectedEventDetailsDTO = new EventDetailsDTO { EventName = expectedEvent.EventName };

            _mockOrganizerService.Setup(m => m.GetEventById(eventId)).Returns(expectedEventDetailsDTO);

            // Act
            var result = _controller.GetEventById(eventId);

            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            _mockOrganizerService.Verify(m => m.GetEventById(eventId), Times.Once);
        }

        [Fact]
        public void GetEventById_ThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var eventId = 1;
            var expectedException = new Exception("Test Exception");

            _mockOrganizerService.Setup(m => m.GetEventById(eventId)).Throws(expectedException);

            // Act
            var result = _controller.GetEventById(eventId);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.StartsWith("Error fetching event:", badRequestResult.Value.ToString());
            _mockOrganizerService.Verify(m => m.GetEventById(eventId), Times.Once);
        }

        // ==============================================================================

        // Update Evcent Controller Test

        [Fact]
        public void UpdateEvent_ValidEvent_ReturnsOk()
        {
            // Arrange
            var eventId = 1;
            var updatedEventDTO = new EventDTO { EventName = "Updated Event Name" };

            _mockOrganizerService.Setup(m => m.UpdateEvent(eventId, updatedEventDTO)).Verifiable();

            // Act
            var result = _controller.UpdateEvent(eventId, updatedEventDTO);

            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            _mockOrganizerService.Verify(m => m.UpdateEvent(eventId, updatedEventDTO), Times.Once);
        }

        [Fact]
        public void UpdateEvent_ThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var eventId = 1;
            var updatedEventDTO = new EventDTO { EventName = "Updated Event Name" };
            var expectedException = new Exception("Test Exception");

            _mockOrganizerService.Setup(m => m.UpdateEvent(eventId, updatedEventDTO)).Throws(expectedException);

            // Act
            var result = _controller.UpdateEvent(eventId, updatedEventDTO);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.StartsWith("Error updating event:", badRequestResult.Value.ToString());
            _mockOrganizerService.Verify(m => m.UpdateEvent(eventId, updatedEventDTO), Times.Once);
        }

        //======================================================================================

        // Delete Event Controller Testing

        [Fact]
        public void CancelEvent_SuccessfulCancellation_ReturnsOk()
        {
            // Arrange
            var eventId = 1;

            _mockOrganizerService.Setup(m => m.CancelEvent(eventId)).Verifiable();

            // Act
            var result = _controller.CancelEvent(eventId);

            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            _mockOrganizerService.Verify(m => m.CancelEvent(eventId), Times.Once);
        }

        [Fact]
        public void CancelEvent_ThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var eventId = 1;
            var expectedException = new Exception("Test Exception");

            _mockOrganizerService.Setup(m => m.CancelEvent(eventId)).Throws(expectedException);

            // Act
            var result = _controller.CancelEvent(eventId);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.StartsWith("Error canceling event:", badRequestResult.Value.ToString());
            _mockOrganizerService.Verify(m => m.CancelEvent(eventId), Times.Once);
        }

        // ================================================================================

        // Get GuestList Controller Test

        [Fact]
        public void GetGuestList_ReturnsGuestList_ReturnsOk()
        {
            // Arrange
            var eventId = 1;
            var attendees = new GuestListAttendee
            {
                GuestListID = 1,
                GuestList = new GuestList { GuestListID = 1, EventID = eventId },
                AttendeeID = 1,
                Attendee = new Attendee { AttendeeID = 1, Name = "Guest 1"}
            };
            var expectedGuestList = new List<AttendeeDTO>()
            {
                new AttendeeDTO { Name = attendees.Attendee.Name },

            };

            _mockOrganizerService.Setup(m => m.GetGuestList(eventId)).Returns(expectedGuestList);

            // Act
            var result = _controller.GetGuestList(eventId);

            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.IsAssignableFrom<IEnumerable<AttendeeDTO>>(okObjectResult.Value); // Ensure returned value is IEnumerable<AttendeeDTO>

        }

        
        [Fact]
        public void GetGuestList_ThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var eventId = 1;
            var expectedException = new Exception("Test Exception");

            _mockOrganizerService.Setup(m => m.GetGuestList(eventId)).Throws(expectedException);

            // Act
            var result = _controller.GetGuestList(eventId);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.StartsWith("Error retrieving guest list:", badRequestResult.Value.ToString());
            _mockOrganizerService.Verify(m => m.GetGuestList(eventId), Times.Once);
        }

    }
}
