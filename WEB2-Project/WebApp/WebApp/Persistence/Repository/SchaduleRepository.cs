using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Persistence.Repository
{
    public class SchaduleRepository : Repository<Schadule, int>, ISchaduleRepository
    {
        public SchaduleRepository(DbContext context) : base(context)
        {

        }
    }
}