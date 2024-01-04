using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.ViewModels
{
    public class FlightsViewModels
    {
        public Guid Id { get; set; } //= Guid.NewGuid();

        [Required]
        public int SeatRows { get; set; }

        [Required]
        public int SeatColumns { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string CountryFrom { get; set; }
        public string CountryTo { get; set; }
        public double RetailPrice { get; set; }

    }
}
