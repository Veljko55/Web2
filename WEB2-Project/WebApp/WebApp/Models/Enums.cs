using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Enums
    {
        public enum PassengerType
        {
            Student = 1,
            Pensioner = 2,
            Regular = 3
        }

        public enum VerificationType
        {
            Process,
            Accepted,
            Rejected,
            Invalid
        }

        public enum RouteType
        {
            Suburban,
            Town
        }

        public enum TicketType
        {
            Hourly,
            Daily,
            Monthly,
            Annual
        }

        public enum DayType
        {
            Workday,
            Weekend
        }
    }
}