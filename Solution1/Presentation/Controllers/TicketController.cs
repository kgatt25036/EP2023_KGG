using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels;

namespace Presentation.Controllers
{
    /*[Authorize] // Requires authentication for accessing this controller
    public class TicketsController : Controller
    {
        private readonly TicketRepository ticketRepository;
        private readonly FlightRepository flightRepository;

        public TicketsController(TicketRepository ticketRepository, FlightRepository flightRepository)
        {
            this.ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository));
            this.flightRepository = flightRepository ?? throw new ArgumentNullException(nameof(flightRepository));
        }

        // Action to show a list of flights with retail prices
        public IActionResult ShowFlights()
        {
            var today = DateTime.Now.Date;

            // Retrieve flights that are not fully booked and have a future departure date
            var flights = flightRepository.GetFlights()
                .Where(f => !ticketRepository.GetTickets(f.Id).Any() && f.DepartureDate >= today)
                .ToList();

            return View(flights);
        }

        // Action to display flight details with retail prices
        public IActionResult FlightDetails(Guid flightId)
        {
            var flight = flightRepository.GetFlight(flightId);

            if (flight == null || flight.DepartureDate < DateTime.Now.Date)
            {
                // Handle invalid flight or past departure date
                return RedirectToAction("ShowFlights");
            }

            // Calculate retail price based on flight details (customize as needed)
            var retailPrice = CalculateRetailPrice(flight);

            // Pass flight and retail price to the view
            ViewData["RetailPrice"] = retailPrice;
            return View(flight);
        }

        // Action to display the booking form
        public IActionResult BookFlight(Guid flightId)
        {
            var flight = flightRepository.GetFlight(flightId);

            if (flight == null || flight.DepartureDate < DateTime.Now.Date)
            {
                // Handle invalid flight or past departure date
                return RedirectToAction("ShowFlights");
            }

            // Pass flight details to the view
            return View(flight);
        }

        // Action to handle the form submission and book the flight
        [HttpPost]
        public IActionResult BookFlight(Ticket ticket)
        {
            var flight = flightRepository.GetFlight(ticket.FlightIdFK);

            if (flight == null || flight.DepartureDate < DateTime.Now.Date)
            {
                // Handle invalid flight or past departure date
                return RedirectToAction("ShowFlights");
            }

            // Check if the seat is available for booking
            if (!ticketRepository.GetTickets(flight.Id).Any(t => t.SeatRow == ticket.SeatRow && t.SeatColumn == ticket.SeatColumn))
            {
                // Calculate PricePaid after commission on WholesalePrice
                ticket.PricePaid = CalculatePricePaid(flight.WholesalePrice, flight.CommissionRate);

                // Book the ticket
                ticketRepository.Book(ticket);

                // Redirect to a success page or show a success message
                return RedirectToAction("BookingSuccess");
            }
            else
            {
                // Handle seat already booked
                ModelState.AddModelError("Row", "Seat is already booked.");
                return View(flight);
            }
        }

        // Action to show a success message after booking
        public IActionResult BookingSuccess()
        {
            // You can customize this view to display a success message
            return View();
        }

        // Action to show the purchase history of the logged-in client
        public IActionResult PurchaseHistory()
        {
            // Retrieve the user's username from the authentication system
            var username = User.Identity.Name;

            // Retrieve tickets purchased by the logged-in client
            var tickets = ticketRepository.GetUserTickets(username).ToList();

            return View(tickets);
        }

        // Method to calculate PricePaid after commission on WholesalePrice
        private double CalculatePricePaid(double wholesalePrice, double commissionRate)
        {
            // Example calculation: PricePaid = WholesalePrice + (WholesalePrice * CommissionRate)
            return wholesalePrice + (wholesalePrice * commissionRate);
        }

        // Method to calculate retail price based on flight details (customize as needed)
        private double CalculateRetailPrice(Flight flight)
        {
            // Example calculation: retail price as a percentage of wholesale price
            const double RetailPricePercentage = 1.2;
            return flight.WholesalePrice * RetailPricePercentage;
        }
    }*/
    //turn on later[Authorize] // Requires authentication for accessing this controller
    public class TicketsController : Controller
    {
        private readonly TicketRepository ticketRepository;
        private readonly FlightRepository flightRepository;

        public TicketsController(TicketRepository ticketRepository, FlightRepository flightRepository)
        {
            this.ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository));
            this.flightRepository = flightRepository ?? throw new ArgumentNullException(nameof(flightRepository));
        }

        // Action to show a list of flights with retail prices
        public IActionResult ShowFlights()
        {
            var today = DateTime.Now.Date;

            // Retrieve all flights
            var flights = flightRepository.GetFlights().ToList();

            // Filter flights that are not fully booked and have a future departure date
            var filteredFlights = flights
                .Where(f => f.DepartureDate > today && !ticketRepository.GetTickets(f.Id).Any())
                .ToList();

            // Map the filtered flights to view models, make fully booked unclickable
            var flightViewModels = filteredFlights.Select(flight => new FlightsViewModels
            {
                Id = flight.Id,
                SeatRows = flight.SeatRows,
                SeatColumns = flight.SeatColumns,
                DepartureDate = flight.DepartureDate,
                ArrivalDate = flight.ArrivalDate,
                CountryFrom = flight.CountryFrom,
                CountryTo = flight.CountryTo,
                RetailPrice = CalculateRetailPrice(flight.WholesalePrice, flight.CommissionRate),
                FullyBooked = flight.FullyBooked
            }).ToList();

            return View(flightViewModels);
        }

        private double CalculateRetailPrice(double wholesalePrice, double commissionRate)
        {
            const double RetailPricePercentage = 1.2;
            return wholesalePrice * RetailPricePercentage;
        }
        [HttpGet]
        public IActionResult Create() { 
            CreateFlightViewModel viewModel = new CreateFlightViewModel();
            return View(viewModel); 
        }
        [HttpPost]
        public IActionResult Create(CreateFlightViewModel viewModel) { 
            flightRepository.Addflight//basically redo this for the tickets
            return View(viewModel); 
        }
    }
}
