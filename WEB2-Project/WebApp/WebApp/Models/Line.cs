using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static WebApp.Models.Enums;

namespace WebApp.Models
{
    public class Line
    {
        [Key]
        public int IdLine { get; set; }
        public string Number { get; set; }
        public RouteType RouteType { get; set; }
        public List<Station> Stations { get; set; }
        public virtual List<Schadule> Schadules { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}