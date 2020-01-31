namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedLocation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Locations",
                c => new
                {
                    IdLocation = c.Int(nullable: false, identity: true),
                    Address = c.String(),
                    X = c.Double(nullable: false),
                    Y = c.Double(nullable: false),
                })
                .PrimaryKey(t => t.IdLocation);

        }

        public override void Down()
        {
            DropTable("dbo.Locations");
        }
    }
}
