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
    [RoutePrefix("api/Line")]
    public class LinesController : ApiController
    {
        private IUnitOfWork db;

        public LinesController()
        {

        }

        public LinesController(IUnitOfWork db)
        {

            

            this.db = db;
        }

        [AllowAnonymous]
        [Route("GetLines")]
        public IEnumerable<LinePlus> GetLines()
        {
            List<Line> lines = db.Lines.GetAll().ToList();
            List<LinePlus> ret = new List<LinePlus>();

            foreach (Line l in lines)
            {
                RouteType type = l.RouteType; //db.TypesOfLine.GetAll().FirstOrDefault(u => u.IDtypeOfLine == l.IDtypeOfLine).typeOfLine;
                LinePlus lp = new LinePlus() { Number = l.Number, IDtypeOfLine = 0, TypeOfLine = type.ToString(), Stations = l.Stations};
                ret.Add(lp);
            }

            return ret;
        }

        [AllowAnonymous]
        [Route("GetScheduleLines")]
        public IEnumerable<LinePlus> GetScheduleLines(string typeOfLine)
        {

            if (typeOfLine == null)
            {
                var type = db.Lines.GetAll().FirstOrDefault(u => u.RouteType == Enums.RouteType.Town);                
                List<Line> lines = db.Lines.GetAll().ToList();
                List<LinePlus> ret = new List<LinePlus>();

                foreach (Line l in lines)
                {
                    
                    LinePlus lp = new LinePlus() { Number = l.Number, IDtypeOfLine = 0, TypeOfLine = type.ToString(), Stations = l.Stations };
                    ret.Add(lp);
                }

                return ret;
            }
            else
            {
                
                RouteType type = Enums.RouteType.Town;
                if (typeOfLine == "Town")
                {
                    type = Enums.RouteType.Town;
                }
                else if (typeOfLine == "Suburban")
                {
                    type = Enums.RouteType.Suburban;
                }
                List<Line> lines = db.Lines.GetAll().ToList();
                List<LinePlus> ret = new List<LinePlus>();

                foreach (Line l in lines)
                {
                    if (l.RouteType == type)
                    {
                        
                        LinePlus lp = new LinePlus() { Number = l.Number, IDtypeOfLine = 0, TypeOfLine = type.ToString(), Stations = l.Stations };
                        ret.Add(lp);
                    }
                }

                return ret;
                
            }
        }
        

        [AllowAnonymous]
        [Route("GetSchedule")]
        public IEnumerable<ScheduleLine> GetSchedule(string typeOfLine, string typeOfDay, string Number)
        {

            if (typeOfLine == null || typeOfDay == null || Number == null)
            {
                //return BadRequest();
            }

            RouteType type = Enums.RouteType.Town;
            if (typeOfLine == "Town")
            {
                type = Enums.RouteType.Town;
            }
            else if (typeOfLine == "Suburban")
            {
                type = Enums.RouteType.Suburban;
            }

            DayType day = DayType.Workday;
            if (typeOfDay == "Work day")
            {
                day = Enums.DayType.Workday;
            }
            else if (typeOfDay == "Suburban")
            {
                day = Enums.DayType.Weekend;
            }

            List<ScheduleLine> schedule = new List<ScheduleLine>();
            var lines = db.Lines.GetAll();
            foreach (var line in lines)
            {
                if(line.Number == Number)
                {
                    foreach (var dep in line.Schadules)
                    {
                       

                        ScheduleLine sl = new ScheduleLine();
                        sl.Number = line.Number;
                        sl.Time = DateTime.Parse(dep.DepartureTime);
                        if (dep.Day == DayType.Weekend)
                            sl.Day = "Weekend";
                        else if (true)
                            sl.Day = "Work day";

                        sl.IDDay = dep.IdSchadule;
                        schedule.Add(sl);
                    }
                }
            }

            return schedule;
        }

        [Authorize(Roles = "Admin")]
        [Route("GetScheduleAdmin")]
        public IEnumerable<ScheduleLine> GetScheduleAdmin()
        {
            var sc = db.Schadules.GetAll();
            
            List<ScheduleLine> schedule = new List<ScheduleLine>();
            var lines = db.Lines.GetAll();
            foreach (var line in lines)
            {
                foreach (var dep in line.Schadules)
                {
                    

                    ScheduleLine sl = new ScheduleLine();
                    sl.Number = line.Number;
                    sl.Time = DateTime.Parse(dep.DepartureTime);
                    if (dep.Day == DayType.Weekend)
                        sl.Day = "Weekend";
                    else if (dep.Day == DayType.Workday)
                        sl.Day = "Work day";

                    sl.IDDay = dep.IdSchadule;
                    schedule.Add(sl);
                }
            }

            return schedule;
        }

        //// GET: api/Lines/5
        [ResponseType(typeof(Line))]
        public IHttpActionResult GetLine(string id)
        {
            List<Line> lines = db.Lines.GetAll().ToList();
            Line line = null;

            foreach (var l in lines)
            {
                if (id == l.Number)
                {
                    line = db.Lines.Get(l.IdLine);
                }
            }

            if (line == null)
            {
                return NotFound();
            }

            return Ok(line);
        }

        [Authorize(Roles = "Admin")]
        [Route("AddLine")]
        public string AddLine(LinePlus linePlus)
        {
            Line line = db.Lines.GetAll().FirstOrDefault(u => u.Number == linePlus.Number);


            if (line != null)
            {
                return "Line with that number already exist";
            }
            else
            {
                RouteType id = RouteType.Town;

                if (linePlus.TypeOfLine == "Town")
                {
                    id = Enums.RouteType.Town;
                }
                else if (linePlus.TypeOfLine == "Suburban")
                {
                    id = Enums.RouteType.Suburban;
                }
                
                Line newLine = new Line() { Number = linePlus.Number, RouteType = id };
                newLine.Stations = new List<Station>();
                foreach (Station s in linePlus.Stations)
                {
                    var station = db.Stations.GetAll().FirstOrDefault(u => u.Name == s.Name);
                    newLine.Stations.Add(station);
                    db.Stations.Update(station);
                }

                db.Lines.Add(newLine);
                try
                {
                    db.Complete();
                }
                catch (Exception e)
                {

                }
            }

            return "ok";
        }

        [Authorize(Roles = "Admin")]
        [Route("EditLine")]
        public string EditLine(LinePlus linePlus)
        {
            int result = 1;
            Line line = db.Lines.GetAll().FirstOrDefault(u => u.Number == linePlus.Number);

            if (line == null)
            {
                return "Line can't be changed";
            }
            else
            {
                if (line.Number != linePlus.Number)
                {
                    return "Data was modified in meantime, please try again!";
                }


                line.Stations = new List<Station>();
                if (linePlus.Stations != null)
                {
                    foreach (Station s in linePlus.Stations)
                    {
                        var station = db.Stations.GetAll().FirstOrDefault(u => u.Name == s.Name);
                        line.Stations.Add(station);
                        db.Stations.Update(station);
                    }
                }

                if (linePlus.TypeOfLine == "Town")
                {
                    line.RouteType = Enums.RouteType.Town;
                }
                else if(linePlus.TypeOfLine == "Suburban")
                {
                    line.RouteType = Enums.RouteType.Suburban;
                }

                db.Lines.Update(line);
                result = db.Complete();
                if (result == 0)
                {
                    return "conflict";
                }
                else if (result == -1)
                {
                    return "Data was modified in meantime, please try again!";
                }
            }

            return "ok";
        }

        [Authorize(Roles = "Admin")]
        [Route("PostLineSchedule")]
        // POST: api/Lines
        // [ResponseType(typeof(Line))]
        public IHttpActionResult PostLine([FromBody] ScheduleLine sl)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            DayType idd = DayType.Workday;

            if (sl.Day == "Work day")
                idd = DayType.Workday; 
            else
                idd = DayType.Weekend; 

            Schadule s = new Schadule { Day = idd, DepartureTime = sl.Time.ToString() };
            var line = db.Lines.GetAll().FirstOrDefault(u => u.Number == sl.Number);
            if (s.Lines == null)
                s.Lines = new List<Line>();
            s.Lines.Add(line);

            db.Schadules.Add(s);

            line.Schadules.Add(s);
            db.Lines.Update(line);

            try
            {
                db.Complete();
            }
            catch (DbUpdateException)
            {

            }

            return CreatedAtRoute("DefaultApi", new { id = line.Number }, line);
        }

        [Authorize(Roles = "Admin")]
        [Route("DeleteLine/{Number}")]
        // DELETE: api/Lines/5
        [ResponseType(typeof(Line))]
        public IHttpActionResult DeleteLine(string Number)
        {
            
            List<Line> lines = db.Lines.GetAll().ToList();
            Line line = null;

            foreach (var l in lines)
            {
                if (Number == l.Number)
                {
                    line = db.Lines.Get(l.IdLine);
                }
            }
            if (line == null)
            {
                return NotFound();
            }

            db.Lines.Remove(line);
            db.Complete();

            return Ok(line);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LineExists(string id)
        {
            return db.Lines.GetAll().Count(e => e.Number == id) > 0;

        }
    }
}
