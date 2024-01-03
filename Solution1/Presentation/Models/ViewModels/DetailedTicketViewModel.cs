using Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels
{
    public class DetailedTicketViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string SeatRow { get; set; }
        [Required]
        public string SeatColumn { get; set; }

        [ForeignKey("Flight")]
        public Guid FlightIdFK { get; set; }
        public virtual Flight Flight { get; set; }
        public string Passport { get; set; }
        public bool PricePaid { get; set; }
        public bool Cancelled { get; set; }
    }
}
