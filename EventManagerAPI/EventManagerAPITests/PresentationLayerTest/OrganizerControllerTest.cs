using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.IServices;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PresentationLayer.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
