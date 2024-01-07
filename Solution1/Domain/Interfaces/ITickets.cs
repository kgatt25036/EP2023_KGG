using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ITickets
    {
        void Book(Ticket ticket);
        void CancelTicket(Guid ticketId);
        bool IsSeatAvailable(Guid flightId, string row, string column);
        List<Ticket> GetAllTickets();
        Ticket GetTicketById(Guid ticketId);
        IEnumerable<Ticket> GetTicketsByPassport(string passport);
    }
}
