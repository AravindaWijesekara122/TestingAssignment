using BusinessLogicLayer.DTOs;
using BusinessLogicLayer.Services;
using DataAccessLayer;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagerAPITests.BusinessLogicLayerTest
{
    public class OrganizerServicesTest
    {
        private readonly Mock<ApplicationDbContext> _mockDbContext;
        private readonly OrganizerService _organizerService;
        public OrganizerServicesTest()
        {
            _mockDbContext = new Mock<ApplicationDbContext>();
            _organizerService = new OrganizerService(_mockDbContext.Object);
        }


        [Fact]
        public void CreateEvent_ValidEventDTO_SavesEvent()
        {
            // Arrange
            var newEventDTO = new EventDTO()
            {
                EventName = "Test Event",
                Date = DateTime.Now.AddDays(7),
                //Time = "10:00 AM",
                Location = "Meeting Room 1",
                Description = "A test event description",
                OrganizerID = 1
            };

            _mockDbContext.Setup(m => m.SaveChanges()).Verifiable();

            // Act
            _organizerService.CreateEvent(newEventDTO);

            // Assert
            _mockDbContext.Verify(m => m.Events.Add(It.IsAny<Event>()), Times.Once);
            _mockDbContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void CreateEvent_WithInvalidData_ThrowsException()
        {
            // Arrange
            var newEventDTO = new EventDTO { /* Initialize with invalid or null data */ };

            // Act & Assert
            Assert.Throws<Exception>(() => _organizerService.CreateEvent(newEventDTO));
        }

        // Get all events

        [Fact]
        public void GetEvents_ReturnsAllEvents()
        {
            // Arrange
            var events = new List<Event>()
            {
                new Event { EventID = 1 },
                new Event { EventID = 2 }
            };

            _mockDbContext.Setup(m => m.Events).Returns((DbSet<Event>)events.AsQueryable());

            // Act
            var returnedEvents = _organizerService.GetEvents();

            // Assert
            Assert.NotNull(returnedEvents);
            Assert.Equal(2, returnedEvents.Count());
        }

        [Fact]
        public void GetEvents_ThrowsDbContextException_RethrowsException()
        {
            // Arrange
            var expectedException = new Exception("Test Exception");
            _mockDbContext.Setup(m => m.Events).Throws(expectedException);

            // Act & Assert
            Assert.Throws<Exception>(() => _organizerService.GetEvents());
        }


        // Cansel event

        [Fact]
        public void CancelEvent_ExistingEvent_RemovesEvent()
        {
            // Arrange
            var eventId = 1;
            var eventToCancel = new Event { EventID = eventId };

            _mockDbContext.Setup(m => m.Events.Find(eventId)).Returns(eventToCancel);
            _mockDbContext.Setup(m => m.SaveChanges()).Verifiable();

            // Act
            _organizerService.CancelEvent(eventId);

            // Assert
            _mockDbContext.Verify(m => m.Events.Remove(eventToCancel), Times.Once);
            _mockDbContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void CancelEvent_ThrowsDbContextException_RethrowsException()
        {
            // Arrange
            var eventId = 1;
            var expectedException = new Exception("Test Exception");
            _mockDbContext.Setup(m => m.Events.Find(eventId)).Returns((Event)null); // Trigger the exception
            _mockDbContext.Setup(m => m.SaveChanges()).Throws(expectedException);

            // Act & Assert
            Assert.Throws<Exception>(() => _organizerService.CancelEvent(eventId));
            _mockDbContext.Verify(m => m.Events.Remove(It.IsAny<Event>()), Times.Never);
            _mockDbContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}
