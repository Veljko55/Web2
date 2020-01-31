namespace WebApp.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebApp.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<WebApp.Persistence.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WebApp.Persistence.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var t1 = new Ticket { IdTicket = 1, Type = Enums.TicketType.Hourly, From = DateTime.Now, To = DateTime.Now };
            var t2 = new Ticket { IdTicket = 2, Type = Enums.TicketType.Daily, From = DateTime.Now, To = DateTime.Now };
            var t3 = new Ticket { IdTicket = 3, Type = Enums.TicketType.Monthly, From = DateTime.Now, To = DateTime.Now };
            var t4 = new Ticket { IdTicket = 4, Type = Enums.TicketType.Annual, From = DateTime.Now, To = DateTime.Now };

            context.Tickets.AddOrUpdate(a => a.IdTicket, t1);
            context.Tickets.AddOrUpdate(a => a.IdTicket, t2);
            context.Tickets.AddOrUpdate(a => a.IdTicket, t3);
            context.Tickets.AddOrUpdate(a => a.IdTicket, t4);

            context.SaveChanges();



            var s1 = new Station { IdStation = 1, Name = "Grbavica", Address = "Puskinova", X = 0, Y = 0 };
            var s2 = new Station { IdStation = 2, Name = "Liman", Address = "Narodnog fronta", X = 0, Y = 0 };
            var s3 = new Station { IdStation = 3, Name = "Klisa", Address = "Tolstojeva", X = 0, Y = 0 };
            var s4 = new Station { IdStation = 4, Name = "Podbara", Address = "Kosovska", X = 0, Y = 0 };
            var s5 = new Station { IdStation = 5, Name = "Liman", Address = "Vojvodjanskih brigada", X = 0, Y = 0 };

            context.Stations.AddOrUpdate(s => s.IdStation, s1);
            context.Stations.AddOrUpdate(s => s.IdStation, s2);
            context.Stations.AddOrUpdate(s => s.IdStation, s3);
            context.Stations.AddOrUpdate(s => s.IdStation, s4);
            context.Stations.AddOrUpdate(s => s.IdStation, s5);

            context.SaveChanges();

            List<Station> stations = new List<Station>();
            stations.Add(s1);
            stations.Add(s2);
            stations.Add(s3);
            stations.Add(s4);
            stations.Add(s5);

            var r1 = new Line { IdLine = 1, Number = "4a", RouteType = Enums.RouteType.Town, Stations = stations };
            //var r2 = new Line { IdLine = 2, Number = "56b", RouteType = Enums.RouteType.Suburban, Stations = stations };
            var r3 = new Line { IdLine = 3, Number = "7b", RouteType = Enums.RouteType.Town, Stations = stations };
            var r4 = new Line { IdLine = 4, Number = "54a", RouteType = Enums.RouteType.Suburban, Stations = stations };
            var r5 = new Line { IdLine = 5, Number = "11a", RouteType = Enums.RouteType.Town, Stations = stations };

            context.Lines.AddOrUpdate(r => r.IdLine, r1);
            //context.Lines.AddOrUpdate(r => r.IdLine, r2);
            context.Lines.AddOrUpdate(r => r.IdLine, r3);
            context.Lines.AddOrUpdate(r => r.IdLine, r4);
            context.Lines.AddOrUpdate(r => r.IdLine, r5);
            context.SaveChanges();

            var sc1 = new Schadule { IdSchadule = 1, Day = Enums.DayType.Weekend, Type = Enums.RouteType.Suburban,  DepartureTime = "07-07:10|07:45-08:00|08:45-09:00" };
            var sc2 = new Schadule { IdSchadule = 2, Day = Enums.DayType.Workday, Type = Enums.RouteType.Town,  DepartureTime = "10:15-10:45|10:30-11:00" };
            var sc3 = new Schadule { IdSchadule = 3, Day = Enums.DayType.Weekend, Type = Enums.RouteType.Suburban,  DepartureTime = "14-14:10|15:15-16:05" };
            var sc4 = new Schadule { IdSchadule = 4, Day = Enums.DayType.Workday, Type = Enums.RouteType.Suburban,  DepartureTime = "17-17:10|17:15-18:05" };
            var sc5 = new Schadule { IdSchadule = 5, Day = Enums.DayType.Workday, Type = Enums.RouteType.Town,  DepartureTime = "14-14:10|15:15-16:05" };

            context.Schadules.AddOrUpdate(sc => sc.IdSchadule, sc1);
            context.Schadules.AddOrUpdate(sc => sc.IdSchadule, sc2);
            context.Schadules.AddOrUpdate(sc => sc.IdSchadule, sc3);
            context.Schadules.AddOrUpdate(sc => sc.IdSchadule, sc4);
            context.Schadules.AddOrUpdate(sc => sc.IdSchadule, sc5);
            context.SaveChanges();

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Admin" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "Controller"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "Controller" };

                manager.Create(role);
            }

            if (!context.Roles.Any(r => r.Name == "AppUser"))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole { Name = "AppUser" };

                manager.Create(role);
            }

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if (!context.Users.Any(u => u.UserName == "admin@yahoo.com"))
            {
                var user = new ApplicationUser() { Id = "admin", BirthdayDate = DateTime.Now, UserName = "admin@yahoo.com", Email = "admin@yahoo.com", PasswordHash = ApplicationUser.HashPassword("Admin123!") };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "Admin");
            }

            if (!context.Users.Any(u => u.UserName == "appu@yahoo.com"))
            { 
                var user = new ApplicationUser() { Id = "appu", BirthdayDate = DateTime.Now, UserName = "appu@yahoo.com", Email = "appu@yahoo.com", PasswordHash = ApplicationUser.HashPassword("Appu123!") };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "AppUser");
            }

            if (!context.Users.Any(u => u.UserName == "kontroler@yahoo.com"))
            {
                var user = new ApplicationUser() { Id = "kontroler", BirthdayDate = DateTime.Now, UserName = "kontroler@yahoo.com", Email = "kontroler@yahoo.com", PasswordHash = ApplicationUser.HashPassword("Kontroler123!") };
                userManager.Create(user);
                userManager.AddToRole(user.Id, "Controller");
            }
        }
    }
}
