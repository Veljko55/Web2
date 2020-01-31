using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static WebApp.Models.Enums;

namespace WebApp.Models
{
    public class Ticket
    {
        [Key]
        public int IdTicket { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public PassengerType Passenger { get; set; }
        public TicketType Type { get; set; }
        public string UserName { get; set; }
        public bool IsChecked { get; set; }
    }
}