namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Claim : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Users", newName: "User");
            CreateTable(
                "dbo.Claim",
                c => new
                    {
                        ClaimId = c.Guid(nullable: false),
                        State = c.String(),
                        UserId = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        BordCardUrl = c.String(),
                        BookConfirmationUrl = c.String(),
                        Type = c.String(),
                        ConnectionAriports = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        City = c.String(),
                        Country = c.String(),
                        Adress = c.String(),
                        Email = c.String(),
                        Tel = c.String(),
                        FlightNumber = c.String(),
                        Date = c.String(),
                        DepartureAirport = c.String(),
                        DestinationAirports = c.String(),
                        HasConnection = c.String(),
                        ConnectionAirports = c.String(),
                        Reason = c.String(),
                        HowMuch = c.String(),
                        Annonsment = c.String(),
                        BookCode = c.String(),
                        AirCompany = c.String(),
                        AdditionalInfo = c.String(),
                        Confirm = c.String(),
                    })
                .PrimaryKey(t => t.ClaimId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Claim", "UserId", "dbo.User");
            DropIndex("dbo.Claim", new[] { "UserId" });
            DropTable("dbo.Claim");
            RenameTable(name: "dbo.User", newName: "Users");
        }
    }
}
