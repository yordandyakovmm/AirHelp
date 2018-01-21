namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class miisingfields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Claim", "Arival", c => c.String());
            AddColumn("dbo.Claim", "DocumentSecurity", c => c.String());
            AddColumn("dbo.Claim", "Willness", c => c.String());
            AddColumn("dbo.Claim", "Delay", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Claim", "Delay");
            DropColumn("dbo.Claim", "Willness");
            DropColumn("dbo.Claim", "DocumentSecurity");
            DropColumn("dbo.Claim", "Arival");
        }
    }
}
