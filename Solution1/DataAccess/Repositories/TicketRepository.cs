using DataAccess.DataContext;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    //public class TicketRepository: ITickets
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
        public void CancelTicket(Guid ticketId)
        {
            var ticket = _airlineDbContext.Tickets.FirstOrDefault(t => t.Id == ticketId);

            if (ticket != null)
            {
                ticket.Cancelled = true;
                _airlineDbContext.SaveChanges();
            }
        }

        /* Get all tickets for a specific flight
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
        }*/

        // Check if a seat is available for booking
        public bool IsSeatAvailable(Guid flightId, string row, string column)//make sure ticket isnt cancelled
        {
            return !_airlineDbContext.Tickets
                .Any(t => t.FlightIdFK == flightId && t.SeatRow == row && t.SeatColumn == column);
        }
        public List<Ticket> GetAllTickets()
        {
            return _airlineDbContext.Tickets.ToList();
        }
        public Ticket GetTicketById(Guid ticketId)
        {
            return _airlineDbContext.Tickets.FirstOrDefault(t => t.Id == ticketId);
        }
        public IEnumerable<Ticket> GetTicketsByPassport(string passport)
        {
            // Assuming you have a method to get all tickets
            var allTickets = GetAllTickets();

            // Filter tickets based on the given passport number
            var userTickets = allTickets.Where(ticket => ticket.Passport == passport);

            return userTickets;
        }
    }
}
