using DataAccess.DataContext;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class FlightRepository
    {
        public AirlineDbContext airlineDbContext { get; set; }

        public FlightRepository(AirlineDbContext airlineDbContext)
        {
            this.airlineDbContext = airlineDbContext ?? throw new ArgumentNullException(nameof(airlineDbContext));
        }

        // Retrieve a specific flight by id
        public Flight GetFlight(Guid flightId)
        {
            return airlineDbContext.Flights.FirstOrDefault(f => f.Id == flightId);
        }

        // Retrieve all flights
        public IQueryable<Flight> GetFlights()
        {
            return airlineDbContext.Flights.AsQueryable();
        }
        // Retrieve available flights for display
        public IQueryable<Flight> GetAvailableFlights()
        {
            var currentDate = DateTime.Now;

            return airlineDbContext.Flights
                .Where(f => f.DepartureDate > currentDate && !IsFlightFullyBookedOrCancelled(f.Id))
                .AsQueryable();
        }
        private bool IsFlightFullyBookedOrCancelled(Guid flightId)
        {
            return airlineDbContext.Tickets.Any(t => t.FlightIdFK == flightId); //&& !t.Cancelled);
        }


        // Book a new flight
        public void BookFlight(Flight flight)
        {
            airlineDbContext.Flights.Add(flight);
            airlineDbContext.SaveChanges();
        }

        // Update flight information
        public void UpdateFlight(Flight flight)
        {
            var originalFlight = GetFlight(flight.Id);
            if (originalFlight != null)
            {
                // Update flight properties based on your requirements
                originalFlight.SeatRows = flight.SeatRows;
                originalFlight.SeatColumns = flight.SeatColumns;
                originalFlight.DepartureDate = flight.DepartureDate;
                originalFlight.ArrivalDate = flight.ArrivalDate;
                originalFlight.CountryFrom = flight.CountryFrom;
                originalFlight.CountryTo = flight.CountryTo;
                originalFlight.WholesalePrice = flight.WholesalePrice;
                originalFlight.CommissionRate = flight.CommissionRate;

                airlineDbContext.SaveChanges();
            }
        }

        // Delete a flight
        public void DeleteFlight(Guid flightId)
        {
            var flightToDelete = GetFlight(flightId);
            if (flightToDelete != null)
            {
                airlineDbContext.Flights.Remove(flightToDelete);
                airlineDbContext.SaveChanges();
            }
            else
            {
                throw new Exception("No flight to delete.");
            }
        }
    }
}
