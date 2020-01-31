namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TicketMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tickets", "UserName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tickets", "UserName");
        }
    }
}
