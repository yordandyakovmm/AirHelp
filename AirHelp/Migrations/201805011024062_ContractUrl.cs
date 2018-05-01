namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ContractUrl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Claim", "contractUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Claim", "contractUrl");
        }
    }
}
