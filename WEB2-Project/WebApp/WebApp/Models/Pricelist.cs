using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class PriceList
    {
        [Key]
        public int IdPriceList { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool InUse { get; set; }
        public List<Price> Prices { get; set; }
    }
}