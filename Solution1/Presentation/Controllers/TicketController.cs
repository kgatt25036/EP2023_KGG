using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Controllers
{
    //turn on later[Authorize] // Requires authentication for accessing this controller
    public class TicketsController : Controller
    {
        private readonly TicketRepository _ticketRepository;
        private readonly FlightRepository _flightRepository;

        public TicketsController(TicketRepository ticketRepository, FlightRepository flightRepository)
        {
            _ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository));
            _flightRepository = flightRepository ?? throw new ArgumentNullException(nameof(flightRepository));
        }

        // Action to show a list of flights with retail prices
        /*public IActionResult ShowFlights()
        {
            var today = DateTime.Now.Date;

            // Retrieve all flights
            var flights = _flightRepository.GetFlights().ToList();

            // Filter flights that are not fully booked and have a future departure date
            var filteredFlights = flights
                .Where(f => f.DepartureDate > today && !_ticketRepository.GetTickets(f.Id).Any())
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
        }*/
        public IActionResult ShowFlights()
        {
            var today = DateTime.Now.Date;

            // Retrieve all flights with a future departure date
            var flights = _flightRepository.GetFlights()
                .Where(f => f.DepartureDate >= today)
                .ToList();

            /* Filter flights that are not fully booked
            var filteredFlights = flights
            .Where(f => !f.FullyBooked)
            .ToList();*/

            // Map the filtered flights to view models, make fully booked unclickable
            var flightViewModels = flights.Select(flight => new FlightsViewModels
            {
                Id = flight.Id,
                SeatRows = flight.SeatRows,
                SeatColumns = flight.SeatColumns,
                DepartureDate = flight.DepartureDate,
                ArrivalDate = flight.ArrivalDate,
                CountryFrom = flight.CountryFrom,
                CountryTo = flight.CountryTo,
                RetailPrice = CalculateRetailPrice(flight.WholesalePrice, flight.CommissionRate)
            }).ToList();

            return View(flightViewModels);
        }

        private double CalculateRetailPrice(double wholesalePrice, double commissionRate)
        {
            const double RetailPricePercentage = 1.2;
            return wholesalePrice * RetailPricePercentage;
        }
        public IActionResult FlightDetails(Guid id)
        {
            var flight = _flightRepository.GetFlight(id);

            if (flight == null)
            {
                // Handle invalid flight
                return RedirectToAction("ShowFlights");
            }

            var detailedFlightViewModel = new DetailedFlightViewModel
            {
                Id = flight.Id,
                SeatRows = flight.SeatRows,
                SeatColumns = flight.SeatColumns,
                DepartureDate = flight.DepartureDate,
                ArrivalDate = flight.ArrivalDate,
                CountryFrom = flight.CountryFrom,
                CountryTo = flight.CountryTo,
                RetailPrice = CalculateRetailPrice(flight.WholesalePrice, flight.CommissionRate),
                FullyBooked = flight.FullyBooked,
            };

            return View(detailedFlightViewModel);
        }
        [HttpGet]
        /*public IActionResult CreateTicket() { 
            CreateTicketViewModel viewModel = new CreateTicketViewModel(_flightRepository);
            return View(viewModel); 
        }*/
        public IActionResult CreateTicket(Guid flightId)
        {
            var viewModel = new CreateTicketViewModel(_flightRepository)
            {
                FlightIdFK = flightId
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateTicket(CreateTicketViewModel viewModel) {
            try
            { 
                _ticketRepository.Book(new Ticket()
                {
                SeatRow = viewModel.SeatRow,
                SeatColumn = viewModel.SeatColumn,
                FlightIdFK = viewModel.FlightIdFK,
                Passport = viewModel.Passport,
                PricePaid = viewModel.PricePaid,
                Cancelled = viewModel.Cancelled,
                });
                TempData["message"] = "Product saved successfully";
                return RedirectToAction("ShowAllTickets");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Product was not inserted successfully";
                return View(viewModel);

            }
            //return View(viewModel);//remove after adding other 2
        }
        public IActionResult ShowAllTickets()
        {
            var allTickets = _ticketRepository.GetAllTickets();

            var ticketViewModels = allTickets.Select(ticket => new TicketViewModel
            {
                Id = ticket.Id,
                SeatRow = ticket.SeatRow,
                SeatColumn = ticket.SeatColumn,
                FlightIdFK = ticket.FlightIdFK,
                Passport = ticket.Passport,
                PricePaid = ticket.PricePaid,
                Cancelled = ticket.Cancelled,
            }).ToList();

            return View(ticketViewModels);
        }
    }
}
