using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels;
using DataAccess.Repositories;
using System.Security.Claims;

namespace Presentation.Controllers
{
    public class AdminController : Controller
    {
        private readonly FlightRepository _flightRepository;
        private readonly TicketRepository _ticketRepository;

        public AdminController(FlightRepository flightRepository, TicketRepository ticketRepository)
        {
            _flightRepository = flightRepository ?? throw new ArgumentNullException(nameof(flightRepository));
            _ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository));
        }

        public IActionResult SelectFlights()
        {
            // Retrieve all available flights
            var flights = _flightRepository.GetFlights().ToList();

            return View(flights);
        }
        public IActionResult GetTickets()
        {
            // Retrieve all tickets
            var allTickets = _ticketRepository.GetAllTickets();

            // Map all tickets to view models
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

            // Pass the list of tickets to the view
            return View(ticketViewModels);
        }


        public IActionResult ViewTicketDetails(Guid ticketId)
        {
            // Retrieve ticket details by ID
            var ticketDetails = _ticketRepository.GetTicketById(ticketId);

            if (ticketDetails == null)
            {
                // Handle invalid ticket ID
                return RedirectToAction("SelectFlights");
            }

            var viewModel = new DetailedTicketViewModel
            {
                Id = ticketDetails.Id,
                SeatRow = ticketDetails.SeatRow,
                SeatColumn = ticketDetails.SeatColumn,
                FlightIdFK = ticketDetails.FlightIdFK,
                Passport = ticketDetails.Passport,
                PassportImg = ticketDetails.PassportImg,
                PricePaid = ticketDetails.PricePaid,
                Cancelled = ticketDetails.Cancelled,
            };

            return View(viewModel);
        }
    }
}
