namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AirCompany : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Claim", "AirCompanyCountry", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Claim", "AirCompanyCountry");
        }
    }
}
