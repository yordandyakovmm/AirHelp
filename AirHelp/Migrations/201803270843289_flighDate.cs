namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class flighDate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Claim", "FlightDate", c => c.String());
            DropColumn("dbo.Claim", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Claim", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Claim", "FlightDate");
        }
    }
}
