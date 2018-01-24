namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class signitureImage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Claim", "SignitureImage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Claim", "SignitureImage");
        }
    }
}
