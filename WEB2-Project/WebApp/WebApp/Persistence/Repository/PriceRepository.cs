using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Persistence.Repository
{
    public class PriceRepository : Repository<Price, int>, IPriceRepository
    {
        public PriceRepository(DbContext context) : base(context)
        {

        }
    }
}