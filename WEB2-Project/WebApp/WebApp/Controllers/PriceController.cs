using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApp.Models;
using WebApp.Persistence.UnitOfWork;
using static WebApp.Models.Enums;

namespace WebApp.Controllers
{
    [Authorize]
    [RoutePrefix("api/Price")]
    public class PriceController : ApiController
    {

        private IUnitOfWork _unitOfWork;
        private DbContext _db;

        public PriceController(IUnitOfWork unitOfWork, DbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }

        public Price getLatestPrice(string ticket)
        {
            if (ticket == null)
            {
                return null;
            }

            TicketType ticketType = Enums.TicketType.Hourly;

            if (ticket == "One-hour")
                ticketType = Enums.TicketType.Hourly;
            else if (ticket == "Day")
                ticketType = Enums.TicketType.Daily;
            else if (ticket == "Mounth")
                ticketType = Enums.TicketType.Monthly;
            else if (ticket == "Year")
                ticketType = Enums.TicketType.Annual;

            List<PriceList> priceLists = _unitOfWork.PriceLists.GetAll().OrderByDescending(u => u.StartDate).ToList();
            TicketType idType = _unitOfWork.Tickets.GetAll().FirstOrDefault(u => u.Type == ticketType).Type;

            List<Price> prices = _unitOfWork.Prices.GetAll().ToList();

            foreach (var pr in prices)
            {
                if (pr.Type == ticketType)
                {
                    return pr;
                }
            }

            foreach (PriceList pl in priceLists)
            {
                if (pl.Prices != null)
                {
                    foreach (Price p in pl.Prices)
                    {
                        if (p.Type == idType)
                        {
                            return p;
                        }
                    }
                }
            }

            return null;
        }
               

        [AllowAnonymous]
        [Route("GetOnePrice")]
        public double GetOnePrice(string ticket, string user)
        {
            if (ticket == null || user == null)
            {
                return 0;
            }

            TicketType ticketType = Enums.TicketType.Hourly;

            if (ticket == "One-hour")
                ticketType = Enums.TicketType.Hourly;
            else if (ticket == "Day")
                ticketType = Enums.TicketType.Daily;
            else if (ticket == "Mounth")
                ticketType = Enums.TicketType.Monthly;
            else if (ticket == "Year")
                ticketType = Enums.TicketType.Annual;

            
            PassengerType userType = Enums.PassengerType.Regular;

            if (user == "Regular")
                userType = Enums.PassengerType.Regular;
            else if (user == "Student")
                userType = Enums.PassengerType.Student;
            else if (user == "Pensioner")
                userType = Enums.PassengerType.Pensioner;

            double pretenge = 1; 

            double popust = 0; 

            var tickett = _unitOfWork.Tickets.GetAll().FirstOrDefault(u => u.Type == ticketType);


            Price pricee = getLatestPrice(ticket);
            double priceRet = 0;
            if (pricee != null)
            {
                priceRet = pricee.Value;
            }
            else
            {
                pricee = new Price();
                pricee.Value = GetDefaultPriceValue(ticket);
                priceRet = pricee.Value;
            }


            if(user == "Student" || user == "Pensioner")
            {
                popust = 10;
                pretenge = popust / 100;
                priceRet = pricee.Value - pricee.Value * pretenge;
            }


            if (pricee == null)
                return 0;

            return priceRet; 
        }

        [Authorize(Roles = "AppUser")]
        [Route("GetPrice")]
        public double GetPrice(string ticket, string email)
        {

            TicketType ticketType = Enums.TicketType.Hourly;

            if (ticket == "One-hour")
                ticketType = Enums.TicketType.Hourly;
            else if (ticket == "Day")
                ticketType = Enums.TicketType.Daily;
            else if (ticket == "Mounth")
                ticketType = Enums.TicketType.Monthly;
            else if (ticket == "Year")
                ticketType = Enums.TicketType.Annual;

            var email1 = Request.GetOwinContext().Authentication.User.Identity.Name;
            ApplicationUserManager cont = Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            List<ApplicationUser> app = cont.Users.ToList();

            ApplicationUser apUs = app.Where(u => u.Email == email).FirstOrDefault();

            var tickett = _unitOfWork.Tickets.GetAll().FirstOrDefault(u => u.Type == ticketType); //koja karta

            var pricee = getLatestPrice(ticket);// _unitOfWork.Prices.GetAll().FirstOrDefault(u => u.IDtypeOfTicket == tickett.IDtypeOfTicket);//koliko kosta
            //var userr = _unitOfWork.TypesOfUser.GetAll().FirstOrDefault(u => u.IDtypeOfUser == apUs.IDtypeOfUser);
            //double popust = (double)userr.Percentage;
            double popust = 5;
            double priceRet = 0;
            if (pricee != null)
            {
                 priceRet = pricee.Value;
            }
            else
            {
                pricee = new Price();
                pricee.Value = GetDefaultPriceValue(ticket);
                priceRet = pricee.Value;
            }

            if (apUs.PassengerType == Enums.PassengerType.Student || apUs.PassengerType == Enums.PassengerType.Pensioner)
            {
                popust = 10;
                priceRet = pricee.Value *(1 - popust/100) ;
            }
            
            return priceRet;
        }

        private double GetDefaultPriceValue(string ticketType)
        {
            if (ticketType == "One-hour")
                return 12;
            else if (ticketType == "Day")
                return 20;
            else if (ticketType == "Mounth")
               return 140;
            else if (ticketType == "Year")
               return 1200;
            else 
                return 50;
        }


    }
}
