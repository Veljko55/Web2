using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApp.Models;
using WebApp.Persistence.UnitOfWork;

namespace WebApp.Controllers
{
    [Authorize]
    [RoutePrefix("api/Station")]
    public class StationsController : ApiController
    {  
        private IUnitOfWork db;
        public StationsController(IUnitOfWork db)
        {
            this.db = db;
        }


        [AllowAnonymous]
        [Route("Get")]
        public IEnumerable<Station> Get(string Number)
        {

            var line = db.Lines.GetAll().Where(l => l.Number == Number).FirstOrDefault();

            return line.Stations;
        }

        [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        [Route("GetAll")]
        public IEnumerable<Station> GetStations()
        {

            return db.Stations.GetAll();
        }

        [Authorize(Roles = "Admin")]
        [Route("GetStations")]
        public IEnumerable<Station> GetStationsForLine()
        {            
            return db.Stations.GetAll();
        }

        [Authorize(Roles = "Admin")]
        [Route("UpdateStation")]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStation([FromBody]Station station)
        {
            int result = 1;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Stations.Update(station);

            result = db.Complete();

            if (result == 0)
            {
                return Conflict();
            }
            else if (result == -1)
            {
                return BadRequest("Data was modified in meantime, please try again!");
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Authorize(Roles = "Admin")]
        [Route("Add")]
        [ResponseType(typeof(Station))]
        public IHttpActionResult PostStation(Station station)
        {
            int result = 1;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (StationExists(station.Name))
            {
                return BadRequest("Station with this name alredy exist! Try again.");
            }
            else
            {
                db.Stations.Add(station);
            }

            try
            {
                result = db.Complete();
            }
            catch (Exception)
            {
                if (StationExists(station.Name))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }
            if (result == 0)
            {
                return Conflict();
            }
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("Delete/{id}/")]
        [ResponseType(typeof(Station))]
        public IHttpActionResult DeleteStation(string id)
        {
            List<Station> stations = db.Stations.GetAll().ToList();
            Station station = null;

            foreach (var s in stations)
            {
                if(id == s.Name)
                {
                    station = db.Stations.Get(s.IdStation);
                }
            }

            if (station == null)
            {
                return NotFound();
            }

            db.Stations.Remove(station);
            db.Complete();

            return Ok(station);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StationExists(string id)
        {
            return db.Stations.GetAll().Count(e => e.Name == id) > 0;
        }
    }
}
