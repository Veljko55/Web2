namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateNewModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Lines",
                c => new
                    {
                        IdLine = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                        RouteType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdLine);
            
            CreateTable(
                "dbo.Stations",
                c => new
                    {
                        IdStation = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Address = c.String(),
                        X = c.Double(nullable: false),
                        Y = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.IdStation);
            
            CreateTable(
                "dbo.PriceLists",
                c => new
                    {
                        IdPriceList = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        InUse = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.IdPriceList);
            
            CreateTable(
                "dbo.Prices",
                c => new
                    {
                        IdPrice = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Value = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.IdPrice);
            
            CreateTable(
                "dbo.Schadules",
                c => new
                    {
                        IdSchadule = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Day = c.Int(nullable: false),
                        DepartureTime = c.String(),
                        IdLine = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdSchadule)
                .ForeignKey("dbo.Lines", t => t.IdLine, cascadeDelete: true)
                .Index(t => t.IdLine);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        IdTicket = c.Int(nullable: false, identity: true),
                        From = c.DateTime(nullable: false),
                        To = c.DateTime(nullable: false),
                        Passenger = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdTicket);
            
            CreateTable(
                "dbo.StationLines",
                c => new
                    {
                        Station_IdStation = c.Int(nullable: false),
                        Line_IdLine = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Station_IdStation, t.Line_IdLine })
                .ForeignKey("dbo.Stations", t => t.Station_IdStation, cascadeDelete: true)
                .ForeignKey("dbo.Lines", t => t.Line_IdLine, cascadeDelete: true)
                .Index(t => t.Station_IdStation)
                .Index(t => t.Line_IdLine);
            
            CreateTable(
                "dbo.PricePriceLists",
                c => new
                    {
                        Price_IdPrice = c.Int(nullable: false),
                        PriceList_IdPriceList = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Price_IdPrice, t.PriceList_IdPriceList })
                .ForeignKey("dbo.Prices", t => t.Price_IdPrice, cascadeDelete: true)
                .ForeignKey("dbo.PriceLists", t => t.PriceList_IdPriceList, cascadeDelete: true)
                .Index(t => t.Price_IdPrice)
                .Index(t => t.PriceList_IdPriceList);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Schadules", "IdLine", "dbo.Lines");
            DropForeignKey("dbo.PricePriceLists", "PriceList_IdPriceList", "dbo.PriceLists");
            DropForeignKey("dbo.PricePriceLists", "Price_IdPrice", "dbo.Prices");
            DropForeignKey("dbo.StationLines", "Line_IdLine", "dbo.Lines");
            DropForeignKey("dbo.StationLines", "Station_IdStation", "dbo.Stations");
            DropIndex("dbo.PricePriceLists", new[] { "PriceList_IdPriceList" });
            DropIndex("dbo.PricePriceLists", new[] { "Price_IdPrice" });
            DropIndex("dbo.StationLines", new[] { "Line_IdLine" });
            DropIndex("dbo.StationLines", new[] { "Station_IdStation" });
            DropIndex("dbo.Schadules", new[] { "IdLine" });
            DropTable("dbo.PricePriceLists");
            DropTable("dbo.StationLines");
            DropTable("dbo.Tickets");
            DropTable("dbo.Schadules");
            DropTable("dbo.Prices");
            DropTable("dbo.PriceLists");
            DropTable("dbo.Stations");
            DropTable("dbo.Lines");
        }
    }
}
