using DataAccess.DataContext;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class TicketRepository
    {
        private readonly AirlineDbContext _airlineDbContext;

        public TicketRepository(AirlineDbContext airlineDbContext)
        {
            _airlineDbContext = airlineDbContext ?? throw new ArgumentNullException(nameof(airlineDbContext));
        }

        // Book a new ticket
        public void Book(Ticket ticket)
        {
            if (IsSeatAvailable(ticket.FlightIdFK, ticket.SeatRow, ticket.SeatColumn))
            {
                _airlineDbContext.Tickets.Add(ticket);
                _airlineDbContext.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Seat is already booked.");
            }
        }

        // Cancel a booked ticket
        public void Cancel(int ticketId)
        {
            var ticket = _airlineDbContext.Tickets.Find(ticketId);

            if (ticket != null)
            {
                ticket.Cancelled = true;
                _airlineDbContext.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Ticket not found.");
            }
        }

        // Get all tickets for a specific flight
        public IQueryable<Ticket> GetTickets(Guid flightId)
        {
            return _airlineDbContext.Tickets
                .Where(t => t.FlightIdFK == flightId)
                .AsQueryable();
        }
        //gets tickets for a user 
        public IQueryable<Ticket> GetUserTickets(string passport)
        {
            return _airlineDbContext.Tickets
                .Where(t => t.Passport == passport)
                .AsQueryable();
        }

        // Check if a seat is available for booking
        private bool IsSeatAvailable(Guid flightId, string row, string column)
        {
            return !_airlineDbContext.Tickets
                .Any(t => t.FlightIdFK == flightId && t.SeatRow == row && t.SeatColumn == column);
        }
        public List<Ticket> GetAllTickets()
        {
            return _airlineDbContext.Tickets.ToList();
        }
    }
}
