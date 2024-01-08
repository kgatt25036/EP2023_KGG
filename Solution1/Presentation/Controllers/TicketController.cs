using DataAccess.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;
using System.Security.Claims;

namespace Presentation.Controllers
{
    public class TicketsController : Controller
    {
        //private readonly ITickets _ticketRepository;
        private readonly TicketRepository _ticketRepository;
        private readonly FlightRepository _flightRepository;
        private readonly UserManager<CustomUser> _userManager;
        public TicketsController(TicketRepository ticketRepository, FlightRepository flightRepository, UserManager<CustomUser> userManager )
        {
            _userManager = userManager;
            _ticketRepository = ticketRepository ?? throw new ArgumentNullException(nameof(ticketRepository));
            _flightRepository = flightRepository ?? throw new ArgumentNullException(nameof(flightRepository));
        }
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
        //[authorize]
        public IActionResult CreateTicket(Guid flightId)
        {
            var flight = _flightRepository.GetFlight(flightId);

            if (flight == null)
            {
                // Handle invalid flight
                return RedirectToAction("ShowFlights");
            }
            var viewModel = new CreateTicketViewModel()
            {
                FlightIdFK = flightId,
                Flight = flight,
                price = CalculateRetailPrice(flight.WholesalePrice, flight.CommissionRate)
            };

            return View(viewModel);
        }

        [HttpPost]
        //[authorize]
        public IActionResult CreateTicket(CreateTicketViewModel viewModel, [FromServices] IWebHostEnvironment host)
        {
            string relativepath = "";
            try
            {

                string filename = Guid.NewGuid() + System.IO.Path.GetExtension(viewModel.PassImgFile.FileName);
                string absolutePath = host.WebRootPath+@"\images\"+filename;
                relativepath = @"/images/" + filename;
                using (FileStream fs = new FileStream(absolutePath, FileMode.CreateNew))
                {
                    viewModel.PassImgFile.CopyTo(fs);
                    fs.Flush();
                    fs.Close();
                }
                    _ticketRepository.Book(new Ticket()
                    {
                        SeatRow = viewModel.SeatRow,
                        SeatColumn = viewModel.SeatColumn,
                        FlightIdFK = viewModel.FlightIdFK,
                        Passport = viewModel.Passport,
                        PricePaid = viewModel.PricePaid,
                        Cancelled = viewModel.Cancelled,
                        PassportImg = relativepath
                    });
                TempData["message"] = "Product saved successfully";
                return RedirectToAction("ShowAllTickets");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Product was not inserted successfully";
                return View(viewModel);
            }
        }
        [Authorize]
        public async Task<IActionResult> ShowAllTickets()//only showing one ticket
        {
            try {

                // Get the current user's passport number
                var currentUser = await _userManager.FindByEmailAsync(User.Identity.Name);



            // Retrieve tickets for the current user based on passport number
            var userTickets = _ticketRepository.GetTicketsByPassport(currentUser.Passportno).ToList();

            // Map tickets to view models
            var ticketViewModels = userTickets.Select(ticket => new TicketViewModel
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
        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            // Retrieve the ticket by id and display the confirmation page
            var ticket = _ticketRepository.GetTicketById(id);
            var viewModel = new DetailedTicketViewModel
            {
                Id = ticket.Id,
                SeatRow = ticket.SeatRow,
                SeatColumn = ticket.SeatColumn,
                FlightIdFK = ticket.FlightIdFK,
                Passport = ticket.Passport,
                PricePaid = ticket.PricePaid,
                Cancelled = ticket.Cancelled,
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(Guid id)
        {
            // Cancel the ticket (update its status)
            _ticketRepository.CancelTicket(id);

            // Redirect to the ShowAllTickets page after deletion
            return RedirectToAction("ShowAllTickets");
        }
        public IActionResult GetSeatingPlan(Guid flightId)
        {
            // Retrieve flight details by ID
            var flight = _flightRepository.GetFlight(flightId);

            if (flight == null)
            {
                // Handle invalid flight ID
                return BadRequest("Invalid flight ID");
            }
            
            // Return a JSON object with seating plan details
            var seatingPlan = new
            {
                SeatRows = flight.SeatRows,
                SeatColumns = flight.SeatColumns
            };

            return Json(seatingPlan);
        }
    }
}
