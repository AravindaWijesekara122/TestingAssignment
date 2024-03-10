using DataAccessLayer.Models;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLayer.Services;
using Moq;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace EventManagerAPITests.BusinessLogicLayerTest
{
    public class CustomerServicesTest
    {
        private readonly Mock<ApplicationDbContext> _mockDbContext;
        private readonly CustomerService _eventService;
        public CustomerServicesTest()
        {
            _mockDbContext = new Mock<ApplicationDbContext>();
            _eventService = new CustomerService(_mockDbContext.Object);
        }
        [Fact]
        public void GetUpcomingEvents_ReturnsUpcomingEvents()
        {
            // Arrange            

            var currentDate = DateTime.Now;
            var expectedUpcomingEvents = new List<Event>
            {
                new Event { EventName = "Event 1", Date = currentDate.AddDays(1) },
                new Event { EventName = "Event 2", Date = currentDate.AddDays(2) }
            }.AsQueryable();

            _mockDbContext.Setup(db => db.Events)
                         .Returns((DbSet<Event>)expectedUpcomingEvents);

            // Act
            var result = _eventService.GetUpcomingEvents();

            // Assert
            //Assert.Equal(expectedUpcomingEvents, result);
            Assert.NotNull(result);
            //Assert.Equal(2, 2);
        }

        [Fact]
        public void GetUpcomingEvents_ThrowsException_WhenDatabaseFails()
        {
            // Arrange

            _mockDbContext.Setup(db => db.Events)
                         .Throws(new Exception("Mocked exception"));

            // Act & Assert
            Assert.Throws<Exception>(() => _eventService.GetUpcomingEvents());
        }

        // Cansel Registration

        [Fact]
        public void CancelRegistration_WhenRegistrationExists_ShouldRemoveRegistration()
        {
            // Arrange
            var eventId = 1;
            var attendeeId = 1;

            var mockDbSet = new Mock<DbSet<GuestListAttendee>>();
            var guestListAttendees = new List<GuestListAttendee>()
            {
            new GuestListAttendee { AttendeeID = attendeeId},
            new GuestListAttendee { AttendeeID = 3}
            };

            _mockDbContext.Setup(m => m.GuestListAttendees).Returns((DbSet<GuestListAttendee>)guestListAttendees.AsQueryable());

            // Act
            _eventService.CancelRegistration(eventId, attendeeId);

            // Assert
            _mockDbContext.Verify(m => m.GuestListAttendees.Remove(It.IsAny<GuestListAttendee>()), Times.Once);
            _mockDbContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void CancelRegistration_WhenRegistrationDoesNotExist_ShouldThrowException()
        {
            // Arrange

            var mockDbSet = new Mock<DbSet<GuestListAttendee>>();
            var registrations = new List<GuestListAttendee>().AsQueryable();

            var attendeeId = 5;
            var eventId = 2;
            var guestListAttendees = new List<GuestListAttendee>()
            {
                new GuestListAttendee { AttendeeID = 1 },
                new GuestListAttendee { AttendeeID = 3 }
            };

            _mockDbContext.Setup(m => m.GuestListAttendees).Returns((DbSet<GuestListAttendee>)guestListAttendees.AsQueryable());

            // Act & Assert
            Assert.Throws<Exception>(() => _eventService.CancelRegistration(eventId, attendeeId));
            _mockDbContext.Verify(m => m.GuestListAttendees.Remove(It.IsAny<GuestListAttendee>()), Times.Never);
            _mockDbContext.Verify(m => m.SaveChanges(), Times.Never);
        }


        // Get Registered Events

        [Fact]
        public void GetRegisteredEvents_ExistingAttendee_ReturnsEvents()
        {
            // Arrange
            var attendeeId = 1;
            var attendee = new Attendee { AttendeeID = attendeeId };
            var events = new List<Event>()
            {
                new Event { EventID = 1 },
                new Event { EventID = 2 }
            };
            var guestListAttendees = new List<GuestListAttendee>()
            {
                new GuestListAttendee { AttendeeID = attendeeId, GuestList = new GuestList { Event = events[0] } },
                new GuestListAttendee { AttendeeID = 2, GuestList = new GuestList { Event = events[1] } }
            };

            _mockDbContext.Setup(m => m.Attendees.Find(attendeeId)).Returns(attendee);
            _mockDbContext.Setup(m => m.GuestListAttendees).Returns((DbSet<GuestListAttendee>)guestListAttendees.AsQueryable());

            // Act
            var registeredEvents = _eventService.GetRegisteredEvents(attendeeId);

            // Assert
            Assert.NotNull(registeredEvents);
            //Assert.Equal(1, registeredEvents.Count());
            //Assert.Contains(registeredEvents, e => e.EventID == events[0].EventID);
        }

        [Fact]
        public void GetRegisteredEvents_ThrowsDbContextException_RethrowsException()
        {
            // Arrange
            var attendeeId = 1;
            var expectedException = new Exception("Test Exception");
            _mockDbContext.Setup(m => m.Attendees.Find(attendeeId)).Throws(expectedException);

            // Act & Assert
            Assert.Throws<Exception>(() => _eventService.GetRegisteredEvents(attendeeId));
        }
    }
}
