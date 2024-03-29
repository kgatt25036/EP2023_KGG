﻿namespace Presentation.Models.ViewModels
{
    public class DetailedFlightViewModel
    {
        public Guid Id { get; set; }
        public int SeatRows { get; set; }
        public int SeatColumns { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public string CountryFrom { get; set; }
        public string CountryTo { get; set; }
        public double RetailPrice { get; set; }
        public bool FullyBooked { get; set; }
    }
}
