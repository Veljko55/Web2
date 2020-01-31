namespace WebApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SchaduleLineMigration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Schadules", "Line_IdLine", "dbo.Lines");
            DropForeignKey("dbo.Schadules", "IdLine", "dbo.Lines");
            DropIndex("dbo.Schadules", new[] { "IdLine" });
            DropIndex("dbo.Schadules", new[] { "Line_IdLine" });
            DropColumn("dbo.Schadules", "Line_IdLine");
            RenameColumn(table: "dbo.Schadules", name: "IdLine", newName: "Line_IdLine");
            AddColumn("dbo.Schadules", "Line_IdLine1", c => c.Int());
            AlterColumn("dbo.Schadules", "Line_IdLine", c => c.Int());
            CreateIndex("dbo.Schadules", "Line_IdLine");
            CreateIndex("dbo.Schadules", "Line_IdLine1");
            AddForeignKey("dbo.Schadules", "Line_IdLine1", "dbo.Lines", "IdLine");
            AddForeignKey("dbo.Schadules", "Line_IdLine", "dbo.Lines", "IdLine");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Schadules", "Line_IdLine", "dbo.Lines");
            DropForeignKey("dbo.Schadules", "Line_IdLine1", "dbo.Lines");
            DropIndex("dbo.Schadules", new[] { "Line_IdLine1" });
            DropIndex("dbo.Schadules", new[] { "Line_IdLine" });
            AlterColumn("dbo.Schadules", "Line_IdLine", c => c.Int(nullable: false));
            DropColumn("dbo.Schadules", "Line_IdLine1");
            RenameColumn(table: "dbo.Schadules", name: "Line_IdLine", newName: "IdLine");
            AddColumn("dbo.Schadules", "Line_IdLine", c => c.Int());
            CreateIndex("dbo.Schadules", "Line_IdLine");
            CreateIndex("dbo.Schadules", "IdLine");
            AddForeignKey("dbo.Schadules", "IdLine", "dbo.Lines", "IdLine", cascadeDelete: true);
            AddForeignKey("dbo.Schadules", "Line_IdLine", "dbo.Lines", "IdLine");
        }
    }
}
