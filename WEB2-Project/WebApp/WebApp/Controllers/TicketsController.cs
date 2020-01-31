using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.Models;
using WebApp.Persistence;
using WebApp.Persistence.UnitOfWork;
using static WebApp.Models.Enums;

namespace WebApp.Controllers
{
    [Authorize]
    [RoutePrefix("api/Tickets")]
    public class TicketsController : ApiController
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        private IUnitOfWork db;
        public TicketsController(IUnitOfWork db)
        {
            this.db = db;
        }

        public TicketsController() { }

        [Authorize(Roles = "AppUser")]
        [Route("Tickets")]
        public IEnumerable<Ticket> GetTickets()
        {
            //diskutabilna funkcija?

            var email1 = Request.GetOwinContext().Authentication.User.Identity.Name;
            var tickets = db.Tickets.GetAll().Where(t => t.UserName == email1).ToList();

            return db.Tickets.GetAll().Where(t => t.UserName == email1).ToList();

        }

        [Authorize(Roles = "AppUser")]
        [Route("Tickets")]
        public IEnumerable<Ticket> GetTickets([FromUri]string email)
        {
            return db.Tickets.GetAll().Where(t => t.UserName == email && t.Type == Enums.TicketType.Hourly);

        }

        [Authorize(Roles = "AppUser")]
        [Route("AllTickets")]
        public IEnumerable<Ticket> GetAllTickets()
        {

            var email1 = Request.GetOwinContext().Authentication.User.Identity.Name;


            return db.Tickets.GetAll().Where(t => t.UserName == email1);

        }

        [Authorize(Roles = "Controller")]
        [Route("CheckValidation")]
        [ResponseType(typeof(Ticket))]
        public string GetTicket(double Id)
        {
            if (((Id % 1) != 0))
                return "Unvalid ticket id. Try without point ->.";

            int id = Convert.ToInt32(Id);
            Ticket ticket = db.Tickets.Get(id);

            if (ticket == null)
            {
                return "Ticket with this id - not found!";
            }

            //valid is time expired
            if (!ticket.IsChecked)
            {
                return "This ticked is not checked!";
            }

            if (ticket.To < DateTime.Now)
            {
                return "Time of this ticket is expired!";
            }


            ////One-hour
            //if (ticket.Type == Enums.TicketType.Hourly)
            //{

            //    if (ticket.From == ticket.To)
            //    {
            //        result = "Not checked in yet. Invalid.";

            //    }
            //    else if (ticket.From > ticket.To)
            //    {
            //        dateTime = ticket.From.AddHours(1);

            //        if (ticket.From > dateTime)
            //        {
            //            result = "1 hour has expired. Invalid.";
            //        }
            //        else
            //        {
            //            result = "Valid ticket!";
            //        }
            //    }

            //    //Day
            //}
            //else if (ticket.Type == Enums.TicketType.Daily)
            //{
            //    dateTime = DateTime.Now;

            //    if (ticket.To > dateTime)
            //    {
            //        result = "Valid ticket!";
            //    }
            //    else
            //    {
            //        result = "Day has expired. Invalid";
            //    }
            //    //Mounth
            //}
            //else if (ticket.Type == Enums.TicketType.Monthly)
            //{
            //    dateTime = DateTime.Now;

            //    if (ticket.To.Month == dateTime.Month && ticket.To.Year == dateTime.Year)
            //    {
            //        result = "Valid ticket";
            //    }
            //    else
            //    {
            //        result = "Month has expired. Invalid";
            //    }

            //    //Year
            //}
            //else if (ticket.Type == Enums.TicketType.Annual)
            //{
            //    dateTime = DateTime.Now;

            //    if (ticket.To.Year == dateTime.Year)
            //    {
            //        result = "Valid ticket";
            //    }
            //    else
            //    {
            //        result = "Year has expired. Invalid";
            //    }
            //}

            return "This ticket is valid!";
        }

        [AllowAnonymous]
        [Route("Buy")]
        [ResponseType(typeof(Ticket))]
        public IHttpActionResult PostTicket(string Type, string UserName)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Ticket ticket = new Ticket();
            ticket.From = DateTime.Now;
            ticket.To = ticket.From;
            ticket.UserName = UserName;
            ticket.IsChecked = false ;


            TicketType type = Enums.TicketType.Hourly;
            if (Type == "One-hour")
            {
                ticket.To = ticket.From.AddHours(1);
                type = Enums.TicketType.Hourly;
            }
            else if (Type == "Day")
            {
                ticket.To = ticket.From.AddDays(1);
                type = Enums.TicketType.Daily;
            }
            else if (Type == "Mounth")
            {
                ticket.To = ticket.From.AddMonths(1);
                type = Enums.TicketType.Monthly;
            }
            else if (Type == "Year")
            {
                ticket.To = ticket.From.AddYears(1);
                type = Enums.TicketType.Annual;
            }

            ticket.Type = type;

            db.Tickets.Add(ticket);
            db.Complete();

            var user = Request.GetOwinContext().Authentication.User.Identity.Name;

            return Ok();
        }

        [Authorize(Roles = "AppUser")]
        [Route("CheckIn")]
        [HttpPut]
        public IHttpActionResult CheckInTicket([FromBody]Ticket t)
        {
            Ticket ticket = db.Tickets.Get(t.IdTicket);
            if (ticket == null)
            {
                return NotFound();
            }
            if (ticket.To < DateTime.Now)
            {
                //kasno si se chekirao sta cu ti ja sad
                return BadRequest();
            }

            ticket.IsChecked = true;
            db.Tickets.Update(ticket);

            db.Complete();

            return Ok(ticket);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TicketExists(int id)
        {
            return db.Tickets.GetAll().Count(e => e.IdTicket == id) > 0;
        }
    }
}
