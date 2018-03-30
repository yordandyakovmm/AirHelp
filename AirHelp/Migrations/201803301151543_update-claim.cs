namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateclaim : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Claim", "IsEUFlight", c => c.Int(nullable: false));
            AddColumn("dbo.Claim", "FlightType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Claim", "FlightType");
            DropColumn("dbo.Claim", "IsEUFlight");
        }
    }
}
