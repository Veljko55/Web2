namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class LineScheduleMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Lines", "Schadule_IdSchadule", c => c.Int());
            AddColumn("dbo.Schadules", "Line_IdLine", c => c.Int());
            CreateIndex("dbo.Lines", "Schadule_IdSchadule");
            CreateIndex("dbo.Schadules", "Line_IdLine");
            AddForeignKey("dbo.Lines", "Schadule_IdSchadule", "dbo.Schadules", "IdSchadule");
            AddForeignKey("dbo.Schadules", "Line_IdLine", "dbo.Lines", "IdLine");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Schadules", "Line_IdLine", "dbo.Lines");
            DropForeignKey("dbo.Lines", "Schadule_IdSchadule", "dbo.Schadules");
            DropIndex("dbo.Schadules", new[] { "Line_IdLine" });
            DropIndex("dbo.Lines", new[] { "Schadule_IdSchadule" });
            DropColumn("dbo.Schadules", "Line_IdLine");
            DropColumn("dbo.Lines", "Schadule_IdSchadule");
        }
    }
}
