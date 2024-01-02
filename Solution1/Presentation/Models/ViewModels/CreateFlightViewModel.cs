using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.ViewModels
{
    public class CreateFlightViewModel
    {
        [Required]
        public string SeatRows { get; set; }

        [Required]
        public string SeatColumns { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string CountryFrom { get; set; }
        public string CountryTo { get; set; }
        public double WholesalePrice { get; set; }
        public double CommissionRate { get; set; }
        public bool FullyBooked { get; set; }
    }
}
