namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateApi : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AirPortData",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        iata = c.String(maxLength: 3, unicode: false),
                        name = c.String(maxLength: 40, unicode: false),
                        city = c.String(maxLength: 40, unicode: false),
                        country = c.String(maxLength: 40, unicode: false),
                        countryCode = c.String(),
                        x = c.Double(nullable: false),
                        y = c.Double(nullable: false),
                        elevation = c.Double(nullable: false),
                        timezone = c.Double(nullable: false),
                        url = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.iata)
                .Index(t => t.name)
                .Index(t => t.city)
                .Index(t => t.country);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.AirPortData", new[] { "country" });
            DropIndex("dbo.AirPortData", new[] { "city" });
            DropIndex("dbo.AirPortData", new[] { "name" });
            DropIndex("dbo.AirPortData", new[] { "iata" });
            DropTable("dbo.AirPortData");
        }
    }
}
