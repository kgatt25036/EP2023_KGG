using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels;
using DataAccess.Repositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Domain.Interfaces;

namespace Presentation.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly FlightRepository _flightRepository;
        //private readonly ITickets _ticketRepository;
        private readonly TicketRepository _ticketRepository;


        public AdminController(FlightRepository flightRepository, TicketRepository ticketRepository)
        {
            _flightRepository = flightRepository ?? throw new ArgumentNullException(nameof(flightRepository));
            _ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository));
        }

        public IActionResult ShowFlights()
        {
            try { 
            // Retrieve all available flights
            var flights = _flightRepository.GetFlights().ToList();
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
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
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
        public IActionResult ShowAllTickets()
        {
            try { 
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
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }


        public IActionResult DetailedTickets(Guid id)
        {
            var ticketDetails = _ticketRepository.GetTicketById(id);
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
