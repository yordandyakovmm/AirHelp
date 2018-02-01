namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class airpoirt : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AirPort",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ClaimId = c.Guid(nullable: false),
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Claim", t => t.ClaimId, cascadeDelete: true)
                .Index(t => t.ClaimId);
            
            AddColumn("dbo.User", "password", c => c.String());
            AddColumn("dbo.User", "type", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AirPort", "ClaimId", "dbo.Claim");
            DropIndex("dbo.AirPort", new[] { "ClaimId" });
            DropColumn("dbo.User", "type");
            DropColumn("dbo.User", "password");
            DropTable("dbo.AirPort");
        }
    }
}
