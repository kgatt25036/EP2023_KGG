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
        

        [Required]
        public string SeatRow { get; set; }
        [Required]
        public string SeatColumn { get; set; }
        public Flight Flight { get; set; }
        [Required]
        public Guid FlightIdFK { get; set; }
        [Required]
        public string Passport { get; set; }
        public double price { get; set; }
        public bool PricePaid { get; set; }
        public bool Cancelled { get; set; }
    }
}
