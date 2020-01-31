using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Persistence.Repository
{
    public class LocationRepository : Repository<Location, int>, ILocationRepository
    {
        public LocationRepository(DbContext context) : base(context)
        {

        }
    }
}