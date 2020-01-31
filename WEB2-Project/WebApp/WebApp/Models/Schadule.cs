using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static WebApp.Models.Enums;

namespace WebApp.Models
{
    public class Schadule
    {
        [Key]
        public int IdSchadule { get; set; }
        public RouteType Type { get; set; }
        public DayType Day { get; set; }
        public Line Line { get; set; }
        public List<Line> Lines { get; set; }
        public string DepartureTime { get; set; } //vreme polaska
        //[ForeignKey("Line")]
        //public int IdLine { get; set; }
    }
}