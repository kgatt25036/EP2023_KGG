using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Ticket
    {
        public Ticket() { } 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string SeatRow { get; set; }
        [Required]
        public string SeatColumn { get; set; }

        [ForeignKey("Flight")]
        public Guid FlightIdFK { get; set; }
        public virtual Flight Flight { get; set; }
        [Required]
        public string Passport { get; set; }
        public bool PricePaid { get; set; }
        public bool Cancelled { get; set; }
    }
}
