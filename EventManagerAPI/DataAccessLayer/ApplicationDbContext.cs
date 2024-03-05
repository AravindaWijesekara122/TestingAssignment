using DataAccessLayer.Entities;
using DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Organizer> Organizers { get; set; }
        public DbSet<Attendee> Attendees { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<GuestList> GuestLists { get; set; }
        public DbSet<GuestListAttendee> GuestListAttendees { get; set; }
        public DbSet<BudgetTracking> BudgetTrackings { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Organizer>()
                .HasKey(o => o.OrganizerID);

            modelBuilder.Entity<Event>()
                .HasKey(e => e.EventID);

            modelBuilder.Entity<Attendee>()
                .HasKey(e => e.AttendeeID);

            modelBuilder.Entity<GuestList>()
                .HasKey(gl => gl.GuestListID);

            modelBuilder.Entity<BudgetTracking>()
                .HasKey(bt => bt.BudgetID);

            modelBuilder.Entity<GuestListAttendee>()
                .HasKey(g => new {g.GuestListID, g.AttendeeID});

            // one-to-many relationship between organizer and event
            modelBuilder.Entity<Organizer>()
                .HasMany(o => o.Event)  // One Organizer has Many Events
                .WithOne(e => e.Organizer) // Each Event belongs to One Organizer
                .HasForeignKey(e => e.OrganizerID)
                .OnDelete(DeleteBehavior.Cascade);

            //one-to-one relationship between event and guest list
            modelBuilder.Entity<Event>()
                .HasOne(e => e.GuestList)
                .WithOne(gl => gl.Event)
                .HasForeignKey<GuestList>(gl => gl.EventID)
                .OnDelete(DeleteBehavior.Cascade);

            // one-to-one relationship between event and budget tracking
            modelBuilder.Entity<Event>()
                .HasOne(e => e.BudgetTracking)
                .WithOne(bt => bt.Event)
                .HasForeignKey<BudgetTracking>(bt => bt.EventID)
                .OnDelete(DeleteBehavior.Cascade);


            // Configure many-to-many relationship
            //modelBuilder.Entity<GuestListAttendee>()
            //    .HasOne(gla => gla.GuestList)
            //    .WithMany(gl => gl.GuestListAttendees)
            //    .HasForeignKey(gla => gla.GuestListID)
            //    .OnDelete(DeleteBehavior.SetNull);

            //modelBuilder.Entity<GuestListAttendee>()
            //    .HasOne(gla => gla.Attendee)
            //    .WithMany(a => a.GuestListAttendees)
            //    .HasForeignKey(gla => gla.AttendeeID)
            //    .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<GuestList>()
                 .HasMany(o => o.GuestListAttendees)  // One Organizer has Many Events
                 .WithOne(e => e.GuestList) // Each Event belongs to One Organizer
                 .HasForeignKey(e => e.GuestListID)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Attendee>()
                .HasMany(o => o.GuestListAttendees)  // One Organizer has Many Events
                .WithOne(e => e.Attendee) // Each Event belongs to One Organizer
                .HasForeignKey(e => e.AttendeeID)
                .OnDelete(DeleteBehavior.Cascade);


            // Seed Organizer
            modelBuilder.Entity<Organizer>().HasData(
                new Organizer { OrganizerID = 1, Name = "Amal", PhoneNumber = "045-6987234", Email = "amal@example.com", Password = "amal123" }
            );
            // Seed Attendees
            modelBuilder.Entity<Attendee>().HasData(
                new Attendee { AttendeeID = 1, Name = "Gihan", PhoneNumber = "074-7854120", Email = "gihan@example.com", Password = "gihan123" },
                new Attendee { AttendeeID = 2, Name = "Kasun", PhoneNumber = "078-7874140", Email = "kasun@example.com", Password = "kasun123" },
                new Attendee { AttendeeID = 3, Name = "Pahan", PhoneNumber = "075-6524781", Email = "pahan@example.com", Password = "pahan123" }
            );

        }

    }
}
