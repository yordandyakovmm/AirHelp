namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refactoring : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdditionalUser",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        ClaimId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Claim", t => t.ClaimId, cascadeDelete: true)
                .Index(t => t.ClaimId);
            
            AddColumn("dbo.Claim", "referalNumber", c => c.Int(nullable: false, identity: true));
            AddColumn("dbo.AirPort", "fs", c => c.String());
            AddColumn("dbo.AirPort", "cityCode", c => c.String());
            AddColumn("dbo.AirPort", "countryCode", c => c.String());
            AddColumn("dbo.AirPort", "countryName", c => c.String());
            AddColumn("dbo.AirPort", "regionName", c => c.String());
            AddColumn("dbo.AirPort", "utcOffsetHours", c => c.Single(nullable: false));
            AddColumn("dbo.AirPort", "latitude", c => c.Single(nullable: false));
            AddColumn("dbo.AirPort", "longitude", c => c.Single(nullable: false));
            AddColumn("dbo.AirPort", "elevationFeet", c => c.Int(nullable: false));
            AddColumn("dbo.AirPort", "classification", c => c.Int(nullable: false));
            AddColumn("dbo.AirPort", "active", c => c.Boolean(nullable: false));
            AddColumn("dbo.User", "City", c => c.String());
            AddColumn("dbo.User", "Country", c => c.String());
            AddColumn("dbo.User", "Adress", c => c.String());
            AddColumn("dbo.User", "Tel", c => c.String());
            AlterColumn("dbo.Claim", "State", c => c.Int(nullable: false));
            AlterColumn("dbo.Claim", "Type", c => c.Int(nullable: false));
            AlterColumn("dbo.Claim", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Claim", "ConnectionAriports");
            DropColumn("dbo.Claim", "FirstName");
            DropColumn("dbo.Claim", "LastName");
            DropColumn("dbo.Claim", "City");
            DropColumn("dbo.Claim", "Country");
            DropColumn("dbo.Claim", "Adress");
            DropColumn("dbo.Claim", "Email");
            DropColumn("dbo.Claim", "Tel");
            DropColumn("dbo.Claim", "DepartureAirport");
            DropColumn("dbo.Claim", "DestinationAirports");
            DropColumn("dbo.Claim", "HasConnection");
            DropColumn("dbo.Claim", "ConnectionAirports");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Claim", "ConnectionAirports", c => c.String());
            AddColumn("dbo.Claim", "HasConnection", c => c.String());
            AddColumn("dbo.Claim", "DestinationAirports", c => c.String());
            AddColumn("dbo.Claim", "DepartureAirport", c => c.String());
            AddColumn("dbo.Claim", "Tel", c => c.String());
            AddColumn("dbo.Claim", "Email", c => c.String());
            AddColumn("dbo.Claim", "Adress", c => c.String());
            AddColumn("dbo.Claim", "Country", c => c.String());
            AddColumn("dbo.Claim", "City", c => c.String());
            AddColumn("dbo.Claim", "LastName", c => c.String());
            AddColumn("dbo.Claim", "FirstName", c => c.String());
            AddColumn("dbo.Claim", "ConnectionAriports", c => c.String());
            DropForeignKey("dbo.AdditionalUser", "ClaimId", "dbo.Claim");
            DropIndex("dbo.AdditionalUser", new[] { "ClaimId" });
            AlterColumn("dbo.Claim", "Date", c => c.String());
            AlterColumn("dbo.Claim", "Type", c => c.String());
            AlterColumn("dbo.Claim", "State", c => c.String());
            DropColumn("dbo.User", "Tel");
            DropColumn("dbo.User", "Adress");
            DropColumn("dbo.User", "Country");
            DropColumn("dbo.User", "City");
            DropColumn("dbo.AirPort", "active");
            DropColumn("dbo.AirPort", "classification");
            DropColumn("dbo.AirPort", "elevationFeet");
            DropColumn("dbo.AirPort", "longitude");
            DropColumn("dbo.AirPort", "latitude");
            DropColumn("dbo.AirPort", "utcOffsetHours");
            DropColumn("dbo.AirPort", "regionName");
            DropColumn("dbo.AirPort", "countryName");
            DropColumn("dbo.AirPort", "countryCode");
            DropColumn("dbo.AirPort", "cityCode");
            DropColumn("dbo.AirPort", "fs");
            DropColumn("dbo.Claim", "referalNumber");
            DropTable("dbo.AdditionalUser");
        }
    }
}
