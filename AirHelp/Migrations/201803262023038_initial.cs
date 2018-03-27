namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
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
            
            CreateTable(
                "dbo.Claim",
                c => new
                    {
                        ClaimId = c.Guid(nullable: false),
                        referalNumber = c.Int(nullable: false, identity: true),
                        State = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        DateCreated = c.DateTime(nullable: false),
                        AttorneyUrl = c.String(),
                        Type = c.Int(nullable: false),
                        FlightNumber = c.String(),
                        Date = c.DateTime(nullable: false),
                        allDistance = c.Double(nullable: false),
                        issueDistance = c.Double(nullable: false),
                        Reason = c.Int(nullable: false),
                        DelayDelay = c.Int(nullable: false),
                        CancelDelay = c.Int(nullable: false),
                        CancelAnnonsment = c.Int(nullable: false),
                        DenayArival = c.Int(nullable: false),
                        DocumentSecurity = c.Int(nullable: false),
                        Willness = c.Int(nullable: false),
                        BookingCode = c.String(),
                        TikedNumber = c.String(),
                        AirCompany = c.String(),
                        AirCompanyCountry = c.String(),
                        AirCompanyCode = c.String(),
                        AdditionalInfo = c.String(),
                        Confirm = c.String(),
                        SignitureImage = c.String(),
                    })
                .PrimaryKey(t => t.ClaimId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AirPort",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ClaimId = c.Guid(nullable: false),
                        FlightNumber = c.String(),
                        FlightDate = c.String(),
                        number = c.Int(nullable: false),
                        iata = c.String(),
                        name = c.String(),
                        city = c.String(),
                        country = c.String(),
                        icao = c.String(),
                        x = c.Double(nullable: false),
                        y = c.Double(nullable: false),
                        elevation = c.Double(nullable: false),
                        timezone = c.Double(nullable: false),
                        type = c.String(),
                        ap_name = c.String(),
                        fs = c.String(),
                        cityCode = c.String(),
                        countryCode = c.String(),
                        countryName = c.String(),
                        regionName = c.String(),
                        utcOffsetHours = c.Single(nullable: false),
                        latitude = c.Single(nullable: false),
                        longitude = c.Single(nullable: false),
                        elevationFeet = c.Int(nullable: false),
                        classification = c.Int(nullable: false),
                        active = c.Boolean(nullable: false),
                        distanceToNext = c.Double(nullable: false),
                        startIssue = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Claim", t => t.ClaimId, cascadeDelete: true)
                .Index(t => t.ClaimId);
            
            CreateTable(
                "dbo.Document",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DocumentName = c.String(),
                        Url = c.String(),
                        ClaimId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Claim", t => t.ClaimId, cascadeDelete: true)
                .Index(t => t.ClaimId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        City = c.String(),
                        Country = c.String(),
                        Adress = c.String(),
                        Tel = c.String(),
                        PictureUrl = c.String(),
                        Role = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        password = c.String(),
                        type = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Claim", "UserId", "dbo.User");
            DropForeignKey("dbo.Document", "ClaimId", "dbo.Claim");
            DropForeignKey("dbo.AirPort", "ClaimId", "dbo.Claim");
            DropForeignKey("dbo.AdditionalUser", "ClaimId", "dbo.Claim");
            DropIndex("dbo.Document", new[] { "ClaimId" });
            DropIndex("dbo.AirPort", new[] { "ClaimId" });
            DropIndex("dbo.Claim", new[] { "UserId" });
            DropIndex("dbo.AdditionalUser", new[] { "ClaimId" });
            DropTable("dbo.User");
            DropTable("dbo.Document");
            DropTable("dbo.AirPort");
            DropTable("dbo.Claim");
            DropTable("dbo.AdditionalUser");
        }
    }
}
