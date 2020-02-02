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
    [RoutePrefix("api/Schedule")]
    public class ScheduleController : ApiController
    {       
        private IUnitOfWork db;
        public ScheduleController(IUnitOfWork db)
        {
            this.db = db;
        }

        [Authorize(Roles = "Admin")]
        [Route("PostLineSchedule")]
        // POST: api/Schedules
        [ResponseType(typeof(Schadule))]
        public IHttpActionResult PostLineSchedule([FromBody]ScheduleLine sl)
        {
            

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (sl == null)
            {
                return NotFound();
            }

            DayType dd = DayType.Workday;
            if (sl.Day == "Work day")
            {
                dd = Enums.DayType.Workday;
            }
            else if (sl.Day == "Weekend")
            {
                dd = Enums.DayType.Weekend;
            }

            Schadule d = new Schadule { Day = dd, DepartureTime = sl.Time.ToString() };
            if (d.Lines == null)
            {
                d.Lines = new List<Line>();
            }
            var line = db.Lines.GetAll().FirstOrDefault(u => u.Number == sl.Number);
            if (line.Stations == null)
            {
                line.Stations = new List<Station>();
            }

            Schadule exist = db.Schadules.GetAll().FirstOrDefault(u => (u.DepartureTime == sl.Time.ToString() && u.Day == dd && u.Line.ToString()==sl.Number));
            if (exist == null)
            {

                d.Lines.Add(line);
                d.Line = line;
                db.Schadules.Add(d);
                line.Schadules.Add(d);
                db.Lines.Update(line);
            }
            else
            {
                if (line.Schadules.FirstOrDefault(u => (u.DepartureTime == sl.Time.ToString() && u.Day == dd)) == null)
                {
                    exist.Lines.Add(line);
                    db.Schadules.Update(exist);
                    line.Schadules.Add(exist);
                    db.Lines.Update(line);
                }
            }

            db.Complete();

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("EditLineSchedule")]
        // POST: api/Schedules
        [ResponseType(typeof(Schadule))]
        public IHttpActionResult EditLineSchedule([FromBody]ScheduleLine sl)
        {
            Schadule schadule = new Schadule();
            schadule = db.Schadules.Find(x => x.IdSchadule == sl.IDDay).FirstOrDefault();
            if (sl.Day == "Work day")
            {
                schadule.Day = Enums.DayType.Workday;
            }
            else if (sl.Day == "Weekend")
            {
                schadule.Day = Enums.DayType.Weekend;
            }
            schadule.DepartureTime = sl.Time.ToString();

            db.Schadules.Update(schadule);
            db.Complete();

            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [Route("DeleteLineSchedule/{Id}")]
        // DELETE: api/Schedules/5
        [ResponseType(typeof(Schadule))]
        public IHttpActionResult DeleteLineSchedule(int Id)
        {
            Schadule schadule = new Schadule();
            schadule = db.Schadules.Find(x => x.IdSchadule == Id).FirstOrDefault();
            var lines = db.Lines.GetAll().Where(x=>x.Schadules.Contains(schadule));

            foreach (var line in lines) 
            {
           
                db.Lines.Update(line);
            }

            db.Schadules.Remove(schadule);
            db.Schadules.Update(schadule);
            db.Complete();

            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SchaduleExists(int id)
        {
            return db.Schadules.GetAll().Count(e => e.IdSchadule == id) > 0;
        }
        
    }
}
