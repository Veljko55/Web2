using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Location
    {
        [Key]
        public int IdLocation { get; set; }
        public string Address { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
    }
}