namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class merge : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Claim", "AttorneyUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Claim", "AttorneyUrl");
        }
    }
}
