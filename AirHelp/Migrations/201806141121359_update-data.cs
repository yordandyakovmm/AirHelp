namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatedata : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AirPortData", new[] { "iata" });
            AlterColumn("dbo.AirPortData", "iata", c => c.String(maxLength: 4, unicode: false));
            CreateIndex("dbo.AirPortData", "iata");
        }
        
        public override void Down()
        {
            DropIndex("dbo.AirPortData", new[] { "iata" });
            AlterColumn("dbo.AirPortData", "iata", c => c.String(maxLength: 3, unicode: false));
            CreateIndex("dbo.AirPortData", "iata");
        }
    }
}
