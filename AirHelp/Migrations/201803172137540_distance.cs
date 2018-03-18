namespace AirHelp.DAL.Migration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class distance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Claim", "distance", c => c.Double(nullable: false));
            AddColumn("dbo.AirPort", "distance", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AirPort", "distance");
            DropColumn("dbo.Claim", "distance");
        }
    }
}
