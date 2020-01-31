using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static WebApp.Models.Enums;

namespace WebApp.Models
{
    public class Price
    {
        [Key]
        public int IdPrice { get; set; }
        public TicketType Type { get; set; }
        public double Value { get; set; }
        public List<PriceList> Pricelists { get; set; }
    }
}