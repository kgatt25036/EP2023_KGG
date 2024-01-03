using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Presentation.Controllers
{
    public class Seat
        {
            public int Id { get; set; }
            public int Column { get; set; }
            public int Row { get; set; }
            public string RowCol { get { 
                return Row +" "+Column;
                } }
        }
    public class SeatingPlanViewModel
    {
        public List<Seat> SeatList { get; set; }
        public string SeatChosen { get; set; }

        public int MaxRows { get; set; }
        public int MaxColumns { get; set; }
    }
    public class UIGeneratedController : Controller
    {

        public IActionResult Createseats()
        {
            int maxrows = 10;
            int maxcolumns = 10;
            List<Seat> seats = new List<Seat>();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    seats.Add(new Seat() { Row = i, Column = j });
                }
            }
            return View(new SeatingPlanViewModel() { SeatList = seats, MaxColumns = maxcolumns, MaxRows = maxrows});
        }
    }
}
