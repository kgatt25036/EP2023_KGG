using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Flight
    {
        public Flight()
        { }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public int SeatRows { get; set; }

        [Required]
        public int SeatColumns { get; set; }
        [Required]

        public DateTime DepartureDate { get; set; }
        [Required]
        public DateTime ArrivalDate { get; set; }
        [Required]
        public string CountryFrom { get; set; }
        [Required]
        public string CountryTo { get; set; }
        [Required]
        public double WholesalePrice { get; set; }
        [Required]
        public double CommissionRate { get; set; }
        public bool FullyBooked { get; set; }
    }
}
