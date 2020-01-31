using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.Dto;
using WebApp.Models;
using WebApp.Persistence.UnitOfWork;
using static WebApp.Models.Enums;

namespace WebApp.Controllers
{
    [Authorize]
    [RoutePrefix("api/PriceList")]
    public class PriceListsController : ApiController
    {
        // private ApplicationDbContext db = new ApplicationDbContext();

        private IUnitOfWork db;
        public PriceListsController(IUnitOfWork db)
        {
            this.db = db;
        }

        [Authorize(Roles = "Admin")]
        [Route("GetPriceListAdmin")]
        // GET: api/PriceList/GetPriceListAdmin
        public IEnumerable<PriceListLine> GetPriceListAdmin()
        {
            List<PriceList> priceLists = db.PriceLists.GetAll().ToList();
            List<PriceListLine> ret = new List<PriceListLine>();
            //privremeno resenje citanja iz baze
            List<Price> prices = db.Prices.GetAll().ToList();


            foreach (var v in priceLists)
            {
                foreach (var price in prices)
                {
                    if (price.IdPrice == v.IdPriceList)
                    {
                        PriceListLine p = new PriceListLine();
                        p.ValidFrom = v.StartDate;
                        if (price.Type == Enums.TicketType.Hourly)
                        {
                            p.TypeOfTicket = "One-hour";
                        }
                        else if (price.Type == Enums.TicketType.Daily)
                        {
                            p.TypeOfTicket = "Day";
                        }
                        else if (price.Type == Enums.TicketType.Monthly)
                        {
                            p.TypeOfTicket = "Mounth";
                        }
                        else if (price.Type == Enums.TicketType.Annual)
                        {
                            p.TypeOfTicket = "Year";
                        }
                        //p.TypeOfTicket = Int32.Parse();//Int32.Parse(db.Prices.GetAll().FirstOrDefault(u => u.Type == price.Type).Type.ToString());
                        p.Value = price.Value;
                        p.IDPrice = price.IdPrice;
                        p.IDPriceList = v.IdPriceList;
                        ret.Add(p);
                    }
                }
            }

            return ret.OrderBy(o => o.ValidFrom).ToList(); 
        }

        // GET: api/PriceLists/5
        [ResponseType(typeof(PriceList))]
        public IHttpActionResult GetPriceList(int id)
        {
            PriceList priceList = db.PriceLists.Get(id);
            if (priceList == null)
            {
                return NotFound();
            }

            return Ok(priceList);
        }

        // PUT: api/PriceLists/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPriceList(int id, PriceList priceList)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != priceList.IdPriceList)
            {
                return BadRequest();
            }

            db.PriceLists.Update(priceList);

            try
            {
                db.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PriceListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "Admin")]
        [Route("PostPriceListLine")]
        // POST: api/PriceList/addPriceListLine
        [ResponseType(typeof(PriceList))]
        public string PostPriceListLine(PriceListLine priceListLine)
        {
            if (!ModelState.IsValid)
            {
                return "bad";
            }
            if (priceListLine == null)
            {
                return "null";
            }

            PriceList priceListExist = db.PriceLists.GetAll().FirstOrDefault(u => u.IdPriceList == priceListLine.IDPriceList );
            TicketType id = Enums.TicketType.Hourly;

            if (priceListLine.TypeOfTicket == "One-hour")
                id = Enums.TicketType.Hourly;
            else if (priceListLine.TypeOfTicket == "Day")
                id = Enums.TicketType.Daily;
            else if (priceListLine.TypeOfTicket == "Mounth")
                id = Enums.TicketType.Monthly;
            else if (priceListLine.TypeOfTicket == "Year")
                id = Enums.TicketType.Annual;

            Price priceExist = db.Prices.GetAll().FirstOrDefault(u => (u.IdPrice == priceListLine.IDPrice));
            Price newPrice = new Price();

            
            if (priceExist == null)
            {
                newPrice.Pricelists = new List<PriceList>();
                newPrice.Value = priceListLine.Value;
                newPrice.Type = id;
            }

            if (priceListExist == null)
            {
                PriceList newPriceList = new PriceList() { StartDate = priceListLine.ValidFrom, EndDate = priceListLine.ValidFrom, Prices = new List<Price>() };
                newPriceList.Prices = new List<Price>();
                if (priceExist == null)
                {
                    try
                    {
                        newPriceList.Prices.Add(newPrice);
                        db.PriceLists.Add(newPriceList);
                        newPrice.Pricelists.Add(newPriceList);
                        db.Prices.Add(newPrice);
                        db.Complete();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                else
                {
                    priceExist.Value = priceListLine.Value;
                    newPriceList.Prices.Add(priceExist);
                    db.PriceLists.Add(newPriceList);
                    priceExist.Pricelists.Add(newPriceList);
                    db.Prices.Update(priceExist);
                }
            }
            else
            {
                
                if (priceListExist.Prices != null)
                {
                    foreach (Price p in priceListExist.Prices)
                    {
                        if (p.Type == id)
                        {
                            return "type of ticket for this price list exists!";
                        }
                    }
                }

                if (priceExist == null)
                {
                    priceListExist.Prices = new List<Price>();
                    priceListExist.Prices.Add(newPrice);
                    db.PriceLists.Update(priceListExist);
                    newPrice.Pricelists.Add(priceListExist);
                    db.Prices.Add(newPrice);
                }
                else
                {
                    if (priceListExist.Prices == null)
                        priceListExist.Prices = new List<Price>();
                    if (priceExist.Pricelists == null)
                        priceExist.Pricelists = new List<PriceList>();

                    
                    
                    priceExist.Value = priceListLine.Value;
                    newPrice = priceExist;

                    db.PriceLists.Remove(priceListExist);
                    
                    db.Prices.Remove(priceExist);
                    newPrice.Pricelists.Remove(priceListExist);
                    priceListExist.Prices.Remove(priceExist);
                    priceListExist.Prices.Add(newPrice);
                    newPrice.Pricelists.Add(priceListExist);
                    db.PriceLists.Add(priceListExist);
                    db.Prices.Add(newPrice);
                }
            }

            try
            {
                db.Complete();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "ok";
        }

        [Authorize(Roles = "Admin")]
        [Route("EditPriceListLine")]
        // POST: api/PriceList/EditPriceListLine
        [ResponseType(typeof(PriceList))]
        public string EditPriceListLine(PriceListLine priceListLine)
        {
            if (!ModelState.IsValid)
            {
                return "bad";
            }
            if (priceListLine == null)
            {
                return "null";
            }

            TicketType type = Enums.TicketType.Hourly;

            if (priceListLine.TypeOfTicket == "One-hour")
                type = Enums.TicketType.Hourly;
            else if (priceListLine.TypeOfTicket == "Day")
                type = Enums.TicketType.Daily;
            else if (priceListLine.TypeOfTicket == "Mounth")
                type = Enums.TicketType.Monthly;
            else if (priceListLine.TypeOfTicket == "Year")
                type = Enums.TicketType.Annual;

            PriceList priceListExist = db.PriceLists.GetAll().FirstOrDefault(u => u.IdPriceList == priceListLine.IDPriceList);
            TicketType id = db.Prices.GetAll().FirstOrDefault(u => u.Type == type).Type;
           
            Price priceExist = db.Prices.GetAll().FirstOrDefault(u => (u.IdPrice == priceListLine.IDPrice));

            Price newPrice = new Price();

            PriceList priceList = new PriceList() { StartDate = priceListLine.ValidFrom, EndDate = priceListLine.ValidFrom, Prices = new List<Price>() };

           
            newPrice.Pricelists = new List<PriceList>();
            newPrice.Value = priceListLine.Value;
            newPrice.Type = db.Prices.GetAll().FirstOrDefault(u => u.Type == type).Type;
            
            priceList.Prices.Add(newPrice);
            Price priceFromBase = db.Prices.GetAll().FirstOrDefault(u => u.IdPrice == priceListLine.IDPrice);

           
            if (priceExist != null)
                db.Prices.Remove(priceExist);
            
            db.Prices.Add(newPrice);
            if (priceListExist != null)
                db.PriceLists.Remove(priceListExist);
            db.PriceLists.Add(priceList);

            
            try
            {
                db.Complete();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "ok";
        }

        [Authorize(Roles = "Admin")]
        [Route("DeleteLine/{IDPriceList}/{IDPrice}")]
        // DELETE: api/PriceLists/5
        [ResponseType(typeof(PriceList))]
        public IHttpActionResult DeletePriceList(int IdPriceList, int IDPrice)
        {
            PriceList priceList = db.PriceLists.GetAll().FirstOrDefault(u => u.IdPriceList == IdPriceList);
            if (priceList == null)
            {
                return NotFound();
            }

            Price price = db.Prices.GetAll().FirstOrDefault(u => u.IdPrice == IDPrice);
           
            db.Prices.Remove(price);
            db.PriceLists.Remove(priceList);
            db.Complete();
            

            return Ok(priceList);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PriceListExists(int id)
        {
            return db.PriceLists.GetAll().Count(e => e.IdPriceList == id) > 0;
        }
    }
}
