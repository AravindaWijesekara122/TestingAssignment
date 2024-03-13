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
    public class CustomerControllerTest
    {
        private Mock<ICustomerService> _mockCustomerService;
        private CustomerController _controller;

        public CustomerControllerTest()
        {
            _mockCustomerService = new Mock<ICustomerService>();
            _controller = new CustomerController(_mockCustomerService.Object);
        }

        // Get Upcoming events controller test

        [Fact]
        public void GetUpcomingEvents_ReturnsUpcomingEvents_ReturnsOk()
        {
            // Arrange
            var upcomingEvents = new List<Event>()
        {
            new Event { Date = DateTime.Now.AddDays(7) },
            new Event { Date = DateTime.Now.AddDays(14) },
        };

            _mockCustomerService.Setup(m => m.GetUpcomingEvents()).Returns(upcomingEvents);

            // Act
            var result = _controller.GetUpcomingEvents();

            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.IsAssignableFrom<IEnumerable<Event>>(okObjectResult.Value); // Ensure returned value is IEnumerable<Event>
                                                                 
        }

        [Fact]
        public void GetUpcomingEvents_ThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var expectedException = new Exception("Test Exception");
            _mockCustomerService.Setup(m => m.GetUpcomingEvents()).Throws(expectedException);

            // Act
            var result = _controller.GetUpcomingEvents();

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.StartsWith("Error retrieving upcoming events:", badRequestResult.Value.ToString());
        }

        // ===========================================================================================
        // Register for an event Test

        [Fact]
        public void RegisterForEvent_SuccessfulRegistration_ReturnsOk()
        {
            // Arrange
            var attendeeId = 1;
            var eventId = 1;
            var expectedSuccessMessage = new SuccessMessageDTO { Message = "Registration successful!" };

            _mockCustomerService.Setup(m => m.RegisterForEvent(eventId, attendeeId)).Returns(expectedSuccessMessage);

            // Act
            var result = _controller.RegisterForEvent(attendeeId, eventId);

            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Equal(expectedSuccessMessage, okObjectResult.Value);
            _mockCustomerService.Verify(m => m.RegisterForEvent(eventId, attendeeId), Times.Once);
        }

        [Fact]
        public void RegisterForEvent_ThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var attendeeId = 1;
            var eventId = 1;
            var expectedException = new Exception("Test Exception");

            _mockCustomerService.Setup(m => m.RegisterForEvent(eventId, attendeeId)).Throws(expectedException);

            // Act
            var result = _controller.RegisterForEvent(attendeeId, eventId);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.StartsWith("Error registering for the event:", badRequestResult.Value.ToString());
            _mockCustomerService.Verify(m => m.RegisterForEvent(eventId, attendeeId), Times.Once);
        }

        //============================================================================================
        // Get Registered Events Test

        [Fact]
        public void GetRegisteredEvents_ReturnsEvents_ReturnsOk()
        {
            // Arrange
            var attendeeId = 1;
            var expectedEvents = new List<Event>()
            {
                new Event { EventID = 1 },
                new Event { EventID = 2 }
            };

            _mockCustomerService.Setup(m => m.GetRegisteredEvents(attendeeId)).Returns(expectedEvents);

            // Act
            var result = _controller.GetRegisteredEvents(attendeeId);

            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.IsAssignableFrom<IEnumerable<Event>>(okObjectResult.Value); // Ensure returned value is IEnumerable<Event>
        }

        [Fact]
        public void GetRegisteredEvents_NoEvents_ReturnsEmptyList()
        {
            // Arrange
            var attendeeId = 1;
            _mockCustomerService.Setup(m => m.GetRegisteredEvents(attendeeId)).Returns(new List<Event>());

            // Act
            var result = _controller.GetRegisteredEvents(attendeeId);

            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            Assert.Empty(okObjectResult.Value as IEnumerable<Event>);
            Assert.IsAssignableFrom<IEnumerable<Event>>(okObjectResult.Value); // Ensure returned value is IEnumerable<Event>
        }

        [Fact]
        public void GetRegisteredEvents_ThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var attendeeId = 1;
            var expectedException = new Exception("Test Exception");

            _mockCustomerService.Setup(m => m.GetRegisteredEvents(attendeeId)).Throws(expectedException);

            // Act
            var result = _controller.GetRegisteredEvents(attendeeId);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
            Assert.StartsWith("Error retrieving registered events:", badRequestResult.Value.ToString());
            _mockCustomerService.Verify(m => m.GetRegisteredEvents(attendeeId), Times.Once);
        }

        // ===========================================================================================
        // Cansel Registraion Controller Testing

        [Fact]
        public void CancelRegistration_SuccessfulCancellation_ReturnsOk()
        {
            // Arrange
            var attendeeId = 1;
            var eventId = 1;

            _mockCustomerService.Setup(m => m.CancelRegistration(attendeeId, eventId)).Verifiable();

            // Act
            var result = _controller.CancelRegistration(attendeeId, eventId);

            // Assert
            var okObjectResult = result as OkObjectResult;
            Assert.NotNull(okObjectResult);
            Assert.Equal(200, okObjectResult.StatusCode);
            _mockCustomerService.Verify(m => m.CancelRegistration(attendeeId, eventId), Times.Once);
        }

        [Fact]
        public void CancelRegistration_ThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var attendeeId = 1;
            var eventId = 2;
            var expectedException = new Exception("Test Exception");

            _mockCustomerService.Setup(m => m.CancelRegistration(eventId, attendeeId)).Throws(expectedException);

            // Act
            var result = _controller.CancelRegistration(attendeeId, eventId);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
            _mockCustomerService.Verify(m => m.CancelRegistration(eventId, attendeeId), Times.Once);
        }
    }
}
