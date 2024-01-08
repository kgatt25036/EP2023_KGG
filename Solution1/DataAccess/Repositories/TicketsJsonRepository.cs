using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Net.Http.Headers;

namespace DataAccess.Repositories
{
    public class TicketsJsonRepository : ITickets
    {
        private string _path;
        string? allText = "";

        public TicketsJsonRepository(string path) { 
            _path = path;
            using (FileStream fs = File.Create(path))
            {
                fs.Close();
            }
        }
        public void Book(Ticket ticket)
        {
            ticket.Id = Guid.NewGuid();
            var list = GetAllTickets();
            list.Add(ticket);
            var allTicketText = JsonSerializer.Serialize(list);
            try
            {
                File.WriteAllText(_path, allTicketText);
            }
            catch (Exception ex) {
                throw new Exception("Error while adding product");
            }
        }

        public void CancelTicket(Guid ticketId)
        {
            throw new NotImplementedException();
        }

        public List<Ticket> GetAllTickets()
        {
            if (File.Exists(_path))
            {
                try { 
                using (StreamReader sr = File.OpenText(_path))
                {
                    allText = sr.ReadToEnd();
                    sr.Close();
                }
                if (string.IsNullOrEmpty(allText))
                {
                    return new List<Ticket>();
                }
                    List<Ticket> myTickets = JsonSerializer.Deserialize<List<Ticket>>(allText);
                    return myTickets;
                }
                catch(Exception ex)
                {   
                    throw new Exception("Error while opening the file");
                }
            }
            else { throw new Exception("File source not found"); }
        }

        public Ticket GetTicketById(Guid ticketId)
        {
            var list = GetAllTickets();
            return list.FirstOrDefault(t => t.Id == ticketId);
        }

        public IEnumerable<Ticket> GetTicketsByPassport(string passport)
        {
            var allTickets = GetAllTickets();
            return allTickets.Where(ticket => ticket.Passport == passport);
        }

        public bool IsSeatAvailable(Guid flightId, string row, string column)
        {
            var allTickets = GetAllTickets();
            return !allTickets.Any(t => t.FlightIdFK == flightId && t.SeatRow == row && t.SeatColumn == column);
        }
    }
}
