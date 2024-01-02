using DataAccess.Repositories;
using Domain.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels
{
    public class CreateTicketViewModel
    {
        public CreateTicketViewModel() { }
        public CreateTicketViewModel(FlightRepository flightRepository) {
                Flights = flightRepository.GetFlights();
        }

        [Required]
        public string SeatRow { get; set; }
        [Required]
        public string SeatColumn { get; set; }
        public IQueryable<Flight> Flights { get; set; }
        public Guid FlightIdFK { get; set; }
        public string Passport { get; set; }
        public bool PricePaid { get; set; }
        public bool Cancelled { get; set; }
    }
}
